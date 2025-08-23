using System.Xml.Linq;
using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// A widget for displaying XML data with syntax highlighting, collapsible nodes, and interactive navigation.
/// </summary>
/// <remarks>
/// Renders XML content with color-coded syntax highlighting and expandable/collapsible tree structure.
/// Perfect for configuration files, API responses, data feeds, and other XML-structured content.
/// </remarks>
public record Xml : WidgetBase<Xml>
{
    /// <summary>
    /// Initializes a new XML widget from an XObject with automatic string conversion.
    /// </summary>
    /// <param name="xml">The XObject (XElement, XDocument, etc.) to display as formatted XML.</param>
    public Xml(XObject xml) : this(xml.ToString() ?? string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new XML widget with the specified XML content string.
    /// </summary>
    /// <param name="content">The XML content string to display with syntax highlighting and interactive features.</param>
    public Xml(string content)
    {
        Content = content;
    }

    /// <summary>Gets or sets the XML content to display.</summary>
    /// <value>The XML content string that will be rendered with syntax highlighting and collapsible tree structure.</value>
    [Prop] public string Content { get; set; }
}