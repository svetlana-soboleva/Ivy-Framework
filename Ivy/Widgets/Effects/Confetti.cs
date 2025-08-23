using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Widget that creates a celebratory confetti effect around its child content. Confetti provides
/// a festive visual effect with animated particles that fall or burst from the widget area,
/// perfect for celebrations, achievements, successful actions, and positive user feedback.
/// </summary>
public record Confetti : WidgetBase<Confetti>
{
    /// <summary>
    /// Initializes a new instance of the Confetti class with optional child content.
    /// The confetti effect will be displayed around or over the child content when triggered.
    /// </summary>
    /// <param name="child">
    /// The optional child widget or content to display with the confetti effect.
    /// If null, the confetti effect will be displayed without any underlying content.
    /// The child content remains interactive while the confetti animation plays.
    /// </param>
    public Confetti(object? child = null) : base(child != null ? [child] : [])
    {
    }

    /// <summary>
    /// Gets or sets when the confetti effect should be triggered.
    /// Controls the user interaction or automatic behavior that starts the confetti animation.
    /// </summary>
    /// <value>
    /// The trigger condition for the confetti effect. Default is Auto, which starts the effect immediately.
    /// Use Click for user-initiated celebrations or Hover for interactive feedback.
    /// </value>
    [Prop] public AnimationTrigger Trigger { get; init; } = AnimationTrigger.Auto;

    /// <summary>
    /// Overrides the | operator to add a single child to the Confetti widget.
    /// Confetti widgets support only one child to maintain the focus of the celebratory effect.
    /// </summary>
    /// <param name="widget">The Confetti widget.</param>
    /// <param name="child">The single child widget or content to add.</param>
    /// <returns>A new Confetti widget with the specified child.</returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when attempting to add multiple children. Confetti widgets are designed to wrap
    /// a single piece of content to maintain the visual focus of the celebration effect.
    /// </exception>
    public static Confetti operator |(Confetti widget, object child)
    {
        if (child is IEnumerable<object> _)
        {
            throw new NotSupportedException("Confetti does not support multiple children.");
        }

        return widget with { Children = [child] };
    }
}

/// <summary>
/// Provides extension methods for creating and configuring Confetti widgets with fluent syntax.
/// These methods enable easy application of confetti effects to widgets and views for celebratory interactions.
/// </summary>
public static class ConfettiExtensions
{
    /// <summary>
    /// Wraps a widget with a confetti effect, creating a celebratory animation around the content.
    /// </summary>
    /// <param name="widget">The widget to wrap with confetti effect.</param>
    /// <param name="trigger">The trigger condition for the confetti effect. Default is Auto.</param>
    /// <returns>A Confetti widget containing the specified widget with the celebratory effect.</returns>
    public static Confetti WithConfetti(this IWidget widget, AnimationTrigger trigger = AnimationTrigger.Auto)
    {
        return new Confetti(widget).Trigger(trigger);
    }

    /// <summary>
    /// Wraps a view with a confetti effect, creating a celebratory animation around the content.
    /// </summary>
    /// <param name="view">The view to wrap with confetti effect.</param>
    /// <param name="trigger">The trigger condition for the confetti effect. Default is Auto.</param>
    /// <returns>A Confetti widget containing the specified view with the celebratory effect.</returns>
    public static Confetti WithConfetti(this IView view, AnimationTrigger trigger = AnimationTrigger.Auto)
    {
        return new Confetti(view).Trigger(trigger);
    }

    /// <summary>
    /// Sets the trigger condition for when the confetti effect should activate.
    /// </summary>
    /// <param name="confetti">The confetti widget to configure.</param>
    /// <param name="trigger">The trigger condition (Auto, Click, or Hover).</param>
    /// <returns>The confetti widget with the specified trigger condition.</returns>
    public static Confetti Trigger(this Confetti confetti, AnimationTrigger trigger)
    {
        return confetti with { Trigger = trigger };
    }
}


