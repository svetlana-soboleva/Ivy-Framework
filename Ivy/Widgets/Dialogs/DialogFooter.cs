using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents the footer section of a dialog widget that typically contains action buttons and controls.
/// The DialogFooter provides a consistent location for user actions such as confirmation, cancellation,
/// submission, and other operations related to the dialog's purpose. This section completes the
/// structured three-part dialog layout alongside DialogHeader and DialogBody.
/// </summary>
public record DialogFooter : WidgetBase<DialogFooter>
{
    /// <summary>
    /// Initializes a new instance of the DialogFooter class with the specified child controls.
    /// The DialogFooter serves as a container for action elements that allow users to respond
    /// to or interact with the dialog's content.
    /// </summary>
    /// <param name="children">
    /// The child widgets to be displayed within the dialog footer. This typically includes action buttons
    /// such as "OK", "Cancel", "Submit", "Save", "Delete", or other operation-specific controls.
    /// The footer may also contain validation messages, progress indicators, or secondary actions.
    /// Common patterns include:
    /// - Confirmation dialogs: "Cancel" and "OK" buttons
    /// - Form dialogs: "Cancel" and "Submit" buttons with validation feedback
    /// - Alert dialogs: Single "OK" or multiple choice buttons
    /// - Destructive actions: "Cancel" and "Delete" buttons with appropriate styling
    /// The layout typically follows platform conventions with primary actions on the right.
    /// </param>
    public DialogFooter(params object[] children) : base(children)
    {
    }
}