using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Shared;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual variants available for code input controls.
/// Currently provides a single default variant with potential for future expansion.
/// </summary>
public enum CodeInputs
{
    /// <summary>Default code input variant with syntax highlighting and standard code editing features.</summary>
    Default
}

/// <summary>
/// Interface for code input controls that extends IAnyInput with code-specific properties.
/// Provides functionality for code editing including placeholder text and visual variants.
/// </summary>
public interface IAnyCodeInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when the code input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual variant of the code input.</summary>
    /// <value>The input variant (currently only Default is available).</value>
    public CodeInputs Variant { get; set; }
}

/// <summary>
/// Abstract base class for code input controls that provides syntax highlighting and code editing functionality.
/// Supports various programming languages with syntax highlighting, copy functionality, and standard input features.
/// </summary>
public abstract record CodeInputBase : WidgetBase<CodeInputBase>, IAnyCodeInput
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

    /// <summary>Gets or sets the visual variant of the code input.</summary>
    /// <value>The input variant (currently only Default is available).</value>
    [Prop] public CodeInputs Variant { get; set; }

    /// <summary>Gets or sets the programming language for syntax highlighting.</summary>
    /// <value>The language for syntax highlighting, or null for plain text display.</value>
    [Prop] public Languages? Language { get; set; } = null;

    /// <summary>Gets or sets whether to display a copy button for the code content.</summary>
    /// <value>true to show a copy button; false to hide it. Defaults to false.</value>
    [Prop] public bool ShowCopyButton { get; set; } = false;

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IAnyInput>, ValueTask>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this code input can bind to.
    /// Code inputs only support string types for code content.
    /// </summary>
    /// <returns>An array containing only the string type.</returns>
    public Type[] SupportedStateTypes() => [typeof(string)];
}

/// <summary>
/// Generic code input control that provides syntax-highlighted code editing functionality.
/// Supports various programming languages with syntax highlighting, multi-line editing,
/// and standard code editor features like copy functionality.
/// </summary>
/// <typeparam name="TString">The type of the code content (typically string).</typeparam>
public record CodeInput<TString> : CodeInputBase, IInput<TString>
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the code input.</param>
    [OverloadResolutionPriority(1)]
    public CodeInput(IAnyState state, string? placeholder = null, bool disabled = false, CodeInputs variant = CodeInputs.Default)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TString>();
        Value = typedState.Value;
        OnChange = e => { typedState.Set(e.Value); return ValueTask.CompletedTask; };
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and async change handler.
    /// </summary>
    /// <param name="value">The initial code content.</param>
    /// <param name="onChange">Optional async event handler called when the code content changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the code input.</param>
    [OverloadResolutionPriority(1)]
    public CodeInput(TString value, Func<Event<IInput<TString>, TString>, ValueTask>? onChange = null, string? placeholder = null, bool disabled = false, CodeInputs variant = CodeInputs.Default)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and synchronous change handler.
    /// Compatibility overload for Action-based change handlers.
    /// </summary>
    /// <param name="value">The initial code content.</param>
    /// <param name="onChange">Optional event handler called when the code content changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the code input.</param>
    public CodeInput(TString value, Action<Event<IInput<TString>, TString>>? onChange = null, string? placeholder = null, bool disabled = false, CodeInputs variant = CodeInputs.Default)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange == null ? null : e => { onChange(e); return ValueTask.CompletedTask; };
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// Sets up the code input with default dimensions suitable for code editing.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the code input.</param>
    public CodeInput(string? placeholder = null, bool disabled = false, CodeInputs variant = CodeInputs.Default)
    {
        Placeholder = placeholder;
        Variant = variant;
        Disabled = disabled;
        Width = Size.Full();
        Height = Size.Units(25);
    }

    /// <summary>Gets the current code content.</summary>
    /// <value>The code content of the specified type.</value>
    [Prop] public TString Value { get; } = default!;

    /// <summary>Gets the event handler called when the code content changes.</summary>
    /// <value>The async change event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IInput<TString>, TString>, ValueTask>? OnChange { get; }
}

/// <summary>
/// Provides extension methods for creating and configuring code inputs with fluent syntax.
/// Includes methods for state binding and convenient configuration of code editor features
/// such as syntax highlighting, copy functionality, and visual appearance.
/// </summary>
public static class CodeInputExtensions
{
    /// <summary>
    /// Creates a code input from a state object with automatic type binding.
    /// Sets up syntax highlighting for the specified programming language.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the code input.</param>
    /// <param name="language">The programming language for syntax highlighting.</param>
    /// <returns>A code input bound to the state object with the specified language highlighting.</returns>
    public static CodeInputBase ToCodeInput(this IAnyState state, string? placeholder = null, bool disabled = false, CodeInputs variant = CodeInputs.Default, Languages language = Languages.Json)
    {
        var type = state.GetStateType();
        Type genericType = typeof(CodeInput<>).MakeGenericType(type);
        CodeInputBase input = (CodeInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant)!;
        return input;
    }

    /// <summary>Sets the placeholder text for the code input.</summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="placeholder">The placeholder text to display when the input is empty.</param>
    /// <returns>The code input with the specified placeholder text.</returns>
    public static CodeInputBase Placeholder(this CodeInputBase widget, string placeholder)
    {
        return widget with { Placeholder = placeholder };
    }

    /// <summary>Sets the disabled state of the code input.</summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="disabled">Whether the input should be disabled.</param>
    /// <returns>The code input with the specified disabled state.</returns>
    public static CodeInputBase Disabled(this CodeInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }

    /// <summary>Sets the visual variant of the code input.</summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="variant">The visual variant (currently only Default is available).</param>
    /// <returns>The code input with the specified variant.</returns>
    public static CodeInputBase Variant(this CodeInputBase widget, CodeInputs variant)
    {
        return widget with { Variant = variant };
    }

    /// <summary>Sets the validation error message for the code input.</summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The code input with the specified validation error.</returns>
    public static CodeInputBase Invalid(this CodeInputBase widget, string invalid)
    {
        return widget with { Invalid = invalid };
    }

    /// <summary>Sets the programming language for syntax highlighting.</summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="language">The programming language for syntax highlighting.</param>
    /// <returns>The code input with the specified language highlighting.</returns>
    public static CodeInputBase Language(this CodeInputBase widget, Languages language)
    {
        return widget with { Language = language };
    }

    /// <summary>Sets whether to display a copy button for the code content.</summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="showCopyButton">Whether to show the copy button.</param>
    /// <returns>The code input with the specified copy button visibility.</returns>
    public static CodeInputBase ShowCopyButton(this CodeInputBase widget, bool showCopyButton = true)
    {
        return widget with { ShowCopyButton = showCopyButton };
    }


    /// <summary>
    /// Sets the blur event handler for the code input.
    /// This method allows you to configure the code input's blur behavior,
    /// enabling it to perform custom actions when the input loses focus.
    /// </summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new code input instance with the updated blur handler.</returns>
    [OverloadResolutionPriority(1)]
    public static CodeInputBase HandleBlur(this CodeInputBase widget, Func<Event<IAnyInput>, ValueTask> onBlur)
    {
        return widget with { OnBlur = onBlur };
    }

    /// <summary>
    /// Sets the blur event handler for the code input.
    /// Compatibility overload for Action-based event handlers.
    /// </summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new code input instance with the updated blur handler.</returns>
    public static CodeInputBase HandleBlur(this CodeInputBase widget, Action<Event<IAnyInput>> onBlur)
    {
        return widget.HandleBlur(onBlur.ToValueTask());
    }

    /// <summary>
    /// Sets a simple blur event handler for the code input.
    /// This method allows you to configure the code input's blur behavior with
    /// a simple action that doesn't require the input event context.
    /// </summary>
    /// <param name="widget">The code input to configure.</param>
    /// <param name="onBlur">The simple action to perform when the input loses focus.</param>
    /// <returns>A new code input instance with the updated blur handler.</returns>
    public static CodeInputBase HandleBlur(this CodeInputBase widget, Action onBlur)
    {
        return widget.HandleBlur(_ => { onBlur(); return ValueTask.CompletedTask; });
    }
}