using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents the main content area of a dialog widget. The DialogBody contains the primary content
/// that users interact with, such as forms, text, images, or any other widgets that fulfill the dialog's purpose.
/// This section provides the main functionality and information display within the structured dialog layout.
/// </summary>
public record DialogBody : WidgetBase<DialogBody>
{
    /// <summary>
    /// Initializes a new instance of the DialogBody class with the specified child content.
    /// The DialogBody serves as a flexible container that can hold any combination of widgets
    /// to create the main content area of a dialog.
    /// </summary>
    /// <param name="children">
    /// The child widgets to be displayed within the dialog body. This can include any type of content
    /// such as text blocks, forms, input fields, images, lists, or complex widget compositions.
    /// The content should represent the primary purpose of the dialog, whether that's collecting user input,
    /// displaying information, showing confirmations, or providing interactive functionality.
    /// Common examples include form fields for data entry, descriptive text for confirmations,
    /// or detailed information displays for user review.
    /// </param>
    public DialogBody(params object[] children) : base(children)
    {
    }
}