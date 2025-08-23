using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a slot widget that provides named content areas within parent widgets,
/// enabling flexible content organization and layout management. Slots act as
/// content placeholders that can be referenced by name, allowing parent widgets
/// to organize and style different sections of their content independently.
/// 
/// The Slot widget is a fundamental building block for creating complex layouts
/// with distinct content areas, such as headers, footers, sidebars, and main
/// content sections.
/// </summary>
public record Slot : WidgetBase<Slot>
{
    /// <summary>
    /// Gets or sets the name identifier for this slot.
    /// This property provides a unique identifier that parent widgets can use
    /// to reference and manage specific content areas within their layout.
    /// 
    /// Named slots enable parent widgets to organize content into distinct
    /// regions such as "Header", "Content", "Footer", "Sidebar", etc.,
    /// allowing for flexible layout management and content organization.
    /// When null, the slot is unnamed and may be treated as a general
    /// content container by the parent widget.
    /// Default is null (unnamed slot).
    /// </summary>
    [Prop] public string? Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the Slot class with the specified children
    /// and no name.
    /// </summary>
    /// <param name="children">Variable number of child elements to display
    /// within the slot. These children will be rendered as the slot's content
    /// and can include any widgets, layouts, or content elements.</param>
    public Slot(params object[] children) : this(null, children)
    {
    }

    /// <summary>
    /// Initializes a new instance of the Slot class with the specified name
    /// and children.
    /// </summary>
    /// <param name="name">The name identifier for this slot, used by parent
    /// widgets to reference and manage the slot's content area. Common names
    /// include "Header", "Content", "Footer", "Sidebar", "Main", etc.</param>
    /// <param name="children">Variable number of child elements to display
    /// within the slot. These children will be rendered as the slot's content
    /// and can include any widgets, layouts, or content elements.</param>
    public Slot(string? name, params object?[] children) : base(children!)
    {
        Name = name;
    }
}