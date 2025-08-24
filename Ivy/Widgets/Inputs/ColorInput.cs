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
/// Defines the visual variants available for color input controls.
/// Each variant provides a different user interface pattern for color selection and input.
/// </summary>
public enum ColorInputs
{
    /// <summary>Text-only input for entering color values as hex codes, RGB values, or color names.</summary>
    Text,
    /// <summary>Visual color picker interface for selecting colors through a graphical color palette.</summary>
    Picker,
    /// <summary>Combined interface with both text input and visual color picker for maximum flexibility.</summary>
    TextAndPicker
}

/// <summary>
/// Interface for color input controls that extends IAnyInput with color-specific properties.
/// Provides functionality for color selection including placeholder text and visual variants
/// for different color input methods (text, picker, or combined).
/// </summary>
public interface IAnyColorInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when the color input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual variant of the color input.</summary>
    /// <value>The input variant (Text, Picker, or TextAndPicker).</value>
    public ColorInputs Variant { get; set; }
}

/// <summary>
/// Abstract base class for color input controls that provides color selection functionality.
/// Supports multiple color formats including hex codes, RGB values, color names, and the Colors enum.
/// Provides both text-based and visual color picker interfaces depending on the variant.
/// </summary>
public abstract record ColorInputBase : WidgetBase<ColorInputBase>, IAnyColorInput
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

    /// <summary>Gets or sets whether the input accepts null values.</summary>
    /// <value>true if null values are allowed; false if a color value is required.</value>
    [Prop] public bool Nullable { get; set; }

    /// <summary>Gets or sets the visual variant of the color input.</summary>
    /// <value>The input variant (Text, Picker, or TextAndPicker). Defaults to TextAndPicker.</value>
    [Prop] public ColorInputs Variant { get; set; } = ColorInputs.TextAndPicker;

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IAnyInput>, ValueTask>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this color input can bind to and work with.
    /// Supports string types for hex codes and color names, and Colors enum for predefined colors.
    /// </summary>
    /// <returns>An array of supported types including string and Colors enum (with nullable variants).</returns>
    public Type[] SupportedStateTypes() => [
        typeof(string),
        typeof(Colors), typeof(Colors?)
        ];
}

/// <summary>
/// Generic color input control that provides color selection functionality for various color types.
/// Supports string-based colors (hex codes, RGB, color names) and Colors enum values.
/// Offers multiple input methods including text entry and visual color picker interfaces.
/// </summary>
/// <typeparam name="TColor">The type of the color value (string, Colors, or Colors?).</typeparam>
public record ColorInput<TColor> : ColorInputBase, IInput<TColor>
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    [OverloadResolutionPriority(1)]
    public ColorInput(IAnyState state, string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TColor>();
        Value = typedState.Value;
        OnChange = e => { typedState.Set(e.Value); return ValueTask.CompletedTask; };
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and async change handler.
    /// </summary>
    /// <param name="value">The initial color value.</param>
    /// <param name="onChange">Async event handler called when the color value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    [OverloadResolutionPriority(1)]
    public ColorInput(TColor value, Func<Event<IInput<TColor>, TColor>, ValueTask> onChange, string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and synchronous change handler.
    /// Compatibility overload for Action-based change handlers.
    /// </summary>
    /// <param name="value">The initial color value.</param>
    /// <param name="onChange">Event handler called when the color value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    public ColorInput(TColor value, Action<Event<IInput<TColor>, TColor>> onChange, string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
        : this(placeholder, disabled, variant)
    {
        OnChange = e => { onChange(e); return ValueTask.CompletedTask; };
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    public ColorInput(string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
    {
        Disabled = disabled;
        Placeholder = placeholder;
        Variant = variant;
    }

    /// <summary>Gets the current color value.</summary>
    /// <value>The color value of the specified type.</value>
    [Prop] public TColor Value { get; } = default!;

    /// <summary>Gets the event handler called when the color value changes.</summary>
    /// <value>The async change event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IInput<TColor>, TColor>, ValueTask>? OnChange { get; }
}

/// <summary>
/// Concrete color input control for string-based color values.
/// Provides a convenient, non-generic interface for working with color strings (hex codes, RGB, color names)
/// while inheriting all functionality from the generic ColorInput&lt;string&gt; base class.
/// </summary>
public record ColorInput : ColorInput<string>
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    [OverloadResolutionPriority(1)]
    public ColorInput(IAnyState state, string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
        : base(state, placeholder, disabled, variant)
    {
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and async change handler.
    /// </summary>
    /// <param name="value">The initial color value as a string (hex code, RGB, or color name).</param>
    /// <param name="onChange">Async event handler called when the color value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    [OverloadResolutionPriority(1)]
    public ColorInput(string value, Func<Event<IInput<string>, string>, ValueTask> onChange, string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
        : base(value, onChange, placeholder, disabled, variant)
    {
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and synchronous change handler.
    /// Compatibility overload for Action-based change handlers.
    /// </summary>
    /// <param name="value">The initial color value as a string (hex code, RGB, or color name).</param>
    /// <param name="onChange">Event handler called when the color value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    public ColorInput(string value, Action<Event<IInput<string>, string>> onChange, string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
        : base(value, onChange, placeholder, disabled, variant)
    {
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    public ColorInput(string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
        : base(placeholder, disabled, variant)
    {
    }
}

/// <summary>
/// Provides extension methods for creating and configuring color inputs with fluent syntax.
/// Includes methods for state binding with automatic type detection and nullable handling,
/// as well as convenient configuration of color input features and appearance.
/// </summary>
public static class ColorInputExtensions
{
    /// <summary>
    /// Creates a color input from a state object with automatic type binding and nullable detection.
    /// Supports both string-based colors and Colors enum values with proper nullable handling.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the color input.</param>
    /// <returns>A color input bound to the state object with automatic type and nullable detection.</returns>
    public static ColorInputBase ToColorInput(this IAnyState state, string? placeholder = null, bool disabled = false, ColorInputs variant = ColorInputs.TextAndPicker)
    {
        var type = state.GetStateType();
        Type genericType = typeof(ColorInput<>).MakeGenericType(type);
        ColorInputBase input = (ColorInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant)!;
        input.Nullable = type.IsNullableType();
        return input;
    }

    /// <summary>Sets the disabled state of the color input.</summary>
    /// <param name="widget">The color input to configure.</param>
    /// <param name="disabled">Whether the input should be disabled.</param>
    /// <returns>The color input with the specified disabled state.</returns>
    public static ColorInputBase Disabled(this ColorInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }

    /// <summary>Sets the placeholder text for the color input.</summary>
    /// <param name="widget">The color input to configure.</param>
    /// <param name="placeholder">The placeholder text to display when the input is empty.</param>
    /// <returns>The color input with the specified placeholder text.</returns>
    public static ColorInputBase Placeholder(this ColorInputBase widget, string? placeholder)
    {
        return widget with { Placeholder = placeholder };
    }

    /// <summary>Sets the validation error message for the color input.</summary>
    /// <param name="widget">The color input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The color input with the specified validation error.</returns>
    public static ColorInputBase Invalid(this ColorInputBase widget, string? invalid)
    {
        return widget with { Invalid = invalid };
    }

    /// <summary>Sets the visual variant of the color input.</summary>
    /// <param name="widget">The color input to configure.</param>
    /// <param name="variant">The visual variant (Text, Picker, or TextAndPicker).</param>
    /// <returns>The color input with the specified variant.</returns>
    public static ColorInputBase Variant(this ColorInputBase widget, ColorInputs variant)
    {
        return widget with { Variant = variant };
    }


    /// <summary>
    /// Sets the blur event handler for the color input.
    /// This method allows you to configure the color input's blur behavior,
    /// enabling it to perform custom actions when the input loses focus.
    /// </summary>
    /// <param name="widget">The color input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new color input instance with the updated blur handler.</returns>
    [OverloadResolutionPriority(1)]
    public static ColorInputBase HandleBlur(this ColorInputBase widget, Func<Event<IAnyInput>, ValueTask> onBlur)
    {
        return widget with { OnBlur = onBlur };
    }

    /// <summary>
    /// Sets the blur event handler for the color input.
    /// Compatibility overload for Action-based event handlers.
    /// </summary>
    /// <param name="widget">The color input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new color input instance with the updated blur handler.</returns>
    public static ColorInputBase HandleBlur(this ColorInputBase widget, Action<Event<IAnyInput>> onBlur)
    {
        return widget.HandleBlur(onBlur.ToValueTask());
    }

    /// <summary>
    /// Sets a simple blur event handler for the color input.
    /// This method allows you to configure the color input's blur behavior with
    /// a simple action that doesn't require the input event context.
    /// </summary>
    /// <param name="widget">The color input to configure.</param>
    /// <param name="onBlur">The simple action to perform when the input loses focus.</param>
    /// <returns>A new color input instance with the updated blur handler.</returns>
    public static ColorInputBase HandleBlur(this ColorInputBase widget, Action onBlur)
    {
        return widget.HandleBlur(_ => { onBlur(); return ValueTask.CompletedTask; });
    }
}