using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a modal dialog widget that displays content in an overlay above the main application interface.
/// Dialogs are used for focused interactions such as confirmations, forms, alerts, and detailed information display.
/// They consist of three main sections: header, body, and footer, providing a structured layout for modal content.
/// </summary>
public record Dialog : WidgetBase<Dialog>
{
    /// <summary>
    /// Gets the default width for dialog widgets, set to 24 rem units.
    /// This provides a reasonable default size that works well for most dialog content
    /// while remaining responsive across different screen sizes.
    /// </summary>
    /// <value>A Size object representing 24 rem units.</value>
    public static Size DefaultWidth => Size.Rem(24);

    /// <summary>
    /// Initializes a new instance of the Dialog class with the specified close handler and dialog sections.
    /// The dialog is structured with three distinct sections that provide a consistent layout pattern
    /// for modal interactions across the application.
    /// </summary>
    /// <param name="onClose">
    /// The event handler called when the dialog should be closed. This is typically triggered by
    /// user actions such as clicking a close button, pressing the Escape key, or clicking outside
    /// the dialog area. The handler should update the application state to hide the dialog.
    /// </param>
    /// <param name="header">
    /// The <see cref="DialogHeader"/> widget that displays the dialog title and optional close controls.
    /// This section typically contains the dialog's title text and provides visual hierarchy.
    /// </param>
    /// <param name="body">
    /// The <see cref="DialogBody"/> widget that contains the main content of the dialog.
    /// This can include forms, text, images, or any other widgets that represent the dialog's purpose.
    /// </param>
    /// <param name="footer">
    /// The <see cref="DialogFooter"/> widget that typically contains action buttons and controls.
    /// Common elements include Cancel, OK, Submit, or other action buttons relevant to the dialog's function.
    /// </param>
    [OverloadResolutionPriority(1)]
    public Dialog(Func<Event<Dialog>, ValueTask> onClose, DialogHeader header, DialogBody body, DialogFooter footer) : base([header, body, footer])
    {
        OnClose = onClose;
    }

    /// <summary>
    /// Gets or sets the event handler called when the dialog should be closed.
    /// This event is triggered by various user interactions such as clicking close buttons,
    /// pressing the Escape key, clicking outside the dialog, or other dismissal actions.
    /// The handler is responsible for updating the application state to hide the dialog.
    /// </summary>
    /// <value>An action that receives a <see cref="Event{T}"/> with this dialog as the source.</value>
    [Event] public Func<Event<Dialog>, ValueTask> OnClose { get; set; }

    /// <summary>
    /// Overrides the | operator to prevent adding children to Dialog widgets.
    /// Dialog widgets have a fixed structure consisting of exactly three sections: header, body, and footer.
    /// Content should be added to the appropriate DialogBody section rather than directly to the Dialog.
    /// </summary>
    /// <param name="dialog">The Dialog widget.</param>
    /// <param name="child">The child object attempting to be added.</param>
    /// <returns>This method always throws an exception.</returns>
    /// <exception cref="NotSupportedException">
    /// Always thrown because Dialog widgets have a fixed three-section structure.
    /// Use the DialogHeader, DialogBody, and DialogFooter constructor parameters to structure content.
    /// </exception>
    public static Dialog operator |(Dialog dialog, object child)
    {
        throw new NotSupportedException("Dialog does not support children.");
    }

    /// <summary>
    /// Compatibility constructor for Action-based event handlers.
    /// Automatically wraps Action delegates in ValueTask-returning functions for backward compatibility.
    /// </summary>
    /// <param name="onClose">Action-based event handler called when the dialog should be closed.</param>
    /// <param name="header">The DialogHeader widget that displays the dialog title and optional close controls.</param>
    /// <param name="body">The DialogBody widget that contains the main content of the dialog.</param>
    /// <param name="footer">The DialogFooter widget that typically contains action buttons and controls.</param>
    public Dialog(Action<Event<Dialog>> onClose, DialogHeader header, DialogBody body, DialogFooter footer)
        : this(e => { onClose(e); return ValueTask.CompletedTask; }, header, body, footer)
    {
    }
}
