using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Shared;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a file uploaded through a file input control.
/// Contains file metadata and optional content data for processing uploaded files.
/// </summary>
public record FileInput
{
    /// <summary>Gets the name of the uploaded file including its extension.</summary>
    /// <value>The original filename as provided by the user's browser.</value>
    public required string Name { get; init; }

    /// <summary>Gets the MIME type of the uploaded file.</summary>
    /// <value>The file's MIME type (e.g., "image/jpeg", "text/plain", "application/pdf").</value>
    public required string Type { get; init; }

    /// <summary>Gets the size of the uploaded file in bytes.</summary>
    /// <value>The file size in bytes.</value>
    public int Size { get; init; }

    /// <summary>Gets the last modified date of the uploaded file.</summary>
    /// <value>The date and time when the file was last modified.</value>
    public DateTime LastModified { get; init; }

    /// <summary>Gets the binary content of the uploaded file.</summary>
    /// <value>The file content as a byte array, or null if content was not loaded. Ignored during JSON serialization when null or empty.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? Content { get; init; }
}

/// <summary>
/// Defines the visual variants available for file input controls.
/// Currently provides a single drag-and-drop variant with potential for future expansion.
/// </summary>
public enum FileInputs
{
    /// <summary>Drag-and-drop file input interface for uploading files by dropping them onto the control.</summary>
    Drop
}

/// <summary>
/// Interface for file input controls that extends IAnyInput with file-specific properties.
/// Provides functionality for file upload including placeholder text and visual variants
/// for different file selection methods.
/// </summary>
public interface IAnyFileInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when no files are selected.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual variant of the file input.</summary>
    /// <value>The input variant (currently only Drop is available).</value>
    public FileInputs Variant { get; set; }
}

/// <summary>
/// Abstract base class for file input controls that provides comprehensive file upload functionality.
/// Supports single and multiple file uploads with file type validation, size limits, and drag-and-drop interfaces.
/// Includes built-in validation for file types and count restrictions.
/// </summary>
public abstract record FileInputBase : WidgetBase<FileInputBase>, IAnyFileInput
{
    /// <summary>Gets or sets whether the input is disabled.</summary>
    /// <value>true if the input is disabled; false if it's interactive.</value>
    [Prop] public bool Disabled { get; set; }

    /// <summary>Gets or sets the validation error message.</summary>
    /// <value>The error message, or null if the input is valid.</value>
    [Prop] public string? Invalid { get; set; }

    /// <summary>Gets or sets the placeholder text displayed when no files are selected.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    [Prop] public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual variant of the file input.</summary>
    /// <value>The input variant (currently only Drop is available).</value>
    [Prop] public FileInputs Variant { get; set; }

    /// <summary>Gets or sets the accepted file types using MIME types or file extensions.</summary>
    /// <value>A comma-separated list of accepted file types (e.g., "image/*", ".pdf,.doc", "text/plain").</value>
    [Prop] public string? Accept { get; set; }

    /// <summary>Gets or sets whether multiple files can be selected.</summary>
    /// <value>true if multiple files are allowed; false for single file selection.</value>
    [Prop] public bool Multiple { get; set; }

    /// <summary>Gets or sets the maximum number of files that can be selected (only applicable when Multiple is true).</summary>
    /// <value>The maximum file count, or null for no limit.</value>
    [Prop] public int? MaxFiles { get; set; }

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IAnyInput>, ValueTask>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this file input can bind to.
    /// File inputs use custom type validation and don't support standard state type binding.
    /// </summary>
    /// <returns>An empty array as file inputs use custom validation logic.</returns>
    public Type[] SupportedStateTypes() => [];

    /// <summary>
    /// Validates the current file input value against configured restrictions.
    /// Performs validation for file types (using Accept property) and file count limits (using MaxFiles property).
    /// </summary>
    /// <param name="value">The current value to validate (FileInput for single files, IEnumerable&lt;FileInput&gt; for multiple files).</param>
    /// <returns>A ValidationResult indicating whether the value is valid and any error messages.</returns>
    public ValidationResult ValidateValue(object? value)
    {
        if (value == null) return ValidationResult.Success();

        if (value is FileInput file)
        {
            return FileInputValidation.ValidateFileType(file, Accept);
        }
        else if (value is IEnumerable<FileInput> files)
        {
            var filesList = files.ToList();

            // Validate file count first if MaxFiles is set
            if (MaxFiles.HasValue)
            {
                var countValidation = FileInputValidation.ValidateFileCount(filesList, MaxFiles);
                if (!countValidation.IsValid)
                {
                    return countValidation;
                }
            }

            // Then validate file types if Accept is set
            if (!string.IsNullOrWhiteSpace(Accept))
            {
                return FileInputValidation.ValidateFileTypes(filesList, Accept);
            }
        }

        return ValidationResult.Success();
    }
}

/// <summary>
/// Generic file input control that provides type-safe file upload functionality.
/// Supports both single file (FileInput) and multiple file (IEnumerable&lt;FileInput&gt;) scenarios
/// with automatic validation, drag-and-drop interface, and configurable file restrictions.
/// </summary>
/// <typeparam name="TValue">The type of the file value (FileInput for single files, IEnumerable&lt;FileInput&gt; for multiple files).</typeparam>
public record FileInput<TValue> : FileInputBase, IInput<TValue>, IAnyFileInput
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// Includes automatic validation when Accept or MaxFiles properties are configured.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when no files are selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the file input.</param>
    [OverloadResolutionPriority(1)]
    public FileInput(IAnyState state, string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TValue>();
        Value = typedState.Value;
        OnChange = e =>
        {
            typedState.Set(e.Value);

            // Auto-validate if Accept or MaxFiles is set
            if (!string.IsNullOrWhiteSpace(Accept) || MaxFiles.HasValue)
            {
                var validation = ValidateValue(e.Value);
                if (!validation.IsValid)
                {
                    Invalid = validation.ErrorMessage;
                }
                else
                {
                    Invalid = null;
                }
            }
            return ValueTask.CompletedTask;
        };
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and async change handler.
    /// </summary>
    /// <param name="value">The initial file value.</param>
    /// <param name="onChange">Optional async event handler called when the file value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when no files are selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the file input.</param>
    [OverloadResolutionPriority(1)]
    public FileInput(TValue value, Func<Event<IInput<TValue>, TValue>, ValueTask>? onChange, string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and synchronous change handler.
    /// Compatibility overload for Action-based change handlers.
    /// </summary>
    /// <param name="value">The initial file value.</param>
    /// <param name="onChange">Optional event handler called when the file value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when no files are selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the file input.</param>
    public FileInput(TValue value, Action<Event<IInput<TValue>, TValue>>? onChange, string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange == null ? null : e => { onChange(e); return ValueTask.CompletedTask; };
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// Sets up the file input with default dimensions suitable for drag-and-drop file operations.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when no files are selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the file input.</param>
    public FileInput(string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
    {
        Placeholder = placeholder;
        Variant = variant;
        Disabled = disabled;
        Width = Size.Full();
        Height = Size.Units(50);
    }

    /// <summary>Gets the current file value.</summary>
    /// <value>The file value of the specified type (FileInput or IEnumerable&lt;FileInput&gt;).</value>
    [Prop] public TValue Value { get; } = default!;

    /// <summary>Gets the event handler called when the file value changes.</summary>
    /// <value>The async change event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IInput<TValue>, TValue>, ValueTask>? OnChange { get; }
}

/// <summary>
/// Provides extension methods for creating, configuring, and working with file inputs.
/// Includes utility methods for file content processing, validation, and fluent configuration
/// of file upload controls with type safety and automatic multiple file detection.
/// </summary>
public static class FileInputExtensions
{
    /// <summary>
    /// Converts the file content to plain text using UTF-8 encoding.
    /// Useful for processing text files uploaded through the file input.
    /// </summary>
    /// <param name="file">The file input containing the content to convert.</param>
    /// <returns>The file content as a UTF-8 string, or null if the file has no content or is empty.</returns>
    public static string? ToPlainText(this FileInput file)
    {
        if (file.Content == null)
        {
            return null;
        }
        return file.Content.Length switch
        {
            0 => null,
            _ => Encoding.UTF8.GetString(file.Content)
        };
    }

    /// <summary>
    /// Creates a file input from a state object with automatic type validation and multiple file detection.
    /// Validates that the state type is either FileInput (single file) or IEnumerable&lt;FileInput&gt; (multiple files).
    /// </summary>
    /// <param name="state">The state object to bind to (must be FileInput or IEnumerable&lt;FileInput&gt; type).</param>
    /// <param name="placeholder">Optional placeholder text displayed when no files are selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the file input.</param>
    /// <returns>A file input bound to the state object with automatic multiple file detection.</returns>
    /// <exception cref="Exception">Thrown when the state type is not FileInput or IEnumerable&lt;FileInput&gt;.</exception>
    public static FileInputBase ToFileInput(this IAnyState state, string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
    {
        var type = state.GetStateType();

        //Check that type is FileInput, FileInput? or IEnumerable<FileInput>
        var isCollection = type.IsGenericType &&
                          type.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                          type.GetGenericArguments()[0] == typeof(FileInput);
        var isValid = type == typeof(FileInput) || isCollection;

        if (!isValid)
        {
            throw new Exception("Invalid type for FileInput");
        }

        Type genericType = typeof(FileInput<>).MakeGenericType(type);
        FileInputBase input = (FileInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant)!;

        // Set Multiple based on type
        input = input with { Multiple = isCollection };

        return input;
    }

    /// <summary>Sets the placeholder text for the file input.</summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="title">The placeholder text to display when no files are selected.</param>
    /// <returns>The file input with the specified placeholder text.</returns>
    public static FileInputBase Placeholder(this FileInputBase widget, string title)
    {
        return widget with { Placeholder = title };
    }

    /// <summary>Sets the disabled state of the file input.</summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="disabled">Whether the input should be disabled.</param>
    /// <returns>The file input with the specified disabled state.</returns>
    public static FileInputBase Disabled(this FileInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }

    /// <summary>Sets the visual variant of the file input.</summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="variant">The visual variant (currently only Drop is available).</param>
    /// <returns>The file input with the specified variant.</returns>
    public static FileInputBase Variant(this FileInputBase widget, FileInputs variant)
    {
        return widget with { Variant = variant };
    }

    /// <summary>Sets the validation error message for the file input.</summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The file input with the specified validation error.</returns>
    public static FileInputBase Invalid(this FileInputBase widget, string? invalid)
    {
        return widget with { Invalid = invalid };
    }

    /// <summary>Sets the accepted file types for the file input using MIME types or file extensions.</summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="accept">A comma-separated list of accepted file types (e.g., "image/*", ".pdf,.doc", "text/plain").</param>
    /// <returns>The file input with the specified file type restrictions.</returns>
    public static FileInputBase Accept(this FileInputBase widget, string accept)
    {
        return widget with { Accept = accept };
    }

    /// <summary>
    /// Sets the maximum number of files that can be selected (only applicable for multiple file inputs).
    /// </summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="maxFiles">The maximum number of files allowed.</param>
    /// <returns>The file input with the specified maximum file limit.</returns>
    /// <exception cref="InvalidOperationException">Thrown when trying to set MaxFiles on a single file input.</exception>
    public static FileInputBase MaxFiles(this FileInputBase widget, int maxFiles)
    {
        if (widget.Multiple != true)
        {
            throw new InvalidOperationException("MaxFiles can only be set on a multi-file input (IEnumerable<FileInput>). Use a collection state type for multiple files.");
        }
        return widget with { MaxFiles = maxFiles };
    }

    /// <summary>
    /// Validates a single file against the widget's accept pattern.
    /// </summary>
    /// <param name="widget">The file input widget containing validation rules.</param>
    /// <param name="file">The file to validate.</param>
    /// <returns>A ValidationResult indicating whether the file is valid and any error messages.</returns>
    public static ValidationResult ValidateFile(this FileInputBase widget, FileInput file)
    {
        return FileInputValidation.ValidateFileType(file, widget.Accept);
    }

    /// <summary>
    /// Validates multiple files against the widget's accept pattern and maximum file count limit.
    /// </summary>
    /// <param name="widget">The file input widget containing validation rules.</param>
    /// <param name="files">The files to validate.</param>
    /// <returns>A ValidationResult indicating whether all files are valid and any error messages.</returns>
    public static ValidationResult ValidateFiles(this FileInputBase widget, IEnumerable<FileInput> files)
    {
        var filesList = files.ToList();

        // Validate file count first
        var countValidation = FileInputValidation.ValidateFileCount(filesList, widget.MaxFiles);
        if (!countValidation.IsValid)
        {
            return countValidation;
        }

        // Then validate file types
        return FileInputValidation.ValidateFileTypes(filesList, widget.Accept);
    }


    /// <summary>
    /// Sets the blur event handler for the file input.
    /// This method allows you to configure the file input's blur behavior,
    /// enabling it to perform custom actions when the input loses focus.
    /// </summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new file input instance with the updated blur handler.</returns>
    [OverloadResolutionPriority(1)]
    public static FileInputBase HandleBlur(this FileInputBase widget, Func<Event<IAnyInput>, ValueTask> onBlur)
    {
        return widget with { OnBlur = onBlur };
    }

    /// <summary>
    /// Sets the blur event handler for the file input.
    /// Compatibility overload for Action-based event handlers.
    /// </summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new file input instance with the updated blur handler.</returns>
    public static FileInputBase HandleBlur(this FileInputBase widget, Action<Event<IAnyInput>> onBlur)
    {
        return widget.HandleBlur(onBlur.ToValueTask());
    }

    /// <summary>
    /// Sets a simple blur event handler for the file input.
    /// This method allows you to configure the file input's blur behavior with
    /// a simple action that doesn't require the input event context.
    /// </summary>
    /// <param name="widget">The file input to configure.</param>
    /// <param name="onBlur">The simple action to perform when the input loses focus.</param>
    /// <returns>A new file input instance with the updated blur handler.</returns>
    public static FileInputBase HandleBlur(this FileInputBase widget, Action onBlur)
    {
        return widget.HandleBlur(_ => { onBlur(); return ValueTask.CompletedTask; });
    }
}