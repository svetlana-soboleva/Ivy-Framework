using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a table row widget that contains a horizontal collection of table cells
/// within a table structure. This widget provides the intermediate organizational layer
/// between individual cells and the complete table, allowing you to group related
/// data or header information into logical rows.
/// 
/// The TableRow widget supports different row types (header, data) and provides
/// flexible cell management through the pipe operator for incremental cell addition.
/// It integrates seamlessly with <see cref="TableCell"/> and <see cref="Table"/>
/// components to create comprehensive table structures for data presentation.
/// </summary>
public record TableRow : WidgetBase<TableRow>
{
    /// <summary>
    /// Initializes a new instance of the TableRow class with the specified table cells.
    /// The row will display the provided cells horizontally in the order they are
    /// specified, creating a structured horizontal line within the table layout.
    /// </summary>
    /// <param name="cells">Variable number of TableCell elements that define the row's
    /// content and structure. Each cell represents a column within the row, containing
    /// data, header information, or other content elements.</param>
    public TableRow(params TableCell[] cells) : base(cells.Cast<object>().ToArray())
    {
    }

    /// <summary>
    /// Gets or sets whether this row should be treated as a header row.
    /// When true, the row is semantically marked as a header and typically receives
    /// special styling such as bold text, background colors, or other visual emphasis
    /// to distinguish it from regular data rows.
    /// 
    /// Header rows are commonly used at the top of a table to provide column labels
    /// or on the left side to provide row labels, creating clear structure and
    /// improving table readability and navigation.
    /// Default is false (regular data row).
    /// </summary>
    [Prop] public bool IsHeader { get; set; }

    /// <summary>
    /// Operator overload that allows adding a single TableCell to the row using the pipe operator.
    /// This provides a convenient syntax for building table rows incrementally by adding
    /// cells one at a time in a readable, chainable manner.
    /// 
    /// The operator creates a new TableRow instance with the additional cell appended to
    /// the existing children, maintaining immutability while providing an intuitive API
    /// for row construction and modification.
    /// </summary>
    /// <param name="row">The TableRow to add the cell to.</param>
    /// <param name="child">The TableCell to add to the row.</param>
    /// <returns>A new TableRow instance with the additional cell appended to the existing cells.</returns>
    public static TableRow operator |(TableRow row, TableCell child)
    {
        return row with { Children = [.. row.Children, child] };
    }
}

/// <summary>
/// Provides extension methods for the TableRow widget that enable a fluent API for
/// configuring row properties and styling. These methods allow you to easily set
/// header status for optimal table appearance and semantic structure.
/// </summary>
public static class TableRowExtensions
{
    /// <summary>
    /// Sets whether the table row should be treated as a header row.
    /// This method allows you to configure the row's semantic role after creation,
    /// enabling or disabling header styling and behavior as needed.
    /// </summary>
    /// <param name="row">The TableRow to configure.</param>
    /// <param name="isHeader">Whether the row should be treated as a header. Default is true.</param>
    /// <returns>A new TableRow instance with the updated header status.</returns>
    public static TableRow IsHeader(this TableRow row, bool isHeader = true)
    {
        return row with { IsHeader = isHeader };
    }
}