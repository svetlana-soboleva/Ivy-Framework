using System.Text;
using System.Text.Json.Serialization;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Shared;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a file uploaded through the FileInput widget.
/// Contains metadata about the file and optionally its binary content.
/// </summary>
public record FileInput
{
    /// <summary>
    /// The original filename of the uploaded file (e.g., "document.pdf")
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// The MIME type of the file (e.g., "application/pdf", "image/jpeg")
    /// </summary>
    public required string Type { get; init; }
    
    /// <summary>
    /// The size of the file in bytes
    /// </summary>
    public int Size { get; init; }
    
    /// <summary>
    /// The last modified timestamp of the file from the user's system
    /// </summary>
    public DateTime LastModified { get; init; }
    
    /// <summary>
    /// The binary content of the file, encoded as base64 string.
    /// This is excluded from JSON serialization when null to reduce payload size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public byte[]? Content { get; init; }
}

/// <summary>
/// Defines the visual variant of the FileInput widget.
/// Currently only supports drag-and-drop interface.
/// </summary>
public enum FileInputs
{
    /// <summary>
    /// Drag-and-drop interface with visual drop zone
    /// </summary>
    Drop
}

/// <summary>
/// Common interface for all FileInput widget variants.
/// Defines the shared properties that all file input widgets must implement.
/// </summary>
public interface IAnyFileInput : IAnyInput
{
    /// <summary>
    /// Text displayed in the drop zone when no file is selected
    /// </summary>
    public string? Placeholder { get; set; }
    
    /// <summary>
    /// The visual variant of the file input (currently only Drop is supported)
    /// </summary>
    public FileInputs Variant { get; set; }
}

/// <summary>
/// Base class for FileInput widgets that provides common properties and behavior.
/// This abstract class defines all the configuration options available for file inputs.
/// </summary>
public abstract record FileInputBase : WidgetBase<FileInputBase>, IAnyFileInput
{
    /// <summary>
    /// When true, the file input is disabled and cannot accept files
    /// </summary>
    [Prop] public bool Disabled { get; set; }
    
    /// <summary>
    /// Error message to display when validation fails (e.g., "File too large")
    /// </summary>
    [Prop] public string? Invalid { get; set; }
    
    /// <summary>
    /// Text displayed in the drop zone when no file is selected
    /// </summary>
    [Prop] public string? Placeholder { get; set; }
    
    /// <summary>
    /// The visual variant of the file input (currently only Drop is supported)
    /// </summary>
    [Prop] public FileInputs Variant { get; set; }
    
    /// <summary>
    /// Comma-separated list of accepted file extensions or MIME types (e.g., ".pdf,.doc" or "image/*")
    /// </summary>
    [Prop] public string? Accept { get; set; }
    
    /// <summary>
    /// When true, allows selecting multiple files. When false, only single file selection is allowed.
    /// </summary>
    [Prop] public bool Multiple { get; set; }
    
    /// <summary>
    /// Minimum file size in bytes. Files smaller than this will be rejected.
    /// </summary>
    [Prop] public long? MinSize { get; set; }
    
    /// <summary>
    /// Maximum file size in bytes. Files larger than this will be rejected.
    /// </summary>
    [Prop] public long? MaxSize { get; set; }
    
    /// <summary>
    /// Minimum number of files required when Multiple is true. Validation fails if fewer files are selected.
    /// </summary>
    [Prop] public int? MinFiles { get; set; }
    
    /// <summary>
    /// Maximum number of files allowed when Multiple is true. Validation fails if more files are selected.
    /// </summary>
    [Prop] public int? MaxFiles { get; set; }
    
    /// <summary>
    /// Event handler called when the file input loses focus
    /// </summary>
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }
    
    /// <summary>
    /// Returns the supported state types for this widget (FileInput, FileInput?, or IEnumerable&lt;FileInput&gt;)
    /// </summary>
    public Type[] SupportedStateTypes() => [];
}

/// <summary>
/// Generic FileInput widget that can handle single files, nullable files, or collections of files.
/// TValue can be FileInput, FileInput?, or IEnumerable&lt;FileInput&gt;.
/// </summary>
/// <typeparam name="TValue">The type of the file value (FileInput, FileInput?, or IEnumerable&lt;FileInput&gt;)</typeparam>
public record FileInput<TValue> : FileInputBase, IInput<TValue>, IAnyFileInput
{
    /// <summary>
    /// Creates a FileInput bound to a state variable
    /// </summary>
    /// <param name="state">The state variable to bind to</param>
    /// <param name="placeholder">Text displayed when no file is selected</param>
    /// <param name="disabled">Whether the input is disabled</param>
    /// <param name="variant">The visual variant (currently only Drop)</param>
    public FileInput(IAnyState state, string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TValue>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
    }

    /// <summary>
    /// Creates a FileInput with a direct value and change handler
    /// </summary>
    /// <param name="value">The current file value</param>
    /// <param name="onChange">Handler called when files are selected or removed</param>
    /// <param name="placeholder">Text displayed when no file is selected</param>
    /// <param name="disabled">Whether the input is disabled</param>
    /// <param name="variant">The visual variant (currently only Drop)</param>
    public FileInput(TValue value, Action<Event<IInput<TValue>, TValue>>? onChange, string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Creates a FileInput with default configuration
    /// </summary>
    /// <param name="placeholder">Text displayed when no file is selected</param>
    /// <param name="disabled">Whether the input is disabled</param>
    /// <param name="variant">The visual variant (currently only Drop)</param>
    public FileInput(string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
    {
        Placeholder = placeholder;
        Variant = variant;
        Disabled = disabled;
        Width = Size.Full();
        Height = Size.Units(50);
    }

    /// <summary>
    /// The current file value(s) stored in this input
    /// </summary>
    [Prop] public TValue Value { get; } = default!;

    /// <summary>
    /// Event handler called when files are selected, removed, or validation fails
    /// </summary>
    [Event] public Action<Event<IInput<TValue>, TValue>>? OnChange { get; }
}

/// <summary>
/// Extension methods for FileInput widgets that provide convenient configuration and utility methods.
/// </summary>
public static class FileInputExtensions
{
    /// <summary>
    /// Converts the binary content of a file to a UTF-8 string.
    /// Useful for text files like .txt, .md, .json, etc.
    /// Returns null if the file has no content or is empty.
    /// </summary>
    /// <param name="file">The file input to extract text from</param>
    /// <returns>The file content as a string, or null if not available</returns>
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
    /// Converts a state variable to a FileInput widget.
    /// Automatically detects if the state is for single files, nullable files, or collections.
    /// For collection types, automatically enables multiple file selection.
    /// </summary>
    /// <param name="state">The state variable to bind to the file input</param>
    /// <param name="placeholder">Text displayed when no file is selected</param>
    /// <param name="disabled">Whether the input is disabled</param>
    /// <param name="variant">The visual variant (currently only Drop)</param>
    /// <returns>A configured FileInput widget bound to the state</returns>
    /// <exception cref="Exception">Thrown when the state type is not compatible with FileInput</exception>
    public static FileInputBase ToFileInput(this IAnyState state, string? placeholder = null, bool disabled = false, FileInputs variant = FileInputs.Drop)
    {
        var stateType = state.GetStateType();

        // Check that type is FileInput, FileInput? or IEnumerable<FileInput>
        var isValidType = stateType == typeof(FileInput)
                      || (stateType.IsGenericType &&
                          stateType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                          stateType.GetGenericArguments()[0] == typeof(FileInput));

        if (!isValidType)
        {
            throw new Exception($"Invalid state type for FileInput: {stateType.Name}. Expected FileInput, FileInput?, or IEnumerable<FileInput>");
        }

        Type genericWidgetType = typeof(FileInput<>).MakeGenericType(stateType);
        FileInputBase fileInputWidget = (FileInputBase)Activator.CreateInstance(genericWidgetType, state, placeholder, disabled, variant)!;

        // Automatically enable multiple selection for collection types
        if (stateType.IsGenericType && stateType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            fileInputWidget = fileInputWidget.Multiple();
        }

        return fileInputWidget;
    }

    /// <summary>
    /// Sets the placeholder text displayed when no file is selected
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="title">The placeholder text to display</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase Placeholder(this FileInputBase widget, string title)
    {
        return widget with { Placeholder = title };
    }

    /// <summary>
    /// Enables or disables the file input widget
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="disabled">Whether to disable the widget (default: true)</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase Disabled(this FileInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }

    /// <summary>
    /// Sets the visual variant of the file input widget
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="variant">The visual variant to use</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase Variant(this FileInputBase widget, FileInputs variant)
    {
        return widget with { Variant = variant };
    }

    /// <summary>
    /// Sets an error message to display when validation fails
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="invalid">The error message to display</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase Invalid(this FileInputBase widget, string? invalid)
    {
        return widget with { Invalid = invalid };
    }

    /// <summary>
    /// Sets the accepted file types using a comma-separated list of extensions or MIME types
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="accept">Comma-separated list (e.g., ".pdf,.doc" or "image/*")</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase Accept(this FileInputBase widget, string accept)
    {
        return widget with { Accept = accept };
    }

    /// <summary>
    /// Enables or disables multiple file selection
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="multiple">Whether to allow multiple file selection (default: true)</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase Multiple(this FileInputBase widget, bool multiple = true)
    {
        return widget with { Multiple = multiple };
    }

    /// <summary>
    /// Sets the minimum file size in bytes. Files smaller than this will be rejected.
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="minSize">Minimum file size in bytes</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase MinSize(this FileInputBase widget, long minSize)
    {
        return widget with { MinSize = minSize };
    }

    /// <summary>
    /// Sets the maximum file size in bytes. Files larger than this will be rejected.
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="maxSize">Maximum file size in bytes</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase MaxSize(this FileInputBase widget, long maxSize)
    {
        return widget with { MaxSize = maxSize };
    }

    /// <summary>
    /// Sets the minimum number of files required when multiple selection is enabled.
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="minFiles">Minimum number of files required</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase MinFiles(this FileInputBase widget, int minFiles)
    {
        return widget with { MinFiles = minFiles };
    }

    /// <summary>
    /// Sets the maximum number of files allowed when multiple selection is enabled.
    /// </summary>
    /// <param name="widget">The file input widget to configure</param>
    /// <param name="maxFiles">Maximum number of files allowed</param>
    /// <returns>The configured file input widget</returns>
    public static FileInputBase MaxFiles(this FileInputBase widget, int maxFiles)
    {
        return widget with { MaxFiles = maxFiles };
    }
}