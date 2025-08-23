using Ivy.Core;

namespace Ivy.Widgets.Internal;

/// <summary>
/// Represents a DBML (Database Markup Language) canvas widget that renders database schema diagrams
/// from DBML code. This widget parses DBML syntax and displays tables, fields, relationships, and
/// database structures as an interactive visual diagram using ReactFlow for layout and visualization.
/// This is an internal widget used by the Ivy Framework for database schema visualization.
/// </summary>
public record DbmlCanvas : WidgetBase<DbmlCanvas>
{
    /// <summary>
    /// Initializes a new instance of the DbmlCanvas class with the specified DBML code.
    /// </summary>
    /// <param name="dbml">The DBML code string to parse and visualize. Can be null for an empty canvas.</param>
    public DbmlCanvas(string? dbml)
    {
        Dbml = dbml;
    }

    /// <summary>
    /// Gets or sets the DBML code string that defines the database schema to visualize.
    /// This property contains the Database Markup Language syntax that describes tables,
    /// fields, relationships, and database structure. When set, the widget automatically
    /// parses the DBML and renders the corresponding visual diagram.
    /// 
    /// The DBML can include:
    /// - Table definitions with field specifications
    /// - Data types (int, varchar, boolean, text, datetime, uuid)
    /// - Primary key and constraint definitions
    /// - Foreign key relationships and references
    /// - Table groups and project definitions
    /// - Notes and documentation
    /// 
    /// Default is null (empty canvas with no schema).
    /// </summary>
    [Prop] public string? Dbml { get; set; }
}
