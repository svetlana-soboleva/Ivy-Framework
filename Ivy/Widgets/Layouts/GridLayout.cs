using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines how grid items are automatically placed within the grid layout when no explicit
/// positioning is specified. Different auto-flow strategies provide various ways to fill
/// the grid space efficiently.
/// </summary>
public enum AutoFlow
{
    /// <summary>Fill each row completely before moving to the next row. This is the default behavior.</summary>
    Row,
    /// <summary>Fill each column completely before moving to the next column.</summary>
    Column,
    /// <summary>Fill rows sequentially, but try to fill gaps with later items for more efficient space usage.</summary>
    RowDense,
    /// <summary>Fill columns sequentially, but try to fill gaps with later items for more efficient space usage.</summary>
    ColumnDense
}

/// <summary>
/// Defines the configuration for a grid layout, including dimensions, spacing, and behavior.
/// This class provides all the properties needed to configure how a grid layout behaves
/// and how its children are positioned and spaced.
/// </summary>
public class GridDefinition
{
    /// <summary>
    /// Gets or sets the number of columns in the grid.
    /// When null, the grid will automatically determine the number of columns based on content.
    /// When specified, the grid will maintain exactly this many columns regardless of content.
    /// Default is null (auto-determined).
    /// </summary>
    public int? Columns { get; set; }

    /// <summary>
    /// Gets or sets the number of rows in the grid.
    /// When null, the grid will automatically determine the number of rows based on content.
    /// When specified, the grid will maintain exactly this many rows regardless of content.
    /// Default is null (auto-determined).
    /// </summary>
    public int? Rows { get; set; }

    /// <summary>
    /// Gets or sets the space between grid items in pixels.
    /// This creates uniform spacing between all grid cells, both horizontally and vertically.
    /// Default is 4 pixels.
    /// </summary>
    public int Gap { get; set; } = 4;

    /// <summary>
    /// Gets or sets the padding around the entire grid container in pixels.
    /// This creates space between the grid content and the grid's outer boundaries.
    /// Default is 0 pixels (no padding).
    /// </summary>
    public int Padding { get; set; } = 0;

    /// <summary>
    /// Gets or sets how grid items are automatically placed when no explicit positioning is specified.
    /// This controls the flow direction and density of item placement within the grid.
    /// Default is null (uses <see cref="AutoFlow.Row"/>).
    /// </summary>
    public AutoFlow? AutoFlow { get; set; } = null;

    /// <summary>
    /// Gets or sets the width of the grid container.
    /// When null, the grid will size itself based on its content and available space.
    /// When specified, the grid will maintain exactly this width.
    /// Default is null (auto-sized).
    /// </summary>
    public Size? Width { get; set; } = null;

    /// <summary>
    /// Gets or sets the height of the grid container.
    /// When null, the grid will size itself based on its content and available space.
    /// When specified, the grid will maintain exactly this height.
    /// Default is null (auto-sized).
    /// </summary>
    public Size? Height { get; set; } = null;
}

/// <summary>
/// Represents a two-dimensional grid layout widget that arranges child elements in rows and columns
/// with precise control over positioning, spacing, and spanning. This widget provides both automatic
/// flow and explicit positioning for flexible grid layouts.
/// 
/// The GridLayout widget supports complex grid arrangements including spanning across multiple cells,
/// custom positioning, and various auto-flow strategies for efficient space utilization.
/// </summary>
public record GridLayout : WidgetBase<GridLayout>
{
    /// <summary>
    /// Initializes a new instance of the GridLayout class with the specified grid definition and children.
    /// The grid will be configured according to the GridDefinition settings, and children will be
    /// positioned according to the grid's auto-flow behavior or explicit positioning.
    /// </summary>
    /// <param name="def">The GridDefinition object containing all grid configuration settings including
    /// columns, rows, gap, padding, auto-flow, and dimensions.</param>
    /// <param name="children">Variable number of child elements to arrange in the grid layout.</param>
    public GridLayout(GridDefinition def, params object[] children) : base(children)
    {
        Columns = def.Columns;
        Rows = def.Rows;
        Gap = def.Gap;
        Padding = def.Padding;
        AutoFlow = def.AutoFlow;
        Width = def.Width;
        Height = def.Height;
    }

    /// <summary>
    /// Gets or sets the number of columns in the grid.
    /// When null, the grid will automatically determine the number of columns based on content.
    /// When specified, the grid will maintain exactly this many columns regardless of content.
    /// Default is the value from the GridDefinition constructor.
    /// </summary>
    [Prop] public int? Columns { get; set; }

    /// <summary>
    /// Gets or sets the number of rows in the grid.
    /// When null, the grid will automatically determine the number of rows based on content.
    /// When specified, the grid will maintain exactly this many rows regardless of content.
    /// Default is the value from the GridDefinition constructor.
    /// </summary>
    [Prop] public int? Rows { get; set; }

    /// <summary>
    /// Gets or sets the space between grid items in pixels.
    /// This creates uniform spacing between all grid cells, both horizontally and vertically.
    /// Default is the value from the GridDefinition constructor.
    /// </summary>
    [Prop] public int Gap { get; set; }

    /// <summary>
    /// Gets or sets the padding around the entire grid container in pixels.
    /// This creates space between the grid content and the grid's outer boundaries.
    /// Default is the value from the GridDefinition constructor.
    /// </summary>
    [Prop] public int Padding { get; set; }

    /// <summary>
    /// Gets or sets how grid items are automatically placed when no explicit positioning is specified.
    /// This controls the flow direction and density of item placement within the grid.
    /// Default is the value from the GridDefinition constructor.
    /// </summary>
    [Prop] public AutoFlow? AutoFlow { get; set; }

    /// <summary>
    /// Gets or sets the column positions for child elements in the grid.
    /// This attached property array maps child indices to their specific column positions,
    /// allowing precise control over where each child is placed in the grid.
    /// Default is null (children use auto-flow positioning).
    /// </summary>
    [Prop(attached: nameof(GridExtensions.GridColumn))] public int?[] ChildColumn { get; set; } = null!;

    /// <summary>
    /// Gets or sets the column span values for child elements in the grid.
    /// This attached property array maps child indices to how many columns they should span,
    /// allowing children to extend across multiple grid columns.
    /// Default is null (children span single columns).
    /// </summary>
    [Prop(attached: nameof(GridExtensions.GridColumnSpan))] public int?[] ChildColumnSpan { get; set; } = null!;

    /// <summary>
    /// Gets or sets the row positions for child elements in the grid.
    /// This attached property array maps child indices to their specific row positions,
    /// allowing precise control over where each child is placed in the grid.
    /// Default is null (children use auto-flow positioning).
    /// </summary>
    [Prop(attached: nameof(GridExtensions.GridRow))] public int?[] ChildRow { get; set; } = null!;

    /// <summary>
    /// Gets or sets the row span values for child elements in the grid.
    /// This attached property array maps child indices to how many rows they should span,
    /// allowing children to extend across multiple grid rows.
    /// Default is null (children span single rows).
    /// </summary>
    [Prop(attached: nameof(GridExtensions.GridRowSpan))] public int?[] ChildRowSpan { get; set; } = null!;
}

/// <summary>
/// Provides extension methods for positioning child elements within grid layouts.
/// These methods allow you to precisely control where each child is placed in the grid
/// and how many grid cells they should span, enabling complex grid arrangements.
/// </summary>
public static class GridExtensions
{
    /// <summary>
    /// Sets the specific column position for a child element in the grid.
    /// This method overrides the auto-flow behavior and places the child at the exact
    /// column position specified, allowing for precise grid layouts.
    /// </summary>
    /// <typeparam name="T">The type of the widget being positioned.</typeparam>
    /// <param name="child">The child widget to position in the grid.</param>
    /// <param name="column">The column index where the child should be placed (1-based indexing).</param>
    /// <returns>The child widget with the grid column position set.</returns>
    public static WidgetBase<T> GridColumn<T>(this WidgetBase<T> child, int column) where T : WidgetBase<T>
    {
        child.SetAttachedValue(typeof(GridLayout), nameof(GridColumn), column);
        return child;
    }

    /// <summary>
    /// Sets how many columns a child element should span in the grid.
    /// This method allows children to extend across multiple grid columns, creating
    /// wider elements that span multiple cells horizontally.
    /// </summary>
    /// <typeparam name="T">The type of the widget being positioned.</typeparam>
    /// <param name="child">The child widget to configure for column spanning.</param>
    /// <param name="columnSpan">The number of columns the child should span across.</param>
    /// <returns>The child widget with the grid column span set.</returns>
    public static WidgetBase<T> GridColumnSpan<T>(this WidgetBase<T> child, int columnSpan) where T : WidgetBase<T>
    {
        child.SetAttachedValue(typeof(GridLayout), nameof(GridColumnSpan), columnSpan);
        return child;
    }

    /// <summary>
    /// Sets the specific row position for a child element in the grid.
    /// This method overrides the auto-flow behavior and places the child at the exact
    /// row position specified, allowing for precise grid layouts.
    /// </summary>
    /// <typeparam name="T">The type of the widget being positioned.</typeparam>
    /// <param name="child">The child widget to position in the grid.</param>
    /// <param name="row">The row index where the child should be placed (1-based indexing).</param>
    /// <returns>The child widget with the grid row position set.</returns>
    public static WidgetBase<T> GridRow<T>(this WidgetBase<T> child, int row) where T : WidgetBase<T>
    {
        child.SetAttachedValue(typeof(GridLayout), nameof(GridRow), row);
        return child;
    }

    /// <summary>
    /// Sets how many rows a child element should span in the grid.
    /// This method allows children to extend across multiple grid rows, creating
    /// taller elements that span multiple cells vertically.
    /// </summary>
    /// <typeparam name="T">The type of the widget being positioned.</typeparam>
    /// <param name="child">The child widget to configure for row spanning.</param>
    /// <param name="rowSpan">The number of rows the child should span across.</param>
    /// <returns>The child widget with the grid row span set.</returns>
    public static WidgetBase<T> GridRowSpan<T>(this WidgetBase<T> child, int rowSpan) where T : WidgetBase<T>
    {
        child.SetAttachedValue(typeof(GridLayout), nameof(GridRowSpan), rowSpan);
        return child;
    }
}