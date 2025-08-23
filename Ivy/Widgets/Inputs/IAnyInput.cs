
using Ivy.Core;

namespace Ivy.Widgets.Inputs;

/// <summary>
/// Base interface for all input controls in the Ivy Framework. Defines common properties and behaviors
/// that all input widgets must implement, including validation state, disabled state, and blur events.
/// This interface provides the foundation for type-safe input handling and form integration.
/// </summary>
public interface IAnyInput
{
    /// <summary>
    /// Gets or sets a value indicating whether the input control is disabled.
    /// When disabled, the input cannot receive focus, accept user input, or trigger events,
    /// and is typically rendered with a visual indication of its disabled state.
    /// </summary>
    /// <value>true if the input is disabled; false if the input is enabled and interactive.</value>
    [Prop] public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the validation error message for this input control.
    /// When not null, indicates that the input contains invalid data and displays
    /// the error message to provide feedback to the user about what needs to be corrected.
    /// </summary>
    /// <value>
    /// The validation error message, or null if the input is valid.
    /// The message should be descriptive and help users understand how to fix the issue.
    /// </value>
    [Prop] public string? Invalid { get; set; }

    /// <summary>
    /// Gets or sets the event handler called when the input control loses focus.
    /// The blur event is triggered when users navigate away from the input, typically
    /// used for validation, formatting, or other post-input processing.
    /// </summary>
    /// <value>An action that receives a <see cref="Event{T}"/> with this input as the source.</value>
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }

    /// <summary>
    /// Returns an array of types that this input control can bind to and work with.
    /// This method is used by the framework to determine type compatibility for automatic
    /// form generation and state binding, ensuring that inputs are matched with appropriate data types.
    /// </summary>
    /// <returns>
    /// An array of Type objects representing the data types this input can handle.
    /// For example, a text input might support string types, while a number input
    /// might support int, double, and decimal types.
    /// </returns>
    public Type[] SupportedStateTypes();
}

/// <summary>
/// Provides extension methods for configuring IAnyInput implementations with fluent syntax.
/// These methods enable easy configuration of common input properties and behaviors
/// across all input types in a consistent, chainable manner.
/// </summary>
public static class AnyInputExtensions
{
    /// <summary>
    /// Sets the disabled state of the input control.
    /// </summary>
    /// <param name="input">The input control to configure.</param>
    /// <param name="disabled">true to disable the input; false to enable it. Default is true.</param>
    /// <returns>The input control with the specified disabled state.</returns>
    public static IAnyInput Disabled(this IAnyInput input, bool disabled = true)
    {
        input.Disabled = disabled;
        return input;
    }

    /// <summary>
    /// Sets the validation error message for the input control.
    /// </summary>
    /// <param name="input">The input control to configure.</param>
    /// <param name="invalid">The validation error message, or null to clear any existing error.</param>
    /// <returns>The input control with the specified validation state.</returns>
    public static IAnyInput Invalid(this IAnyInput input, string? invalid)
    {
        input.Invalid = invalid;
        return input;
    }

    /// <summary>
    /// Sets the blur event handler for the input control.
    /// </summary>
    /// <param name="input">The input control to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus, or null to remove the handler.</param>
    /// <returns>The input control with the specified blur event handler.</returns>
    public static IAnyInput HandleBlur(this IAnyInput input, Action<Event<IAnyInput>>? onBlur)
    {
        input.OnBlur = onBlur;
        return input;
    }
}