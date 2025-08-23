using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents the header section of a dialog widget that displays the dialog title and provides visual hierarchy.
/// The DialogHeader establishes the context and purpose of the dialog through its title text, and may include
/// close controls or other header-specific elements. This section forms the top part of the structured
/// three-part dialog layout alongside DialogBody and DialogFooter.
/// </summary>
public record DialogHeader : WidgetBase<DialogHeader>
{
    /// <summary>
    /// Initializes a new instance of the DialogHeader class with the specified title text.
    /// The header provides immediate context about the dialog's purpose and establishes
    /// the visual hierarchy for the modal interaction.
    /// </summary>
    /// <param name="title">
    /// The title text to display in the dialog header. This should be a concise, descriptive
    /// label that clearly communicates the dialog's purpose to users. Examples include
    /// "Confirm Delete", "Edit Profile", "Settings", "About", or operation-specific titles
    /// like "Export Data" or "Create New Project". The title helps users understand the
    /// context and expected interaction within the dialog.
    /// </param>
    public DialogHeader(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Gets or sets the title text displayed in the dialog header.
    /// The title provides users with immediate context about the dialog's purpose and function,
    /// establishing clear expectations for the interaction that follows.
    /// </summary>
    /// <value>
    /// A string containing the dialog title. Should be concise and descriptive, clearly
    /// communicating the dialog's purpose such as "Confirm Action", "Edit Settings", or
    /// operation-specific titles that help users understand the context.
    /// </value>
    [Prop]
    public string Title { get; set; }

    /// <summary>
    /// Overrides the | operator to prevent adding children to DialogHeader widgets.
    /// DialogHeader widgets are designed to display only the title text and maintain a clean,
    /// consistent header appearance across all dialogs in the application.
    /// </summary>
    /// <param name="widget">The DialogHeader widget.</param>
    /// <param name="child">The child object attempting to be added.</param>
    /// <returns>This method always throws an exception.</returns>
    /// <exception cref="NotSupportedException">
    /// Always thrown because DialogHeader widgets only display title text.
    /// Use the Title property to set the header content instead of adding child widgets.
    /// </exception>
    public static DialogHeader operator |(DialogHeader widget, object child)
    {
        throw new NotSupportedException("DialogHeader does not support children.");
    }
}