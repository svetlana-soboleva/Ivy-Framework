using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a table widget that displays structured data in a clean, organized tabular format.
/// This widget is designed to render data in rows and columns, making it suitable for structured
/// display of content like data listings, reports, grids, and any information that benefits from
/// organized tabular presentation.
/// 
/// The Table widget accepts rows composed of <see cref="TableRow"/> elements, providing a flexible
/// foundation for creating both simple and complex data tables. It supports the pipe operator for
/// easy row addition and integrates seamlessly with the Ivy Framework's table building system
/// through extension methods and automatic conversion from collections.
/// </summary>
public record Table : WidgetBase<Table>
{
    /// <summary>
    /// Initializes a new instance of the Table class with the specified table rows.
    /// The table will display the provided rows in the order they are specified, creating
    /// a structured tabular layout for data presentation.
    /// </summary>
    /// <param name="rows">Variable number of TableRow elements that define the table structure
    /// and content. Each row represents a horizontal line in the table containing cells
    /// with data or header information.</param>
    public Table(params TableRow[] rows) : base(rows.Cast<object>().ToArray())
    {
    }

    /// <summary>
    /// Operator overload that allows adding a single TableRow to the table using the pipe operator.
    /// This provides a convenient syntax for building tables incrementally by adding rows
    /// one at a time in a readable, chainable manner.
    /// 
    /// The operator creates a new Table instance with the additional row appended to the
    /// existing children, maintaining immutability while providing an intuitive API for
    /// table construction.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="child">The TableRow to add to the table.</param>
    /// <returns>A new Table instance with the additional row appended to the existing rows.</returns>
    public static Table operator |(Table table, TableRow child)
    {
        return table with { Children = [.. table.Children, child] };
    }
}