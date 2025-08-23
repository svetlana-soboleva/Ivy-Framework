using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a floating panel widget that positions its content at a fixed location on the screen,
/// making it ideal for elements that should remain accessible while users scroll through content.
/// Floating panels are commonly used for navigation buttons, action panels, floating controls,
/// and other UI elements that need to stay visible regardless of scroll position.
/// 
/// The FloatingPanel widget automatically uses a high z-index to ensure it appears above other
/// content, and supports nine different alignment positions for precise placement control.
/// </summary>
public record FloatingPanel : WidgetBase<FloatingPanel>
{
    /// <summary>
    /// Initializes a new instance of the FloatingPanel class with the specified child content and alignment.
    /// The panel will be positioned according to the alignment setting, with the child content displayed
    /// at the specified screen position.
    /// </summary>
    /// <param name="child">The content to display in the floating panel. Can be null for an empty panel.</param>
    /// <param name="align">The alignment position for the floating panel on the screen. Default is <see cref="Align.BottomRight"/>.</param>
    public FloatingPanel(object? child = null, Align align = Align.BottomRight) : base(child != null ? [child] : [])
    {
        Align = align;
    }

    /// <summary>
    /// Gets or sets the alignment position of the floating panel on the screen.
    /// This property controls where the panel is positioned relative to the viewport, supporting
    /// nine different positions including corners, edges, and center.
    /// 
    /// Available positions include: TopLeft, TopCenter, TopRight, Left, Center, Right, BottomLeft,
    /// BottomCenter, and BottomRight. The panel will be anchored to the specified position.
    /// Default is <see cref="Align.BottomRight"/>.
    /// </summary>
    [Prop] public Align Align { get; set; }

    /// <summary>
    /// Gets or sets the offset adjustment for the floating panel's position.
    /// This property allows fine-tuning of the panel's position by applying additional spacing
    /// from the default alignment position. The offset is applied using a <see cref="Thickness"/>
    /// object that specifies left, top, right, and bottom offset values.
    /// 
    /// When null, no offset is applied and the panel uses the exact alignment position.
    /// When specified, the offset values are added to the base alignment position for precise positioning.
    /// Default is null (no offset applied).
    /// </summary>
    [Prop] public Thickness? Offset { get; set; }

    /// <summary>
    /// Operator overload that allows adding a single child to the floating panel using the pipe operator.
    /// This provides a convenient syntax for adding content to floating panels in a readable manner.
    /// 
    /// Note: This operator only supports single children. Multiple children are not supported
    /// and will throw a <see cref="NotSupportedException"/> if attempted.
    /// </summary>
    /// <param name="widget">The floating panel to add content to.</param>
    /// <param name="child">The single child content to add to the panel.</param>
    /// <returns>A new FloatingPanel instance with the child content added.</returns>
    /// <exception cref="NotSupportedException">Thrown when attempting to add multiple children using IEnumerable.</exception>
    public static FloatingPanel operator |(FloatingPanel widget, object child)
    {
        if (child is IEnumerable<object> _)
        {
            throw new NotSupportedException("FloatingLayer does not support multiple children.");
        }

        return widget with { Children = [child] };
    }
}

/// <summary>
/// Provides extension methods for the FloatingPanel widget that enable a fluent API for configuring
/// panel positioning and alignment. These methods allow you to easily set alignment positions,
/// apply offset adjustments, and use convenience methods for common offset scenarios.
/// </summary>
public static class FloatingLayerExtensions
{
    /// <summary>
    /// Sets the alignment position of the floating panel on the screen.
    /// This method allows you to change the panel's position after creation, supporting all
    /// nine alignment positions for flexible panel positioning.
    /// </summary>
    /// <param name="floatingButton">The floating panel to configure.</param>
    /// <param name="align">The new alignment position for the panel.</param>
    /// <returns>A new FloatingPanel instance with the updated alignment position.</returns>
    public static FloatingPanel Align(this FloatingPanel floatingButton, Align align) => floatingButton with { Align = align };

    /// <summary>
    /// Sets the offset adjustment for the floating panel's position using a Thickness object.
    /// This method allows fine-grained control over the panel's position by specifying
    /// left, top, right, and bottom offset values for precise positioning adjustments.
    /// </summary>
    /// <param name="floatingButton">The floating panel to configure.</param>
    /// <param name="offset">The Thickness object containing left, top, right, and bottom offset values.</param>
    /// <returns>A new FloatingPanel instance with the updated offset positioning.</returns>
    public static FloatingPanel Offset(this FloatingPanel floatingButton, Thickness? offset) => floatingButton with { Offset = offset };

    /// <summary>
    /// Applies a left offset to the floating panel's position.
    /// This convenience method moves the panel to the right by the specified number of units
    /// from its base alignment position, useful for fine-tuning horizontal positioning.
    /// </summary>
    /// <param name="floatingButton">The floating panel to configure.</param>
    /// <param name="offset">The number of units to offset the panel to the right from its base position.</param>
    /// <returns>A new FloatingPanel instance with the left offset applied.</returns>
    public static FloatingPanel OffsetLeft(this FloatingPanel floatingButton, int offset) => floatingButton with { Offset = new Thickness(offset, 0, 0, 0) };

    /// <summary>
    /// Applies a top offset to the floating panel's position.
    /// This convenience method moves the panel down by the specified number of units
    /// from its base alignment position, useful for fine-tuning vertical positioning.
    /// </summary>
    /// <param name="floatingButton">The floating panel to configure.</param>
    /// <param name="offset">The number of units to offset the panel down from its base position.</param>
    /// <returns>A new FloatingPanel instance with the top offset applied.</returns>
    public static FloatingPanel OffsetTop(this FloatingPanel floatingButton, int offset) => floatingButton with { Offset = new Thickness(0, offset, 0, 0) };

    /// <summary>
    /// Applies a right offset to the floating panel's position.
    /// This convenience method moves the panel to the left by the specified number of units
    /// from its base alignment position, useful for fine-tuning horizontal positioning.
    /// </summary>
    /// <param name="floatingButton">The floating panel to configure.</param>
    /// <param name="offset">The number of units to offset the panel to the left from its base position.</param>
    /// <returns>A new FloatingPanel instance with the right offset applied.</returns>
    public static FloatingPanel OffsetRight(this FloatingPanel floatingButton, int offset) => floatingButton with { Offset = new Thickness(0, 0, offset, 0) };

    /// <summary>
    /// Applies a bottom offset to the floating panel's position.
    /// This convenience method moves the panel up by the specified number of units
    /// from its base alignment position, useful for fine-tuning vertical positioning.
    /// </summary>
    /// <param name="floatingButton">The floating panel to configure.</param>
    /// <param name="offset">The number of units to offset the panel up from its base position.</param>
    /// <returns>A new FloatingPanel instance with the bottom offset applied.</returns>
    public static FloatingPanel OffsetBottom(this FloatingPanel floatingButton, int offset) => floatingButton with { Offset = new Thickness(0, 0, 0, offset) };
}