using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual variants available for date and time input controls.
/// Each variant provides a different interface for selecting dates, times, or both.
/// </summary>
public enum DateTimeInputs
{
    /// <summary>Date-only input for selecting calendar dates without time components.</summary>
    Date,
    /// <summary>Combined date and time input for selecting both date and time values.</summary>
    DateTime,
    /// <summary>Time-only input for selecting time values without date components.</summary>
    Time
}

/// <summary>
/// Interface for date and time input controls that extends IAnyInput with date/time-specific properties.
/// Provides functionality for date and time selection including visual variants, placeholder text,
/// and custom formatting for different date/time representations.
/// </summary>
public interface IAnyDateTimeInput : IAnyInput
{
    /// <summary>Gets or sets the visual variant of the date/time input.</summary>
    /// <value>The input variant (Date, DateTime, or Time).</value>
    public DateTimeInputs Variant { get; set; }

    /// <summary>Gets or sets the placeholder text displayed when the input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the date/time format string for displaying and parsing values.</summary>
    /// <value>The format string (e.g., "yyyy-MM-dd", "HH:mm:ss"), or null to use the default format.</value>
    public string? Format { get; set; }
}

/// <summary>
/// Abstract base class for date and time input controls that provides comprehensive date/time functionality.
/// Supports multiple date and time types including DateTime, DateTimeOffset, DateOnly, and TimeOnly
/// with automatic type conversion, custom formatting, and validation features.
/// </summary>
public abstract record DateTimeInputBase : WidgetBase<DateTimeInputBase>, IAnyDateTimeInput
{
    /// <summary>Gets or sets the visual variant of the date/time input.</summary>
    /// <value>The input variant (Date, DateTime, or Time).</value>
    [Prop] public DateTimeInputs Variant { get; set; }

    /// <summary>Gets or sets the placeholder text displayed when the input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    [Prop] public string? Placeholder { get; set; }

    /// <summary>Gets or sets the date/time format string for displaying and parsing values.</summary>
    /// <value>The format string (e.g., "yyyy-MM-dd", "HH:mm:ss"), or null to use the default format.</value>
    [Prop] public string? Format { get; set; }

    /// <summary>Gets or sets whether the input is disabled.</summary>
    /// <value>true if the input is disabled; false if it's interactive.</value>
    [Prop] public bool Disabled { get; set; }

    /// <summary>Gets or sets the validation error message.</summary>
    /// <value>The error message, or null if the input is valid.</value>
    [Prop] public string? Invalid { get; set; }

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IAnyInput>, ValueTask>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this date/time input can bind to and work with.
    /// Supports various .NET date and time types with automatic conversion between them.
    /// </summary>
    /// <returns>An array of supported date/time types including DateTime, DateTimeOffset, DateOnly, and TimeOnly (with nullable variants).</returns>
    public Type[] SupportedStateTypes() =>
    [
        // DateTime types
        typeof(DateTime), typeof(DateTime?),
        typeof(DateTimeOffset), typeof(DateTimeOffset?),
        typeof(DateOnly), typeof(DateOnly?),
        typeof(TimeOnly), typeof(TimeOnly?),
    ];
}

/// <summary>
/// Generic date and time input control that provides type-safe date/time functionality for various .NET date/time types.
/// Supports DateTime, DateTimeOffset, DateOnly, and TimeOnly with automatic nullable detection and type conversion.
/// Provides flexible input variants for date-only, time-only, or combined date/time selection.
/// </summary>
/// <typeparam name="TDate">The type of the date/time value (DateTime, DateTimeOffset, DateOnly, TimeOnly, or their nullable variants).</typeparam>
public record DateTimeInput<TDate> : DateTimeInputBase, IInput<TDate>
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the date/time input.</param>
    [OverloadResolutionPriority(1)]
    public DateTimeInput(IAnyState state, string? placeholder = null, bool disabled = false, DateTimeInputs variant = DateTimeInputs.Date) : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TDate>();
        Value = typedState.Value;
        OnChange = e => { typedState.Set(e.Value); return ValueTask.CompletedTask; };
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and async change handler.
    /// </summary>
    /// <param name="value">The initial date/time value.</param>
    /// <param name="onChange">Async event handler called when the date/time value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the date/time input.</param>
    [OverloadResolutionPriority(1)]
    public DateTimeInput(TDate value, Func<Event<IInput<TDate>, TDate>, ValueTask> onChange, string? placeholder = null, bool disabled = false, DateTimeInputs variant = DateTimeInputs.Date) : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and synchronous change handler.
    /// Compatibility overload for Action-based change handlers.
    /// </summary>
    /// <param name="value">The initial date/time value.</param>
    /// <param name="onChange">Event handler called when the date/time value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the date/time input.</param>
    public DateTimeInput(TDate value, Action<Event<IInput<TDate>, TDate>> onChange, string? placeholder = null, bool disabled = false, DateTimeInputs variant = DateTimeInputs.Date) : this(placeholder, disabled, variant)
    {
        OnChange = e => { onChange(e); return ValueTask.CompletedTask; };
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the date/time input.</param>
    public DateTimeInput(string? placeholder = null, bool disabled = false, DateTimeInputs variant = DateTimeInputs.Date)
    {
        Variant = variant;
        Placeholder = placeholder;
        Disabled = disabled;
    }

    /// <summary>Gets or sets the current date/time value.</summary>
    /// <value>The date/time value of the specified type.</value>
    [Prop] public TDate Value { get; set; } = default!;

    /// <summary>Gets or sets whether the input accepts null values.</summary>
    /// <value>true if null values are allowed; automatically determined based on TDate type.</value>
    [Prop] public bool Nullable { get; set; } = typeof(TDate) == typeof(DateTime?) || typeof(TDate) == typeof(DateTimeOffset?) || typeof(TDate) == typeof(DateOnly?) || typeof(TDate) == typeof(TimeOnly?);

    /// <summary>Gets or sets the event handler called when the date/time value changes.</summary>
    /// <value>The async change event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IInput<TDate>, TDate>, ValueTask>? OnChange { get; set; }
}

/// <summary>
/// Provides extension methods for creating and configuring date/time inputs with fluent syntax.
/// Includes comprehensive type conversion between various .NET date/time types, automatic nullable detection,
/// and convenient methods for creating different date/time input variants.
/// </summary>
public static class DateTimeInputExtensions
{
    /// <summary>
    /// Creates a date-only input from a state object with automatic type conversion.
    /// Convenience method that creates a date input with the Date variant.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the date input (defaults to Date).</param>
    /// <returns>A date input bound to the state object.</returns>
    public static DateTimeInputBase ToDateInput(this IAnyState state, string? placeholder = null, bool disabled = false,
        DateTimeInputs variant = DateTimeInputs.Date)
        => ToDateTimeInput(state, placeholder, disabled, variant);

    /// <summary>
    /// Creates a date/time input from a state object with comprehensive type conversion and nullable handling.
    /// Supports automatic conversion between DateTime, DateTimeOffset, DateOnly, TimeOnly, and string types.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the date/time input (defaults to DateTime).</param>
    /// <returns>A date/time input bound to the state object with automatic type conversion.</returns>
    public static DateTimeInputBase ToDateTimeInput(this IAnyState state, string? placeholder = null, bool disabled = false, DateTimeInputs variant = DateTimeInputs.DateTime)
    {
        var stateType = state.GetStateType();
        var isNullable = stateType.IsNullableType();

        // Create the appropriate DateTimeInput based on the original state type
        if (isNullable)
        {
            var dateValue = ConvertToDateValue<object?>(state);
            var input = new DateTimeInput<object?>(dateValue, e => SetStateValue(state, e.Value), placeholder, disabled, variant);
            input.ScaffoldDefaults(null!, stateType);
            input.Nullable = true;
            return input;
        }
        else
        {
            var dateValue = ConvertToDateValue<object>(state);
            var input = new DateTimeInput<object>(dateValue, e => SetStateValue(state, e.Value), placeholder, disabled, variant);
            input.ScaffoldDefaults(null!, stateType);
            return input;
        }
    }

    private static T ConvertToDateValue<T>(IAnyState state)
    {
        var stateType = state.GetStateType();
        var value = state.As<object>().Value;

        // Convert to appropriate date value based on type
        var dateValue = stateType switch
        {
            // DateTime types - direct conversion
            _ when stateType == typeof(DateTime) => value,
            _ when stateType == typeof(DateTime?) => value,
            _ when stateType == typeof(DateTimeOffset) => value,
            _ when stateType == typeof(DateTimeOffset?) => value,
            _ when stateType == typeof(DateOnly) => value,
            _ when stateType == typeof(DateOnly?) => value,
            _ when stateType == typeof(TimeOnly) => value,
            _ when stateType == typeof(TimeOnly?) => value,

            // Other types - try BestGuessConvert, fallback to current date
            _ => Core.Utils.BestGuessConvert(value, typeof(DateTime)) ?? DateTime.Now
        };

        return (T)dateValue!;
    }

    private static void SetStateValue(IAnyState state, object? dateValue)
    {
        var stateType = state.GetStateType();
        var isNullable = stateType.IsNullableType();

        // Convert date value back to the original state type
        var convertedValue = stateType switch
        {
            // DateTime types - direct conversion
            _ when stateType == typeof(DateTime) =>
                dateValue is DateTime dt ? dt :
                dateValue is string s ? DateTime.Parse(s) :
                DateTime.Now,
            _ when stateType == typeof(DateTime?) =>
                dateValue is null ? null :
                dateValue is DateTime dt ? dt :
                dateValue is string s ? DateTime.Parse(s) :
                (DateTime?)DateTime.Now,
            _ when stateType == typeof(DateTimeOffset) => dateValue ?? DateTimeOffset.Now,
            _ when stateType == typeof(DateTimeOffset?) => dateValue,
            _ when stateType == typeof(DateOnly) =>
                dateValue is DateOnly d ? d :
                dateValue is string s ? DateOnly.FromDateTime(DateTime.Parse(s)) :
                dateValue is DateTime dt ? DateOnly.FromDateTime(dt) :
                DateOnly.FromDateTime(DateTime.Now),
            _ when stateType == typeof(DateOnly?) =>
                dateValue is null ? null :
                dateValue is DateOnly d ? d :
                dateValue is string s ? DateOnly.FromDateTime(DateTime.Parse(s)) :
                dateValue is DateTime dt ? DateOnly.FromDateTime(dt) :
                (DateOnly?)DateOnly.FromDateTime(DateTime.Now),
            _ when stateType == typeof(TimeOnly) =>
                dateValue is TimeOnly t ? t :
                dateValue is string s ? ParseTimeOnly(s) :
                dateValue is DateTime dt ? TimeOnly.FromDateTime(dt) :
                TimeOnly.FromDateTime(DateTime.Now),
            _ when stateType == typeof(TimeOnly?) =>
                dateValue is null ? null :
                dateValue is string s && string.IsNullOrWhiteSpace(s) ? null :
                dateValue is TimeOnly t ? t :
                dateValue is string s2 ? ParseTimeOnly(s2) :
                dateValue is DateTime dt ? TimeOnly.FromDateTime(dt) :
                (TimeOnly?)TimeOnly.FromDateTime(DateTime.Now),
            _ when stateType == typeof(string) => dateValue?.ToString() ?? DateTime.Now.ToString("O"),

            // Other types - use BestGuessConvert
            _ => Core.Utils.BestGuessConvert(dateValue, stateType) ?? DateTime.Now
        };

        // Set the state value
        state.As<object>().Set(convertedValue!);
    }

    private static TimeOnly ParseTimeOnly(string timeString)
    {
        // Try different time formats
        var formats = new[] { "HH:mm:ss", "HH:mm", "H:mm:ss", "H:mm" };

        foreach (var format in formats)
        {
            if (TimeOnly.TryParseExact(timeString, format, null, System.Globalization.DateTimeStyles.None, out var result))
            {
                return result;
            }
        }

        // If all parsing fails, return current time
        return TimeOnly.FromDateTime(DateTime.Now);
    }

    /// <summary>
    /// Creates a time-only input from a state object with automatic type conversion.
    /// Convenience method that creates a time input with the Time variant.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>A time input bound to the state object.</returns>
    public static DateTimeInputBase ToTimeInput(this IAnyState state, string? placeholder = null, bool disabled = false)
        => state.ToDateTimeInput(placeholder, disabled, DateTimeInputs.Time);

    /// <summary>
    /// Internal method for setting default values during scaffolding operations.
    /// Automatically generates placeholder text from property names when not explicitly set.
    /// </summary>
    /// <param name="input">The date/time input to configure.</param>
    /// <param name="name">The property name to use for generating placeholder text.</param>
    /// <param name="type">The property type for additional context.</param>
    /// <returns>The configured date/time input with default values applied.</returns>
    internal static IAnyDateTimeInput ScaffoldDefaults(this IAnyDateTimeInput input, string? name, Type type)
    {
        if (string.IsNullOrEmpty(input.Placeholder))
        {
            input.Placeholder = Utils.SplitPascalCase(name) ?? name;
        }

        return input;
    }

    /// <summary>Sets the disabled state of the date/time input.</summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="disabled">Whether the input should be disabled.</param>
    /// <returns>The date/time input with the specified disabled state.</returns>
    public static DateTimeInputBase Disabled(this DateTimeInputBase widget, bool disabled = true) => widget with { Disabled = disabled };

    /// <summary>Sets the visual variant of the date/time input.</summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="variant">The visual variant (Date, DateTime, or Time).</param>
    /// <returns>The date/time input with the specified variant.</returns>
    public static DateTimeInputBase Variant(this DateTimeInputBase widget, DateTimeInputs variant) => widget with { Variant = variant };

    /// <summary>Sets the placeholder text for the date/time input.</summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="placeholder">The placeholder text to display when the input is empty.</param>
    /// <returns>The date/time input with the specified placeholder text.</returns>
    public static DateTimeInputBase Placeholder(this DateTimeInputBase widget, string placeholder) => widget with { Placeholder = placeholder };

    /// <summary>Sets the date/time format string for the input.</summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="format">The format string (e.g., "yyyy-MM-dd", "HH:mm:ss") for displaying and parsing values.</param>
    /// <returns>The date/time input with the specified format.</returns>
    public static DateTimeInputBase Format(this DateTimeInputBase widget, string format) => widget with { Format = format };

    /// <summary>Sets the validation error message for the date/time input.</summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The date/time input with the specified validation error.</returns>
    public static DateTimeInputBase Invalid(this DateTimeInputBase widget, string? invalid) => widget with { Invalid = invalid };


    /// <summary>
    /// Sets the blur event handler for the date/time input.
    /// This method allows you to configure the date/time input's blur behavior,
    /// enabling it to perform custom actions when the input loses focus.
    /// </summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new date/time input instance with the updated blur handler.</returns>
    [OverloadResolutionPriority(1)]
    public static DateTimeInputBase HandleBlur(this DateTimeInputBase widget, Func<Event<IAnyInput>, ValueTask> onBlur)
    {
        return widget with { OnBlur = onBlur };
    }

    /// <summary>
    /// Sets the blur event handler for the date/time input.
    /// Compatibility overload for Action-based event handlers.
    /// </summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new date/time input instance with the updated blur handler.</returns>
    public static DateTimeInputBase HandleBlur(this DateTimeInputBase widget, Action<Event<IAnyInput>> onBlur)
    {
        return widget.HandleBlur(onBlur.ToValueTask());
    }

    /// <summary>
    /// Sets a simple blur event handler for the date/time input.
    /// This method allows you to configure the date/time input's blur behavior with
    /// a simple action that doesn't require the input event context.
    /// </summary>
    /// <param name="widget">The date/time input to configure.</param>
    /// <param name="onBlur">The simple action to perform when the input loses focus.</param>
    /// <returns>A new date/time input instance with the updated blur handler.</returns>
    public static DateTimeInputBase HandleBlur(this DateTimeInputBase widget, Action onBlur)
    {
        return widget.HandleBlur(_ => { onBlur(); return ValueTask.CompletedTask; });
    }
}