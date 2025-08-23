using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual variants available for number input controls.
/// Each variant provides a different user interface for numeric value entry and manipulation.
/// </summary>
public enum NumberInputs
{
    /// <summary>Standard number input field for direct numeric entry with validation and formatting.</summary>
    Number,
    /// <summary>Slider control for visual numeric selection within a defined range.</summary>
    Slider
}

/// <summary>
/// Defines the formatting styles available for displaying numeric values.
/// Each style applies different formatting rules and visual presentation for numbers.
/// </summary>
public enum NumberFormatStyle
{
    /// <summary>Standard decimal number formatting (e.g., 123.45).</summary>
    Decimal,
    /// <summary>Currency formatting with currency symbols and appropriate decimal places (e.g., $123.45).</summary>
    Currency,
    /// <summary>Percentage formatting with percent symbol and scaling (e.g., 12.34%).</summary>
    Percent
}

/// <summary>
/// Interface for number input controls that extends IAnyInput with numeric-specific properties.
/// Provides functionality for numeric value entry including range validation, formatting,
/// precision control, and various display styles for different numeric scenarios.
/// </summary>
public interface IAnyNumberInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when the input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the minimum allowed value for the numeric input.</summary>
    /// <value>The minimum value, or null if no minimum constraint should be applied.</value>
    public double? Min { get; set; }

    /// <summary>Gets or sets the maximum allowed value for the numeric input.</summary>
    /// <value>The maximum value, or null if no maximum constraint should be applied.</value>
    public double? Max { get; set; }

    /// <summary>Gets or sets the step increment for the numeric input (used for sliders and increment/decrement buttons).</summary>
    /// <value>The step value, or null to use default stepping behavior.</value>
    public double? Step { get; set; }

    /// <summary>Gets or sets the number of decimal places to display and accept.</summary>
    /// <value>The precision (decimal places), or null to use type-appropriate defaults.</value>
    public int? Precision { get; set; }

    /// <summary>Gets or sets the visual variant of the number input.</summary>
    /// <value>The input variant (Number or Slider).</value>
    public NumberInputs Variant { get; set; }

    /// <summary>Gets or sets the formatting style for displaying numeric values.</summary>
    /// <value>The format style (Decimal, Currency, or Percent).</value>
    public NumberFormatStyle FormatStyle { get; set; }

    /// <summary>Gets or sets the currency code for currency formatting (e.g., "USD", "EUR").</summary>
    /// <value>The currency code, or null to use default currency formatting.</value>
    public string? Currency { get; set; }

    /// <summary>Gets or sets the target type name for frontend validation and formatting.</summary>
    /// <value>The target type name (e.g., "int", "double", "decimal"), or null for automatic detection.</value>
    public string? TargetType { get; set; }
}

/// <summary>
/// Abstract base class for number input controls that provides comprehensive numeric input functionality.
/// Supports all .NET numeric types with range validation, precision control, formatting options,
/// and multiple input variants including standard inputs and sliders.
/// </summary>
public abstract record NumberInputBase : WidgetBase<NumberInputBase>, IAnyNumberInput
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

    /// <summary>Gets or sets the minimum allowed value for the numeric input.</summary>
    /// <value>The minimum value, or null if no minimum constraint should be applied.</value>
    [Prop] public double? Min { get; set; }

    /// <summary>Gets or sets the maximum allowed value for the numeric input.</summary>
    /// <value>The maximum value, or null if no maximum constraint should be applied.</value>
    [Prop] public double? Max { get; set; }

    /// <summary>Gets or sets the step increment for the numeric input.</summary>
    /// <value>The step value, or null to use default stepping behavior.</value>
    [Prop] public double? Step { get; set; }

    /// <summary>Gets or sets the number of decimal places to display and accept.</summary>
    /// <value>The precision (decimal places), or null to use type-appropriate defaults.</value>
    [Prop] public int? Precision { get; set; }

    /// <summary>Gets or sets the visual variant of the number input.</summary>
    /// <value>The input variant (Number or Slider).</value>
    [Prop] public NumberInputs Variant { get; set; }

    /// <summary>Gets or sets the formatting style for displaying numeric values.</summary>
    /// <value>The format style (Decimal, Currency, or Percent).</value>
    [Prop] public NumberFormatStyle FormatStyle { get; set; }

    /// <summary>Gets or sets the currency code for currency formatting.</summary>
    /// <value>The currency code (e.g., "USD", "EUR"), or null to use default currency formatting.</value>
    [Prop] public string? Currency { get; set; }

    /// <summary>Gets or sets the target type name for frontend validation and formatting.</summary>
    /// <value>The target type name, or null for automatic detection.</value>
    [Prop] public string? TargetType { get; set; }

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this number input can bind to and work with.
    /// Supports all standard .NET numeric types including signed, unsigned, and floating-point types.
    /// </summary>
    /// <returns>An array of supported numeric types including integers, floating-point numbers, and their nullable variants.</returns>
    public Type[] SupportedStateTypes() => [
        // Signed numeric types
        typeof(short), typeof(short?),
        typeof(int), typeof(int?),
        typeof(long), typeof(long?),
        typeof(float), typeof(float?),
        typeof(double), typeof(double?),
        typeof(decimal), typeof(decimal?),

        // Unsigned integer types
        typeof(byte), typeof(byte?)
    ];
}

/// <summary>
/// Generic number input control that provides type-safe numeric input functionality for various .NET numeric types.
/// Supports comprehensive numeric validation, formatting, range constraints, and multiple input variants
/// with automatic type-specific defaults and precision handling.
/// </summary>
/// <typeparam name="TNumber">The type of the numeric value (int, double, decimal, float, long, short, byte, or their nullable variants).</typeparam>
public record NumberInput<TNumber> : NumberInputBase, IInput<TNumber>, IAnyNumberInput
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the number input.</param>
    /// <param name="formatStyle">The formatting style for displaying numeric values.</param>
    public NumberInput(IAnyState state, string? placeholder = null, bool disabled = false, NumberInputs variant = NumberInputs.Number, NumberFormatStyle formatStyle = NumberFormatStyle.Decimal)
        : this(placeholder, disabled, variant, formatStyle)
    {
        var typedState = state.As<TNumber>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and state setter function.
    /// </summary>
    /// <param name="value">The initial numeric value.</param>
    /// <param name="state">Function to update the state when the numeric value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the number input.</param>
    /// <param name="formatStyle">The formatting style for displaying numeric values.</param>
    public NumberInput(TNumber value, Action<TNumber> state, string? placeholder = null, bool disabled = false, NumberInputs variant = NumberInputs.Number, NumberFormatStyle formatStyle = NumberFormatStyle.Decimal)
        : this(placeholder, disabled, variant, formatStyle)
    {
        OnChange = e => state(e.Value);
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the number input.</param>
    /// <param name="formatStyle">The formatting style for displaying numeric values.</param>
    public NumberInput(string? placeholder = null, bool disabled = false, NumberInputs variant = NumberInputs.Number, NumberFormatStyle formatStyle = NumberFormatStyle.Decimal)
    {
        Placeholder = placeholder;
        Disabled = disabled;
        Variant = variant;
        FormatStyle = formatStyle;
    }

    /// <summary>Gets the current numeric value.</summary>
    /// <value>The numeric value of the specified type.</value>
    [Prop] public TNumber Value { get; } = default!;

    /// <summary>Gets or sets whether the input accepts null values.</summary>
    /// <value>true if null values are allowed; automatically determined based on TNumber type.</value>
    [Prop] public bool Nullable { get; set; } = typeof(TNumber).IsNullableType();

    /// <summary>Gets the event handler called when the numeric value changes.</summary>
    /// <value>The change event handler, or null if no handler is set.</value>
    [Event] public Action<Event<IInput<TNumber>, TNumber>>? OnChange { get; }
}

/// <summary>
/// Provides extension methods for creating and configuring number inputs with fluent syntax.
/// Includes intelligent type-based defaults, specialized input variants (sliders, money inputs),
/// and comprehensive configuration methods for numeric input controls.
/// </summary>
public static class NumberInputExtensions
{
    /// <summary>
    /// Creates a slider-style number input from a state object.
    /// Convenience method that creates a number input with the Slider variant for visual range selection.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="formatStyle">The formatting style for displaying numeric values.</param>
    /// <returns>A slider-style number input bound to the state object.</returns>
    public static NumberInputBase ToSliderInput(this IAnyState state, string? placeholder = null, bool disabled = false, NumberFormatStyle formatStyle = NumberFormatStyle.Decimal)
    {
        return state.ToNumberInput(placeholder, disabled, NumberInputs.Slider, formatStyle);
    }

    /// <summary>
    /// Creates a number input from a state object with automatic type detection and intelligent defaults.
    /// Automatically configures precision, step, min/max values, and target type based on the numeric type.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the number input.</param>
    /// <param name="formatStyle">The formatting style for displaying numeric values.</param>
    /// <returns>A number input bound to the state object with type-appropriate defaults.</returns>
    public static NumberInputBase ToNumberInput(this IAnyState state, string? placeholder = null, bool disabled = false, NumberInputs variant = NumberInputs.Number, NumberFormatStyle formatStyle = NumberFormatStyle.Decimal)
    {
        var type = state.GetStateType();
        Type genericType = typeof(NumberInput<>).MakeGenericType(type);
        NumberInputBase input = (NumberInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant, formatStyle)!;
        input.ScaffoldDefaults(null, type);
        return input;
    }

    /// <summary>
    /// Creates a currency-formatted number input from a state object.
    /// Convenience method that creates a number input with Currency formatting and specified currency code.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the number input.</param>
    /// <param name="currency">The currency code for formatting (e.g., "USD", "EUR").</param>
    /// <returns>A currency-formatted number input bound to the state object.</returns>
    public static NumberInputBase ToMoneyInput(this IAnyState state, string? placeholder = null, bool disabled = false, NumberInputs variant = NumberInputs.Number, string currency = "USD")
        => state.ToNumberInput(placeholder, disabled, variant, NumberFormatStyle.Currency).Currency(currency);

    /// <summary>
    /// Internal method for setting intelligent defaults based on the numeric type.
    /// Automatically configures precision, step values, min/max ranges, target type, and currency defaults.
    /// </summary>
    /// <param name="input">The number input to configure.</param>
    /// <param name="name">The property name (currently unused, reserved for future label generation).</param>
    /// <param name="type">The numeric type to base defaults on.</param>
    /// <returns>The configured number input with type-appropriate defaults.</returns>
    internal static IAnyNumberInput ScaffoldDefaults(this IAnyNumberInput input, string? name, Type type)
    {
        input.Precision ??= type.SuggestPrecision();
        input.Step ??= type.SuggestStep();
        input.Min ??= type.SuggestMin();
        input.Max ??= type.SuggestMax();

        // Set target type for frontend validation
        input.TargetType = GetTargetTypeName(type);

        // Add default currency for Currency style inputs
        if (input.FormatStyle == NumberFormatStyle.Currency && string.IsNullOrEmpty(input.Currency))
        {
            input.Currency = "USD";
        }

        return input;
    }

    /// <summary>
    /// Gets the target type name for frontend validation and formatting.
    /// Handles nullable types by extracting the underlying type name.
    /// </summary>
    /// <param name="type">The .NET type to get the name for.</param>
    /// <returns>The lowercase type name suitable for frontend use.</returns>
    private static string GetTargetTypeName(Type type)
    {
        // Handle nullable types
        var underlyingType = Nullable.GetUnderlyingType(type);
        var actualType = underlyingType ?? type;

        return actualType.Name.ToLowerInvariant();
    }

    /// <summary>Sets the placeholder text for the number input.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="placeholder">The placeholder text to display when the input is empty.</param>
    /// <returns>The number input with the specified placeholder text.</returns>
    public static NumberInputBase Placeholder(this NumberInputBase widget, string placeholder)
    {
        return widget with { Placeholder = placeholder };
    }

    /// <summary>Sets the disabled state of the number input.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="enabled">Whether the input should be disabled (note: parameter name suggests enabled but sets disabled state).</param>
    /// <returns>The number input with the specified disabled state.</returns>
    public static NumberInputBase Disabled(this NumberInputBase widget, bool enabled = true)
    {
        return widget with { Disabled = enabled };
    }

    /// <summary>Sets the minimum allowed value for the number input.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="min">The minimum value that can be entered.</param>
    /// <returns>The number input with the specified minimum value constraint.</returns>
    public static NumberInputBase Min(this NumberInputBase widget, double min)
    {
        return widget with { Min = min };
    }

    /// <summary>Sets the maximum allowed value for the number input.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="max">The maximum value that can be entered.</param>
    /// <returns>The number input with the specified maximum value constraint.</returns>
    public static NumberInputBase Max(this NumberInputBase widget, double max)
    {
        return widget with { Max = max };
    }

    /// <summary>Sets the step increment for the number input (used for sliders and increment/decrement buttons).</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="step">The step value for incremental changes.</param>
    /// <returns>The number input with the specified step increment.</returns>
    public static NumberInputBase Step(this NumberInputBase widget, double step)
    {
        return widget with { Step = step };
    }

    /// <summary>Sets the visual variant of the number input.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="variant">The visual variant (Number or Slider).</param>
    /// <returns>The number input with the specified variant.</returns>
    public static NumberInputBase Variant(this NumberInputBase widget, NumberInputs variant)
    {
        return widget with { Variant = variant };
    }

    /// <summary>Sets the number of decimal places to display and accept.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="precision">The number of decimal places for precision control.</param>
    /// <returns>The number input with the specified precision.</returns>
    public static NumberInputBase Precision(this NumberInputBase widget, int precision)
    {
        return widget with { Precision = precision };
    }

    /// <summary>Sets the formatting style for displaying numeric values.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="formatStyle">The format style (Decimal, Currency, or Percent).</param>
    /// <returns>The number input with the specified formatting style.</returns>
    public static NumberInputBase FormatStyle(this NumberInputBase widget, NumberFormatStyle formatStyle)
    {
        return widget with { FormatStyle = formatStyle };
    }

    /// <summary>Sets the currency code for currency formatting.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="currency">The currency code (e.g., "USD", "EUR", "GBP").</param>
    /// <returns>The number input with the specified currency formatting.</returns>
    public static NumberInputBase Currency(this NumberInputBase widget, string currency)
    {
        return widget with { Currency = currency };
    }

    /// <summary>Sets the validation error message for the number input.</summary>
    /// <param name="widget">The number input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The number input with the specified validation error.</returns>
    public static NumberInputBase Invalid(this NumberInputBase widget, string invalid)
    {
        return widget with { Invalid = invalid };
    }

}