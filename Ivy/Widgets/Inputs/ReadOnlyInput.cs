using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Interface for read-only input controls that extends IAnyInput for non-editable value display.
/// Provides a contract for input controls that display values without allowing user modification,
/// typically used for displaying computed values, system information, or locked data fields.
/// </summary>
public interface IAnyReadOnlyInput : IAnyInput
{
}

/// <summary>
/// Generic read-only input control that displays values without allowing user modification.
/// Provides a non-editable display of data with optional copy functionality, validation state display,
/// and state binding support. Ideal for showing computed values, system information, or locked fields
/// while maintaining the input control interface for consistent form layouts.
/// </summary>
/// <typeparam name="TValue">The type of the value to display (can be any type with string representation).</typeparam>
public record ReadOnlyInput<TValue> : WidgetBase<ReadOnlyInput<TValue>>, IInput<TValue>, IAnyReadOnlyInput
{
    /// <summary>
    /// Initializes a new read-only input bound to a state object for automatic value synchronization.
    /// The input will display the current state value and update when the state changes,
    /// though user interaction is disabled.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates and change handling.</param>
    [OverloadResolutionPriority(1)]
    public ReadOnlyInput(IAnyState state)
    {
        var typedState = state.As<TValue>();
        Value = typedState.Value;
        OnChange = e => { typedState.Set(e.Value); return ValueTask.CompletedTask; };
    }

    /// <summary>
    /// Initializes a new read-only input with an explicit value and optional async change handler.
    /// Useful for displaying static values or when manual change handling is required.
    /// </summary>
    /// <param name="value">The value to display in the read-only input.</param>
    /// <param name="onChange">Optional async event handler called when the value would change (though user interaction is disabled).</param>
    [OverloadResolutionPriority(1)]
    public ReadOnlyInput(TValue value, Func<Event<IInput<TValue>, TValue>, ValueTask>? onChange = null)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new read-only input with an explicit value and optional synchronous change handler.
    /// Compatibility overload for Action-based change handlers.
    /// Useful for displaying static values or when manual change handling is required.
    /// </summary>
    /// <param name="value">The value to display in the read-only input.</param>
    /// <param name="onChange">Optional event handler called when the value would change (though user interaction is disabled).</param>
    public ReadOnlyInput(TValue value, Action<Event<IInput<TValue>, TValue>>? onChange = null)
    {
        OnChange = onChange == null ? null : e => { onChange(e); return ValueTask.CompletedTask; };
        Value = value;
    }

    /// <summary>Gets the current value displayed in the read-only input.</summary>
    /// <value>The value of type TValue that is displayed to the user.</value>
    [Prop] public TValue Value { get; }

    /// <summary>Gets or sets whether the input appears disabled.</summary>
    /// <value>true if the input should appear disabled; false for normal read-only appearance.</value>
    /// <remarks>Read-only inputs are inherently non-interactive, but this affects visual styling.</remarks>
    [Prop] public bool Disabled { get; set; }

    /// <summary>Gets or sets the validation error message to display.</summary>
    /// <value>The error message, or null if the input is valid.</value>
    /// <remarks>Useful for showing validation errors on computed or derived values.</remarks>
    [Prop] public string? Invalid { get; set; }

    /// <summary>Gets or sets whether to show a copy button for the displayed value.</summary>
    /// <value>true to show a copy button (default); false to hide the copy functionality.</value>
    /// <remarks>Allows users to copy the read-only value to the clipboard for use elsewhere.</remarks>
    [Prop] public bool ShowCopyButton { get; set; } = true;

    /// <summary>Gets the event handler called when the value would change.</summary>
    /// <value>The async change event handler, or null if no handler is set.</value>
    /// <remarks>While users cannot directly modify read-only inputs, this supports programmatic value updates.</remarks>
    [Event] public Func<Event<IInput<TValue>, TValue>, ValueTask>? OnChange { get; }

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IAnyInput>, ValueTask>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this read-only input can bind to and work with.
    /// Supports any object type since read-only inputs only display values using their string representation.
    /// </summary>
    /// <returns>An array containing the object type, indicating universal type support.</returns>
    public Type[] SupportedStateTypes() => [typeof(object)];
}

/// <summary>
/// Provides extension methods for creating read-only input controls with automatic type detection.
/// Enables convenient conversion of state objects to read-only inputs for displaying non-editable values
/// in forms and user interfaces while maintaining consistent input control styling and behavior.
/// </summary>
public static class ReadOnlyInputExtensions
{
    /// <summary>
    /// Creates a read-only input control from a state object with automatic type detection.
    /// Uses reflection to create the appropriate generic ReadOnlyInput&lt;T&gt; instance based on the state's type,
    /// enabling type-safe read-only display of any state value.
    /// </summary>
    /// <param name="state">The state object to create a read-only input for.</param>
    /// <returns>A read-only input control bound to the state object with automatic type matching.</returns>
    /// <remarks>
    /// This method uses reflection to dynamically create the correct generic type instance,
    /// allowing read-only inputs to work with any state type without explicit type specification.
    /// The resulting input will display the state's current value and update automatically when the state changes.
    /// </remarks>
    public static IAnyReadOnlyInput ToReadOnlyInput(this IAnyState state)
    {
        var type = state.GetStateType();
        Type genericType = typeof(ReadOnlyInput<>).MakeGenericType(type);
        IAnyReadOnlyInput input = (IAnyReadOnlyInput)Activator.CreateInstance(genericType, state)!;
        return input;
    }

    /// <summary>
    /// Sets the blur event handler for the read-only input.
    /// This method allows you to configure the read-only input's blur behavior,
    /// enabling it to perform custom actions when the input loses focus.
    /// </summary>
    /// <param name="widget">The read-only input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new read-only input instance with the updated blur handler.</returns>
    [OverloadResolutionPriority(1)]
    public static IAnyReadOnlyInput HandleBlur<T>(this IAnyReadOnlyInput widget, Func<Event<IAnyInput>, ValueTask> onBlur) where T : notnull
    {
        if (widget is ReadOnlyInput<T> typedWidget)
        {
            return typedWidget with { OnBlur = onBlur };
        }
        throw new InvalidOperationException($"Widget is not of expected type ReadOnlyInput<{typeof(T).Name}>");
    }

    /// <summary>
    /// Sets the blur event handler for the read-only input.
    /// Compatibility overload for Action-based event handlers.
    /// </summary>
    /// <param name="widget">The read-only input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new read-only input instance with the updated blur handler.</returns>
    public static IAnyReadOnlyInput HandleBlur<T>(this IAnyReadOnlyInput widget, Action<Event<IAnyInput>> onBlur) where T : notnull
    {
        return widget.HandleBlur<T>(onBlur.ToValueTask());
    }

    /// <summary>
    /// Sets a simple blur event handler for the read-only input.
    /// This method allows you to configure the read-only input's blur behavior with
    /// a simple action that doesn't require the input event context.
    /// </summary>
    /// <param name="widget">The read-only input to configure.</param>
    /// <param name="onBlur">The simple action to perform when the input loses focus.</param>
    /// <returns>A new read-only input instance with the updated blur handler.</returns>
    public static IAnyReadOnlyInput HandleBlur<T>(this IAnyReadOnlyInput widget, Action onBlur) where T : notnull
    {
        return widget.HandleBlur<T>(_ => { onBlur(); return ValueTask.CompletedTask; });
    }
}