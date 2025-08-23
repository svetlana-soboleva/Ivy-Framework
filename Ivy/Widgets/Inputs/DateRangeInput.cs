using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Interface for date range input controls that extends IAnyInput with date range-specific properties.
/// Provides functionality for selecting date ranges including placeholder text and custom date formatting.
/// </summary>
public interface IAnyDateRangeInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when the date range input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the date format string for displaying and parsing dates.</summary>
    /// <value>The date format string (e.g., "yyyy-MM-dd", "MM/dd/yyyy"), or null to use the default format.</value>
    public string? Format { get; set; }
}

/// <summary>
/// Abstract base class for date range input controls that provides date range selection functionality.
/// Supports selecting a range of dates with start and end dates, including nullable date ranges.
/// Provides date formatting, validation, and standard input control features.
/// </summary>
public abstract record DateRangeInputBase : WidgetBase<DateRangeInputBase>, IAnyDateRangeInput
{
    /// <summary>Gets or sets the placeholder text displayed when the input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    [Prop] public string? Placeholder { get; set; }

    /// <summary>Gets or sets the date format string for displaying and parsing dates.</summary>
    /// <value>The date format string (e.g., "yyyy-MM-dd", "MM/dd/yyyy"), or null to use the default format.</value>
    [Prop] public string? Format { get; set; }

    /// <summary>Gets or sets whether the input is disabled.</summary>
    /// <value>true if the input is disabled; false if it's interactive.</value>
    [Prop] public bool Disabled { get; set; }

    /// <summary>Gets or sets the validation error message.</summary>
    /// <value>The error message, or null if the input is valid.</value>
    [Prop] public string? Invalid { get; set; }

    /// <summary>Gets or sets whether the input accepts null values.</summary>
    /// <value>true if null date ranges are allowed; false if a date range is required.</value>
    [Prop] public bool Nullable { get; set; }

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this date range input can bind to and work with.
    /// Only supports tuple types with two DateOnly elements for representing date ranges.
    /// </summary>
    /// <returns>An array of supported tuple types for date ranges (DateOnly, DateOnly) and (DateOnly?, DateOnly?).</returns>
    public Type[] SupportedStateTypes() =>
    [
        // Only DateOnly tuple types
        typeof((DateOnly, DateOnly)), typeof((DateOnly?, DateOnly?)),
    ];
}

/// <summary>
/// Generic date range input control that provides date range selection functionality for tuple-based date ranges.
/// Supports both non-nullable (DateOnly, DateOnly) and nullable (DateOnly?, DateOnly?) date range tuples.
/// Provides date formatting, validation, and automatic nullable detection based on the generic type.
/// </summary>
/// <typeparam name="TDateRange">The type of the date range tuple ((DateOnly, DateOnly) or (DateOnly?, DateOnly?)).</typeparam>
public record DateRangeInput<TDateRange> : DateRangeInputBase, IInput<TDateRange>
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// Automatically detects whether the date range is nullable based on the generic type.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    public DateRangeInput(IAnyState state, string? placeholder = null, bool disabled = false) : this(placeholder, disabled)
    {
        var typedState = state.As<TDateRange>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
        Nullable = typeof(TDateRange) == typeof((DateOnly?, DateOnly?));
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and change handler.
    /// Automatically detects whether the date range is nullable based on the generic type.
    /// </summary>
    /// <param name="value">The initial date range value as a tuple.</param>
    /// <param name="onChange">Event handler called when the date range value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    public DateRangeInput(TDateRange value, Action<Event<IInput<TDateRange>, TDateRange>> onChange, string? placeholder = null, bool disabled = false) : this(placeholder, disabled)
    {
        OnChange = onChange;
        Value = value;
        Nullable = typeof(TDateRange) == typeof((DateOnly?, DateOnly?));
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    public DateRangeInput(string? placeholder = null, bool disabled = false)
    {
        Placeholder = placeholder;
        Disabled = disabled;
    }

    /// <summary>Gets or sets the current date range value as a tuple.</summary>
    /// <value>The date range tuple containing start and end dates.</value>
    [Prop] public TDateRange Value { get; set; } = default!;

    /// <summary>Gets or sets the event handler called when the date range value changes.</summary>
    /// <value>The change event handler, or null if no handler is set.</value>
    [Event] public Action<Event<IInput<TDateRange>, TDateRange>>? OnChange { get; set; }
}

/// <summary>
/// Provides extension methods for creating and configuring date range inputs with fluent syntax.
/// Includes methods for state binding with automatic tuple type validation and convenient
/// configuration of date formatting, validation, and appearance options.
/// </summary>
public static class DateRangeInputExtensions
{
    /// <summary>
    /// Creates a date range input from a state object with automatic tuple type validation.
    /// Validates that the state type is a tuple with exactly two elements for date range representation.
    /// </summary>
    /// <param name="state">The state object to bind to (must be a tuple type with two elements).</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>A date range input bound to the state object.</returns>
    /// <exception cref="Exception">Thrown when the state type is not a tuple with exactly two elements.</exception>
    public static DateRangeInputBase ToDateRangeInput(this IAnyState state, string? placeholder = null, bool disabled = false)
    {
        var type = state.GetStateType();

        // Check if it's a tuple type with 2 elements
        if (!type.IsGenericType || type.GetGenericArguments().Length != 2)
        {
            throw new Exception("DateRangeInput can only be used with a tuple of two elements");
        }

        Type genericType = typeof(DateRangeInput<>).MakeGenericType(type);
        DateRangeInputBase input = (DateRangeInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled)!;
        return input;
    }

    /// <summary>Sets the disabled state of the date range input.</summary>
    /// <param name="widget">The date range input to configure.</param>
    /// <param name="disabled">Whether the input should be disabled.</param>
    /// <returns>The date range input with the specified disabled state.</returns>
    public static DateRangeInputBase Disabled(this DateRangeInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }

    /// <summary>Sets the placeholder text for the date range input.</summary>
    /// <param name="widget">The date range input to configure.</param>
    /// <param name="placeholder">The placeholder text to display when the input is empty.</param>
    /// <returns>The date range input with the specified placeholder text.</returns>
    public static DateRangeInputBase Placeholder(this DateRangeInputBase widget, string placeholder)
    {
        return widget with { Placeholder = placeholder };
    }

    /// <summary>Sets the date format string for the date range input.</summary>
    /// <param name="widget">The date range input to configure.</param>
    /// <param name="format">The date format string (e.g., "yyyy-MM-dd", "MM/dd/yyyy") for displaying and parsing dates.</param>
    /// <returns>The date range input with the specified date format.</returns>
    public static DateRangeInputBase Format(this DateRangeInputBase widget, string format)
    {
        return widget with { Format = format };
    }

    /// <summary>Sets the validation error message for the date range input.</summary>
    /// <param name="widget">The date range input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The date range input with the specified validation error.</returns>
    public static DateRangeInputBase Invalid(this DateRangeInputBase widget, string? invalid)
    {
        return widget with { Invalid = invalid };
    }

    /// <summary>Sets whether the date range input accepts null values.</summary>
    /// <param name="widget">The date range input to configure.</param>
    /// <param name="nullable">Whether null date ranges should be allowed.</param>
    /// <returns>The date range input with the specified nullable setting.</returns>
    public static DateRangeInputBase Nullable(this DateRangeInputBase widget, bool nullable = true)
    {
        return widget with { Nullable = nullable };
    }
}