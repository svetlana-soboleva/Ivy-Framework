
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual variants available for feedback input controls.
/// Each variant provides a different user interface for collecting user feedback and ratings.
/// </summary>
public enum FeedbackInputs
{
    /// <summary>Star rating system for numerical feedback (typically 1-5 stars).</summary>
    Stars,
    /// <summary>Thumbs up/down system for binary positive/negative feedback.</summary>
    Thumbs,
    /// <summary>Emoji-based feedback system for emotional or satisfaction ratings.</summary>
    Emojis,
}

/// <summary>
/// Interface for feedback input controls that extends IAnyInput with feedback-specific properties.
/// Provides functionality for collecting user feedback and ratings including placeholder text
/// and visual variants for different feedback collection methods.
/// </summary>
public interface IAnyFeedbackInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when the feedback input is empty.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual variant of the feedback input.</summary>
    /// <value>The input variant (Stars, Thumbs, or Emojis).</value>
    public FeedbackInputs Variant { get; set; }
}

/// <summary>
/// Abstract base class for feedback input controls that provides user feedback and rating functionality.
/// Supports both boolean feedback (thumbs up/down) and numerical ratings (star ratings, emoji scales).
/// Automatically selects appropriate variants based on the underlying data type.
/// </summary>
public abstract record FeedbackInputBase : WidgetBase<FeedbackInputBase>, IAnyFeedbackInput
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

    /// <summary>Gets or sets the visual variant of the feedback input.</summary>
    /// <value>The input variant (Stars, Thumbs, or Emojis).</value>
    [Prop] public FeedbackInputs Variant { get; set; }

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IAnyInput>, ValueTask>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this feedback input can bind to and work with.
    /// Supports boolean types for binary feedback and integer types for numerical ratings.
    /// </summary>
    /// <returns>An array of supported types including bool and int (with nullable variants).</returns>
    public Type[] SupportedStateTypes() => [
        typeof(bool), typeof(bool?),
        typeof(int), typeof(int?),
    ];
}

/// <summary>
/// Generic feedback input control that provides type-safe feedback collection functionality.
/// Supports both boolean feedback (thumbs up/down) and numerical ratings (star ratings, emoji scales).
/// Automatically detects nullable types and provides appropriate feedback interfaces.
/// </summary>
/// <typeparam name="TNumber">The type of the feedback value (bool, bool?, int, or int?).</typeparam>
public record FeedbackInput<TNumber> : FeedbackInputBase, IInput<TNumber>
{
    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the feedback input.</param>
    [OverloadResolutionPriority(1)]
    public FeedbackInput(IAnyState state, string? placeholder = null, bool disabled = false, FeedbackInputs variant = FeedbackInputs.Stars)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TNumber>();
        Value = typedState.Value;
        OnChange = e => { typedState.Set(e.Value); return ValueTask.CompletedTask; };
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and async change handler.
    /// </summary>
    /// <param name="value">The initial feedback value.</param>
    /// <param name="onChange">Async function to handle feedback value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the feedback input.</param>
    [OverloadResolutionPriority(1)]
    public FeedbackInput(TNumber value, Func<Event<IInput<TNumber>, TNumber>, ValueTask> onChange, string? placeholder = null, bool disabled = false, FeedbackInputs variant = FeedbackInputs.Stars)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and state setter function.
    /// Compatibility overload for Action-based state setters.
    /// </summary>
    /// <param name="value">The initial feedback value.</param>
    /// <param name="state">Function to update the state when the feedback value changes.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the feedback input.</param>
    public FeedbackInput(TNumber value, Action<TNumber> state, string? placeholder = null, bool disabled = false, FeedbackInputs variant = FeedbackInputs.Stars)
        : this(placeholder, disabled, variant)
    {
        OnChange = e => { state(e.Value); return ValueTask.CompletedTask; };
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with basic configuration.
    /// </summary>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the feedback input.</param>
    public FeedbackInput(string? placeholder = null, bool disabled = false, FeedbackInputs variant = FeedbackInputs.Stars)
    {
        Placeholder = placeholder;
        Disabled = disabled;
        Variant = variant;
    }

    /// <summary>Gets the current feedback value.</summary>
    /// <value>The feedback value of the specified type.</value>
    [Prop] public TNumber Value { get; } = default!;

    /// <summary>Gets or sets whether the input accepts null values.</summary>
    /// <value>true if null values are allowed; automatically determined based on TNumber type.</value>
    [Prop] public bool Nullable { get; set; } = typeof(TNumber).IsNullableType();

    /// <summary>Gets the event handler called when the feedback value changes.</summary>
    /// <value>The async change event handler, or null if no handler is set.</value>
    [Event] public Func<Event<IInput<TNumber>, TNumber>, ValueTask>? OnChange { get; }
}

/// <summary>
/// Provides extension methods for creating and configuring feedback inputs with fluent syntax.
/// Includes intelligent variant selection based on data type and convenient configuration
/// methods for customizing feedback collection interfaces.
/// </summary>
public static class FeedbackInputExtensions
{
    /// <summary>
    /// Creates a feedback input from a state object with intelligent variant selection.
    /// Automatically selects Thumbs variant for boolean types and Stars variant for integer types.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="placeholder">Optional placeholder text displayed when the input is empty.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">Optional explicit variant selection; if null, automatically determined by type.</param>
    /// <returns>A feedback input bound to the state object with appropriate variant selection.</returns>
    public static FeedbackInputBase ToFeedbackInput(this IAnyState state, string? placeholder = null, bool disabled = false, FeedbackInputs? variant = null)
    {
        var type = state.GetStateType();

        variant ??= type == typeof(bool) || type == typeof(bool?) ? FeedbackInputs.Thumbs : FeedbackInputs.Stars;

        Type genericType = typeof(FeedbackInput<>).MakeGenericType(type);
        FeedbackInputBase input = (FeedbackInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant)!;
        return input;
    }

    /// <summary>Sets the placeholder text for the feedback input.</summary>
    /// <param name="widget">The feedback input to configure.</param>
    /// <param name="placeholder">The placeholder text to display when the input is empty.</param>
    /// <returns>The feedback input with the specified placeholder text.</returns>
    public static FeedbackInputBase Placeholder(this FeedbackInputBase widget, string placeholder) => widget with { Placeholder = placeholder };

    /// <summary>Sets the disabled state of the feedback input.</summary>
    /// <param name="widget">The feedback input to configure.</param>
    /// <param name="enabled">Whether the input should be disabled (note: parameter name suggests enabled but sets disabled state).</param>
    /// <returns>The feedback input with the specified disabled state.</returns>
    public static FeedbackInputBase Disabled(this FeedbackInputBase widget, bool enabled = true) => widget with { Disabled = enabled };

    /// <summary>Sets the visual variant of the feedback input.</summary>
    /// <param name="widget">The feedback input to configure.</param>
    /// <param name="variant">The visual variant (Stars, Thumbs, or Emojis).</param>
    /// <returns>The feedback input with the specified variant.</returns>
    public static FeedbackInputBase Variant(this FeedbackInputBase widget, FeedbackInputs variant) => widget with { Variant = variant };

    /// <summary>Sets the validation error message for the feedback input.</summary>
    /// <param name="widget">The feedback input to configure.</param>
    /// <param name="invalid">The validation error message to display.</param>
    /// <returns>The feedback input with the specified validation error.</returns>
    public static FeedbackInputBase Invalid(this FeedbackInputBase widget, string invalid) => widget with { Invalid = invalid };


    /// <summary>
    /// Sets the blur event handler for the feedback input.
    /// This method allows you to configure the feedback input's blur behavior,
    /// enabling it to perform custom actions when the input loses focus.
    /// </summary>
    /// <param name="widget">The feedback input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new feedback input instance with the updated blur handler.</returns>
    [OverloadResolutionPriority(1)]
    public static FeedbackInputBase HandleBlur(this FeedbackInputBase widget, Func<Event<IAnyInput>, ValueTask> onBlur)
    {
        return widget with { OnBlur = onBlur };
    }

    /// <summary>
    /// Sets the blur event handler for the feedback input.
    /// Compatibility overload for Action-based event handlers.
    /// </summary>
    /// <param name="widget">The feedback input to configure.</param>
    /// <param name="onBlur">The event handler to call when the input loses focus.</param>
    /// <returns>A new feedback input instance with the updated blur handler.</returns>
    public static FeedbackInputBase HandleBlur(this FeedbackInputBase widget, Action<Event<IAnyInput>> onBlur)
    {
        return widget.HandleBlur(onBlur.ToValueTask());
    }

    /// <summary>
    /// Sets a simple blur event handler for the feedback input.
    /// This method allows you to configure the feedback input's blur behavior with
    /// a simple action that doesn't require the input event context.
    /// </summary>
    /// <param name="widget">The feedback input to configure.</param>
    /// <param name="onBlur">The simple action to perform when the input loses focus.</param>
    /// <returns>A new feedback input instance with the updated blur handler.</returns>
    public static FeedbackInputBase HandleBlur(this FeedbackInputBase widget, Action onBlur)
    {
        return widget.HandleBlur(_ => { onBlur(); return ValueTask.CompletedTask; });
    }
}