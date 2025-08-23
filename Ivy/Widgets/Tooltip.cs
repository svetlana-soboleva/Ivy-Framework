using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a tooltip widget that provides contextual information when hovering
/// or focusing on a trigger element. 
/// </summary>
public record Tooltip : WidgetBase<Tooltip>
{
    /// <summary>
    /// Initializes a new instance of the Tooltip class with the specified
    /// trigger element and tooltip content.
    /// </summary>
    /// <param name="trigger">The element that triggers the tooltip display
    /// when hovered or focused.</param>
    /// <param name="content">The content to display in the tooltip, typically
    /// explanatory text or helpful information.</param>
    public Tooltip(object trigger, object content) : base([new Slot("Trigger", trigger), new Slot("Content", content)])
    {
    }

    /// <summary>
    /// Operator overload that prevents adding children to the Tooltip using the pipe operator.
    /// </summary>
    /// <param name="widget">The Tooltip widget.</param>
    /// <param name="child">The child content to add (not supported).</param>
    /// <returns>This method always throws a NotSupportedException.</returns>
    /// <exception cref="NotSupportedException">Always thrown, as Tooltip does not support additional children.</exception>
    public static Tooltip operator |(Tooltip widget, object child)
    {
        throw new NotSupportedException("Tooltip does not support children.");
    }
}

/// <summary>
/// Provides extension methods for adding tooltips to widgets and views,
/// enabling a fluent API for tooltip integration.
/// </summary>
public static class TooltipExtensions
{
    /// <summary>
    /// Adds a tooltip to a widget that displays the specified text when
    /// the widget is hovered or focused.
    /// </summary>
    /// <param name="widget">The widget to add the tooltip to.</param>
    /// <param name="toolTip">The tooltip text to display.</param>
    /// <returns>A new Tooltip instance with the widget as trigger and the text as content.</returns>
    public static IWidget WithTooltip(this IWidget widget, string toolTip)
    {
        return new Tooltip(widget, toolTip);
    }

    /// <summary>
    /// Adds a tooltip to a view that displays the specified text when
    /// the view is hovered or focused.
    /// </summary>
    /// <param name="view">The view to add the tooltip to.</param>
    /// <param name="toolTip">The tooltip text to display.</param>
    /// <returns>A new Tooltip instance with the view as trigger and the text as content.</returns>
    public static IWidget WithTooltip(this IView view, string toolTip)
    {
        return new Tooltip(view, toolTip);
    }
}