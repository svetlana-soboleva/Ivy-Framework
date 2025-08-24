using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual and functional variants available for text input controls.
/// Each variant provides specialized input behavior, validation, and user interface
/// optimized for different types of text data entry scenarios.
/// </summary>
public enum TextInputs
{
    /// <summary>Standard single-line text input for general text entry.</summary>
    Text,
    /// <summary>Multi-line textarea input for longer text content and descriptions.</summary>
    Textarea,
    /// <summary>Email input with email-specific validation and keyboard optimization.</summary>
    Email,
    /// <summary>Telephone input optimized for phone number entry with appropriate keyboard.</summary>
    Tel,
    /// <summary>URL input with URL validation and web address formatting.</summary>
    Url,
    /// <summary>Password input with masked character display for secure text entry.</summary>
    Password,
    /// <summary>Search input optimized for search queries with search-specific styling and behavior.</summary>
    Search
}

/// <summary>
/// Interface for text input controls that extends IAnyInput with text-specific properties.
/// Provides functionality for text entry with placeholder text and variant configuration
/// for different text input scenarios including single-line, multi-line, and specialized formats.
/// </summary>
public interface IAnyTextInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when the input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual and functional variant of the text input.</summary>
    /// <value>The text input variant (Text, Textarea, Email, Tel, Url, Password, or Search).</value>
    public TextInputs Variant { get; set; }
}

/// <summary>
/// Abstract base class for text input controls that provides common text input functionality.
/// Supports various text input variants including single-line, multi-line, and specialized formats
/// like email, password, and search inputs with keyboard shortcuts and validation support.
/// </summary>
public abstract record TextInputBase : WidgetBase<TextInputBase>, IAnyTextInput
{
    /// <summary>Gets or sets whether the input is disabled.</summary>
    /// <value>true if the input is disabled; false if it's interactive.</value>
    [Prop] public bool Disabled { get; set; }

    /// <summary>Gets or sets the validation error message.</summary>
    /// <value>The error message, or null if the input is valid.</value>
    [Prop] public string? Invalid { get; set; }

    /// <summary>Gets or sets the placeholder text displayed when the input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    [Prop] public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual and functional variant of the text input.</summary>
    /// <value>The text input variant (Text, Textarea, Email, Tel, Url, Password, or Search).</value>
    [Prop] public TextInputs Variant { get; set; }

    /// <summary>Gets or sets the keyboard shortcut key for focusing this input.</summary>
    /// <value>The shortcut key combination, or null if no shortcut is assigned.</value>
    /// <remarks>Enables quick keyboard navigation to this input field for improved accessibility and user experience.</remarks>
    [Prop] public string? ShortcutKey { get; set; }

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IAnyInput>, ValueTask>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this text input can bind to and work with.
    /// Base implementation returns empty array; derived classes should override to specify supported types.
    /// </summary>
    /// <returns>An empty array in the base implementation.</returns>
    public Type[] SupportedStateTypes() => [];
}

/// <summary>
/// Generic text input control that provides type-safe text entry functionality for string-like types.
/// Supports various text input variants (single-line, multi-line, specialized formats) with comprehensive
/// state binding, validation, and keyboard shortcut support for flexible text data entry scenarios.
/// </summary>
/// <typeparam name="TString">The type of the text value (typically string or string-convertible types).</typeparam>
public record TextInput<TString> : TextInputBase, IInput<TString>
{
    /// <summary>
    /// Initializes a new text input bound to a state object for automatic value synchronization.
    /// The input will display the current state value and update the state when text changes.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates and change handling.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual and functional variant of the text input.</param>
    public TextInput(IAnyState state, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TString>();
        Value = typedState.Value;
        OnChange = e => { typedState.Set(e.Value); return ValueTask.CompletedTask; };
    }

    /// <summary>
    /// Initializes a new text input with an explicit value and change handler.
    /// Useful for manual state management or when custom change handling is required.
    /// </summary>
    /// <param name="value">The initial text value.</param>
    /// <param name="onChange">Optional event handler called when the text value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual and functional variant of the text input.</param>
    [OverloadResolutionPriority(1)]
    public TextInput(TString value, Func<Event<IInput<TString>, TString>, ValueTask>? onChange = null, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    // Overload for Action<Event<IInput<TString>, TString>>
    public TextInput(TString value, Action<Event<IInput<TString>, TString>>? onChange = null, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange?.ToValueTask();
        Value = value;
    }

    /// <summary>
    /// Initializes a new text input with basic configuration.
    /// Requires separate value and change handler assignment for functionality.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual and functional variant of the text input.</param>
    public TextInput(string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
    {
        Placeholder = placeholder;
        Variant = variant;
        Disabled = disabled;
    }

    /// <summary>Gets the current text value.</summary>
    /// <value>The text value of type TString.</value>
    [Prop] public TString Value { get; } = default!;

    /// <summary>Gets the event handler called when the text value changes.</summary>
    /// <value>The change event handler that receives the text input and the new value, or null if no handler is set.</value>
    [Event] public Func<Event<IInput<TString>, TString>, ValueTask>? OnChange { get; }
}

/// <summary>
/// Concrete text input control for string values that provides convenient string-specific text entry functionality.
/// Inherits from TextInput&lt;string&gt; to provide a non-generic interface for the most common text input scenario
/// while maintaining all the functionality of the generic base class.
/// </summary>
public record TextInput : TextInput<string>
{
    /// <summary>
    /// Initializes a new string text input bound to a state object for automatic value synchronization.
    /// The input will display the current state value and update the state when text changes.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates and change handling.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual and functional variant of the text input.</param>
    public TextInput(IAnyState state, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : base(state, placeholder, disabled, variant)
    {
    }

    /// <summary>
    /// Initializes a new string text input with an explicit value and change handler.
    /// Useful for manual state management or when custom change handling is required.
    /// </summary>
    /// <param name="value">The initial string value.</param>
    /// <param name="onChange">Optional event handler called when the text value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual and functional variant of the text input.</param>
    [OverloadResolutionPriority(1)]
    public TextInput(string value, Func<Event<IInput<string>, string>, ValueTask>? onChange = null, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : base(value, onChange, placeholder, disabled, variant)
    {
    }

    // Overload for Action<Event<IInput<string>, string>>
    public TextInput(string value, Action<Event<IInput<string>, string>>? onChange = null, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : base(value, onChange?.ToValueTask(), placeholder, disabled, variant)
    {
    }

    /// <summary>
    /// Initializes a new string text input with basic configuration.
    /// Requires separate value and change handler assignment for functionality.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual and functional variant of the text input.</param>
    public TextInput(string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : base(placeholder, disabled, variant)
    {
    }
}

/// <summary>
/// Provides extension methods for creating and configuring text input controls with fluent syntax.
/// Includes automatic type detection, specialized input variants (textarea, email, password, etc.),
/// and comprehensive configuration methods for text-based input controls.
/// </summary>
public static class TextInputExtensions
{
    /// <summary>
    /// Creates a text input from a state object with automatic type detection.
    /// Uses reflection to create the appropriate generic TextInput&lt;T&gt; instance based on the state's type,
    /// enabling type-safe text input for any string-like state value.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual and functional variant of the text input.</param>
    /// <returns>A text input bound to the state object with automatic type matching.</returns>
    public static TextInputBase ToTextInput(this IAnyState state, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
    {
        var type = state.GetStateType();
        Type genericType = typeof(TextInput<>).MakeGenericType(type);
        TextInputBase input = (TextInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant)!;
        return input;
    }

    /// <summary>
    /// Creates a multi-line textarea input from a state object.
    /// Convenience method that creates a text input with the Textarea variant for longer text content.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>A textarea input bound to the state object.</returns>
    public static TextInputBase ToTextAreaInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Textarea);

    /// <summary>
    /// Creates a search input from a state object.
    /// Convenience method that creates a text input with the Search variant optimized for search queries.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>A search input bound to the state object.</returns>
    public static TextInputBase ToSearchInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Search);

    /// <summary>
    /// Creates a password input from a state object.
    /// Convenience method that creates a text input with the Password variant for secure text entry with masked characters.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>A password input bound to the state object.</returns>
    public static TextInputBase ToPasswordInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Password);

    /// <summary>
    /// Creates an email input from a state object.
    /// Convenience method that creates a text input with the Email variant optimized for email address entry with validation.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>An email input bound to the state object.</returns>
    public static TextInputBase ToEmailInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Email);

    /// <summary>
    /// Creates a URL input from a state object.
    /// Convenience method that creates a text input with the Url variant optimized for web address entry with validation.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>A URL input bound to the state object.</returns>
    public static TextInputBase ToUrlInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Url);

    /// <summary>
    /// Creates a telephone input from a state object.
    /// Convenience method that creates a text input with the Tel variant optimized for phone number entry.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>A telephone input bound to the state object.</returns>
    public static TextInputBase ToTelInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Tel);

    /// <summary>Sets the placeholder text for the text input.</summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="placeholder">The placeholder text to display when the input is empty.</param>
    /// <returns>The text input with the specified placeholder text.</returns>
    public static TextInputBase Placeholder(this TextInputBase widget, string placeholder) => widget with { Placeholder = placeholder };

    /// <summary>Sets the disabled state of the text input.</summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="disabled">Whether the input should be disabled.</param>
    /// <returns>The text input with the specified disabled state.</returns>
    public static TextInputBase Disabled(this TextInputBase widget, bool disabled = true) => widget with { Disabled = disabled };

    /// <summary>Sets the visual and functional variant of the text input.</summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="variant">The text input variant (Text, Textarea, Email, Tel, Url, Password, or Search).</param>
    /// <returns>The text input with the specified variant.</returns>
    public static TextInputBase Variant(this TextInputBase widget, TextInputs variant) => widget with { Variant = variant };

    /// <summary>Sets the validation error message for the text input.</summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The text input with the specified validation error.</returns>
    public static TextInputBase Invalid(this TextInputBase widget, string invalid) => widget with { Invalid = invalid };

    /// <summary>Sets the keyboard shortcut key for focusing the text input.</summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="shortcutKey">The keyboard shortcut key combination for focusing this input.</param>
    /// <returns>The text input with the specified shortcut key.</returns>
    public static TextInputBase ShortcutKey(this TextInputBase widget, string shortcutKey) => widget with { ShortcutKey = shortcutKey };


    /// <summary>
    /// Sets the blur event handler for the text input.
    /// This method allows you to configure the text input's blur behavior,
    /// enabling it to perform custom actions when the input loses focus.
    /// </summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new text input instance with the updated blur handler.</returns>
    [OverloadResolutionPriority(1)]
    public static TextInputBase HandleBlur(this TextInputBase widget, Func<Event<IAnyInput>, ValueTask> onBlur)
    {
        return widget with { OnBlur = onBlur };
    }

    /// <summary>
    /// Sets the blur event handler for the text input.
    /// Compatibility overload for Action-based event handlers.
    /// </summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new text input instance with the updated blur handler.</returns>
    public static TextInputBase HandleBlur(this TextInputBase widget, Action<Event<IAnyInput>> onBlur)
    {
        return widget.HandleBlur(onBlur.ToValueTask());
    }

    /// <summary>
    /// Sets a simple blur event handler for the text input.
    /// This method allows you to configure the text input's blur behavior with
    /// a simple action that doesn't require the input event context.
    /// </summary>
    /// <param name="widget">The text input to configure.</param>
    /// <param name="onBlur">The simple action to perform when the input loses focus.</param>
    /// <returns>A new text input instance with the updated blur handler.</returns>
    public static TextInputBase HandleBlur(this TextInputBase widget, Action onBlur)
    {
        return widget.HandleBlur(_ => { onBlur(); return ValueTask.CompletedTask; });
    }
}