using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents an expandable widget that creates collapsible content sections
/// that users can expand and collapse to maintain clean, organized layouts.
/// This widget provides an interactive way to hide and show content, helping
/// to reduce visual clutter while maintaining easy access to information.
/// 
/// The Expandable widget uses a slot-based architecture with separate header
/// and content areas, allowing for flexible content organization and styling.
/// The header remains visible at all times, while the content can be toggled
/// between expanded and collapsed states based on user interaction.
/// </summary>
public record Expandable : WidgetBase<Expandable>
{
    /// <summary>
    /// Initializes a new instance of the Expandable class with the specified
    /// header and content. The expandable widget will display the header
    /// prominently while allowing the content to be expanded or collapsed
    /// based on user interaction.
    /// </summary>
    /// <param name="header">The content to display in the header section.
    /// This content remains visible at all times and typically contains
    /// the title, summary, or trigger element for expanding/collapsing
    /// the content section.</param>
    /// <param name="content">The content that can be expanded or collapsed.
    /// This content is initially hidden and becomes visible when the user
    /// expands the section, allowing for clean information organization
    /// and reduced visual clutter.</param>
    public Expandable(object header, object content) : base([new Slot("Header", header), new Slot("Content", content)])
    {

    }

    /// <summary>
    /// Gets or sets whether the expandable widget is disabled and cannot
    /// be interacted with. This property controls the interactive state
    /// of the widget, preventing expansion/collapse when set to true.
    /// 
    /// When disabled, the expandable widget maintains its current state
    /// and users cannot toggle between expanded and collapsed views.
    /// This is useful for situations where the content should remain
    /// in a fixed state based on application logic or user permissions.
    /// Default is false (widget is enabled and interactive).
    /// </summary>
    [Prop] public bool Disabled { get; set; } = false;
}

/// <summary>
/// Provides extension methods for the Expandable widget that enable a fluent API
/// for configuring expandable behavior and appearance. These methods allow you
/// to easily set properties and configure the widget for optimal presentation
/// and functionality.
/// </summary>
public static class ExpandableExtensions
{
    /// <summary>
    /// Sets the disabled state of the expandable widget.
    /// This method allows you to control whether the widget can be
    /// interacted with, enabling you to lock the expandable in its
    /// current state based on application logic or user permissions.
    /// 
    /// When disabled, the expandable widget cannot be expanded or
    /// collapsed, maintaining its current state until re-enabled.
    /// </summary>
    /// <param name="widget">The Expandable widget to configure.</param>
    /// <param name="disabled">Whether the widget should be disabled (true) or enabled (false).</param>
    /// <returns>The Expandable instance for method chaining.</returns>
    public static Expandable Disabled(this Expandable widget, bool disabled)
    {
        widget.Disabled = disabled;
        return widget;
    }
}


