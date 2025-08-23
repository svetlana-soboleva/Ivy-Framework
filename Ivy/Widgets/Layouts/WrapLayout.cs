using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a wrap layout widget that arranges child elements in rows that automatically
/// wrap to the next line when they reach the end of the available horizontal space.
/// 
/// The WrapLayout widget provides flexible, responsive behavior where content automatically
/// flows to new lines based on available space. It supports comprehensive
/// styling options including gaps, padding, margins, background colors, and alignment control.
/// </summary>
public record WrapLayout : WidgetBase<WrapLayout>
{
    /// <summary>
    /// Initializes a new instance of the WrapLayout class with the specified children and
    /// configuration options. The layout will automatically arrange children in rows with
    /// wrapping behavior based on available horizontal space.
    /// </summary>
    /// <param name="children">Array of child elements to arrange in the wrap layout. These elements
    /// will be positioned sequentially and automatically wrap to new lines when horizontal space
    /// is exhausted.</param>
    /// <param name="gap">The space between child elements in pixels. This creates uniform spacing
    /// between all children, both horizontally and vertically, providing visual separation
    /// and breathing room. Default is 4 pixels.</param>
    /// <param name="padding">Optional padding around the entire wrap layout container. When null,
    /// no padding is applied. When specified using a <see cref="Thickness"/> object, creates
    /// space between the layout content and its outer boundaries.</param>
    /// <param name="margin">Optional margin around the entire wrap layout container. When null,
    /// no margin is applied. When specified using a <see cref="Thickness"/> object, creates
    /// space between the layout and surrounding elements.</param>
    /// <param name="background">Optional background color for the wrap layout container. When null,
    /// no background color is applied. When specified, fills the entire layout area with the
    /// selected color for visual distinction.</param>
    /// <param name="alignment">Optional alignment for child elements within the wrap layout. When null,
    /// uses default alignment behavior. When specified, controls how children are positioned
    /// relative to the available space in each row.</param>
    /// <param name="removeParentPadding">Whether to remove any padding inherited from parent containers.
    /// When true, the wrap layout will ignore parent padding and extend to the full available
    /// space, allowing it to break out of parent container boundaries.</param>
    public WrapLayout(object[] children, int gap = 4, Thickness? padding = null, Thickness? margin = null,
        Colors? background = null, Align? alignment = null, bool removeParentPadding = false) : base(children)
    {
        Gap = gap;
        Padding = padding;
        Margin = margin;
        Background = background;
        Alignment = alignment;
        RemoveParentPadding = removeParentPadding;
    }

    /// <summary>
    /// Gets or sets the space between child elements in pixels.
    /// This property creates uniform spacing between all children in the wrap layout,
    /// both horizontally between elements in the same row and vertically between rows.
    /// 
    /// The gap provides visual separation and breathing room between elements, creating
    /// a clean, organized appearance regardless of the number of children or their sizes.
    /// Default is 4 pixels.
    /// </summary>
    [Prop] public int Gap { get; set; }

    /// <summary>
    /// Gets or sets the padding around the entire wrap layout container.
    /// This property controls the internal spacing between the layout's content and its
    /// outer boundaries, creating visual separation and preventing content from touching
    /// the edges of the container.
    /// 
    /// When null, no padding is applied. When specified using a <see cref="Thickness"/>
    /// object, you can control padding on different sides (left, top, right, bottom)
    /// independently for precise layout control.
    /// Default is null (no padding).
    /// </summary>
    [Prop] public Thickness? Padding { get; set; }

    /// <summary>
    /// Gets or sets the margin around the entire wrap layout container.
    /// This property controls the external spacing between the layout and surrounding
    /// elements, creating visual separation and preventing the layout from touching
    /// adjacent components.
    /// 
    /// When null, no margin is applied. When specified using a <see cref="Thickness"/>
    /// object, you can control margin on different sides (left, top, right, bottom)
    /// independently for precise positioning relative to other elements.
    /// Default is null (no margin).
    /// </summary>
    [Prop] public Thickness? Margin { get; set; }

    /// <summary>
    /// Gets or sets the background color for the entire wrap layout container.
    /// This property fills the entire layout area with the specified color, providing
    /// visual distinction and background context for the layout's content.
    /// 
    /// When null, no background color is applied, allowing the underlying background
    /// to show through. When specified, the entire layout area is filled with the
    /// selected color, creating a solid background for all child elements.
    /// Default is null (no background color).
    /// </summary>
    [Prop] public Colors? Background { get; set; }

    /// <summary>
    /// Gets or sets the alignment for child elements within the wrap layout.
    /// This property controls how children are positioned relative to the available
    /// space in each row, allowing for precise control over element positioning
    /// and visual balance within the layout.
    /// 
    /// When null, uses default alignment behavior. When specified, controls whether
    /// children are aligned to the start, center, or end of each row, or distributed
    /// across the available space for optimal visual appearance.
    /// Default is null (default alignment behavior).
    /// </summary>
    [Prop] public Align? Alignment { get; set; }

    /// <summary>
    /// Gets or sets whether to remove any padding inherited from parent containers.
    /// When true, the wrap layout will ignore parent padding and extend to the full
    /// available space, allowing it to break out of parent container boundaries.
    /// 
    /// This property is useful for creating full-width or full-height layouts that
    /// need to extend beyond their parent's padding constraints, such as navigation
    /// bars, full-screen sections, or edge-to-edge content areas.
    /// Default is false (parent padding is respected).
    /// </summary>
    [Prop] public bool RemoveParentPadding { get; set; }
}