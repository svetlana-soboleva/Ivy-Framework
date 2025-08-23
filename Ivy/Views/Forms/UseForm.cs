using Ivy.Core;
using Ivy.Core.Hooks;
using Ivy.Shared;

namespace Ivy.Views.Forms;

/// <summary>
/// Provides extension methods for advanced form usage patterns including modal presentations and custom form lifecycle management.
/// </summary>
/// <remarks>
/// This class extends the form system with sophisticated presentation modes and lifecycle management.
/// It enables forms to be displayed in modal contexts (sheets and dialogs) with automatic state management,
/// validation coordination, and user interaction handling. These extensions are particularly useful
/// for creating polished user interfaces with proper modal behavior and form submission workflows.
/// </remarks>
public static class UseFormExtensions
{
    /// <summary>
    /// Creates a form instance from a form builder factory with optimized state management for custom layouts.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object that the form will edit.</typeparam>
    /// <param name="context">The view context for state management and lifecycle coordination.</param>
    /// <param name="factory">A factory function that creates and configures the form builder.</param>
    /// <returns>A tuple containing the submit handler, form view, validation view, and loading state for custom form layouts.</returns>
    /// <remarks>
    /// <para>This extension method provides an optimized way to use forms in custom layouts by managing
    /// the form builder lifecycle efficiently. The factory function is called only once and cached,
    /// preventing unnecessary rebuilds while maintaining proper state synchronization.</para>
    /// 
    /// <para><strong>Key Benefits:</strong></para>
    /// <list type="bullet">
    /// <item><description><strong>Performance:</strong> Form builder is created once and cached for efficiency</description></item>
    /// <item><description><strong>Flexibility:</strong> Returns individual components for complete layout control</description></item>
    /// <item><description><strong>State Management:</strong> Proper lifecycle management with automatic cleanup</description></item>
    /// <item><description><strong>Validation:</strong> Coordinated validation across all form fields</description></item>
    /// </list>
    /// 
    /// <para><strong>Usage Example:</strong></para>
    /// <code>
    /// var (onSubmit, formView, validationView, loading) = this.UseForm(() =>
    ///     userState.ToForm()
    ///         .Label(x => x.Name, "Full Name")
    ///         .Required(x => x.Email)
    /// );
    /// 
    /// return Layout.Vertical()
    ///     | new Card(formView).Title("User Information")
    ///     | Layout.Horizontal(
    ///         new Button("Save").HandleClick(onSubmit).Loading(loading),
    ///         validationView
    ///     );
    /// </code>
    /// </remarks>
    public static (Func<Task<bool>> onSubmit, IView formView, IView validationView, bool loading) UseForm<TModel>(this IViewContext context, Func<FormBuilder<TModel>> factory)
    {
        return context.UseState(factory, buildOnChange: false).Value.UseForm(context);
    }

    /// <summary>
    /// Converts a form builder into a sheet modal presentation with automatic submission handling and state management.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object that the form will edit.</typeparam>
    /// <param name="formBuilder">The configured form builder to display in the sheet.</param>
    /// <param name="isOpen">Reactive state controlling the visibility of the sheet modal.</param>
    /// <param name="title">Optional title displayed in the sheet header.</param>
    /// <param name="description">Optional description text displayed below the title.</param>
    /// <param name="submitTitle">Optional custom text for the submit button, defaults to the form builder's submit title.</param>
    /// <param name="width">Optional custom width for the sheet, defaults to Sheet.DefaultWidth.</param>
    /// <returns>A view that renders the form as a sheet modal with proper submission and cancellation handling.</returns>
    /// <remarks>
    /// <para>This extension method transforms any form builder into a polished sheet modal presentation
    /// with complete user interaction handling. The sheet automatically manages form submission,
    /// validation display, loading states, and modal dismissal based on user actions.</para>
    /// 
    /// <para><strong>Modal Behavior:</strong></para>
    /// <list type="bullet">
    /// <item><description><strong>Automatic Dismissal:</strong> Sheet closes on successful form submission</description></item>
    /// <item><description><strong>Cancel Handling:</strong> Cancel button immediately closes the sheet without saving</description></item>
    /// <item><description><strong>Validation Integration:</strong> Validation errors prevent submission and are displayed inline</description></item>
    /// <item><description><strong>Loading States:</strong> Submit button shows loading state during async operations</description></item>
    /// <item><description><strong>Footer Layout:</strong> Professional footer with submit, cancel, and validation display</description></item>
    /// </list>
    /// 
    /// <para><strong>Usage Example:</strong></para>
    /// <code>
    /// var isEditUserOpen = UseState(false);
    /// 
    /// return Layout.Vertical()
    ///     | new Button("Edit User").HandleClick(_ => isEditUserOpen.Set(true))
    ///     | userState.ToForm()
    ///         .Label(x => x.Name, "Full Name")
    ///         .Required(x => x.Email)
    ///         .ToSheet(
    ///             isEditUserOpen,
    ///             title: "Edit User Information",
    ///             description: "Please update the user details below.",
    ///             submitTitle: "Save Changes",
    ///             width: Size.Units(400)
    ///         );
    /// </code>
    /// </remarks>
    public static IView ToSheet<TModel>(this FormBuilder<TModel> formBuilder, IState<bool> isOpen, string? title = null, string? description = null, string? submitTitle = null, Size? width = null)
    {
        return new FuncView((context) =>
            {
                (Func<Task<bool>> onSubmit, IView formView, IView validationView, bool loading) = formBuilder.UseForm(context);

                if (!isOpen.Value) return null; //shouldn't happen

                async void HandleSubmit()
                {
                    if (await onSubmit())
                    {
                        isOpen.Value = false;
                    }
                }

                var layout = new FooterLayout(
                    Layout.Horizontal().Gap(2)
                        | new Button(submitTitle ?? formBuilder.SubmitTitle).HandleClick(new Action(HandleSubmit).ToEventHandler<Button>())
                            .Loading(loading).Disabled(loading)
                        | new Button("Cancel").Variant(ButtonVariant.Outline).HandleClick(_ => isOpen.Set(false))
                        | validationView,
                    formView
                );

                return new Sheet(_ =>
                {
                    isOpen.Value = false;
                }, layout, title, description).Width(width ?? Sheet.DefaultWidth);
            }
        );
    }

    /// <summary>
    /// Converts a form builder into a dialog modal presentation with structured layout and automatic interaction handling.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object that the form will edit.</typeparam>
    /// <param name="formBuilder">The configured form builder to display in the dialog.</param>
    /// <param name="isOpen">Reactive state controlling the visibility of the dialog modal.</param>
    /// <param name="title">Optional title displayed in the dialog header.</param>
    /// <param name="description">Optional description text displayed in the dialog body above the form.</param>
    /// <param name="submitTitle">Optional custom text for the submit button, defaults to the form builder's submit title.</param>
    /// <param name="width">Optional custom width for the dialog, defaults to Dialog.DefaultWidth.</param>
    /// <returns>A view that renders the form as a dialog modal with proper header, body, and footer structure.</returns>
    /// <remarks>
    /// <para>This extension method creates a structured dialog modal presentation with proper semantic
    /// layout using DialogHeader, DialogBody, and DialogFooter components. The dialog provides
    /// a more formal presentation style compared to sheets, suitable for important form interactions
    /// that require user attention and explicit confirmation.</para>
    /// 
    /// <para><strong>Dialog Structure:</strong></para>
    /// <list type="bullet">
    /// <item><description><strong>Header:</strong> Contains the title with proper typography and spacing</description></item>
    /// <item><description><strong>Body:</strong> Displays optional description text followed by the form fields</description></item>
    /// <item><description><strong>Footer:</strong> Contains validation messages, cancel button, and submit button</description></item>
    /// <item><description><strong>Interaction:</strong> Automatic modal dismissal on successful submission or cancellation</description></item>
    /// <item><description><strong>Accessibility:</strong> Proper focus management and keyboard navigation support</description></item>
    /// </list>
    /// 
    /// <para><strong>Usage Example:</strong></para>
    /// <code>
    /// var isCreateUserOpen = UseState(false);
    /// 
    /// return Layout.Vertical()
    ///     | new Button("Create New User").HandleClick(_ => isCreateUserOpen.Set(true))
    ///     | newUserState.ToForm()
    ///         .Required(x => x.Name, x => x.Email)
    ///         .Group("Personal", x => x.Name, x => x.Email)
    ///         .Group("Settings", x => x.Role, x => x.Department)
    ///         .ToDialog(
    ///             isCreateUserOpen,
    ///             title: "Create New User",
    ///             description: "Enter the details for the new user account. All required fields must be completed.",
    ///             submitTitle: "Create User",
    ///             width: Size.Units(500)
    ///         );
    /// </code>
    /// 
    /// <para><strong>Best Practices:</strong></para>
    /// <list type="bullet">
    /// <item><description>Use dialogs for critical actions that require explicit user confirmation</description></item>
    /// <item><description>Provide clear, descriptive titles that indicate the action being performed</description></item>
    /// <item><description>Include helpful description text for complex forms or important operations</description></item>
    /// <item><description>Choose appropriate widths based on form complexity and content requirements</description></item>
    /// </list>
    /// </remarks>
    public static IView ToDialog<TModel>(this FormBuilder<TModel> formBuilder, IState<bool> isOpen, string? title = null, string? description = null, string? submitTitle = null, Size? width = null)
    {
        return new FuncView((context) =>
            {
                (Func<Task<bool>> onSubmit, IView formView, IView validationView, bool loading) = formBuilder.UseForm(context);

                if (!isOpen.Value) return null; //shouldn't happen

                async void HandleSubmit()
                {
                    if (await onSubmit())
                    {
                        isOpen.Value = false;
                    }
                }

                return new Dialog(
                    _ => isOpen.Set(false),
                    new DialogHeader(title ?? ""),
                    new DialogBody(
                        Layout.Vertical()
                            | description!
                            | formView
                    ),
                    new DialogFooter(
                        validationView,
                        new Button("Cancel", _ => isOpen.Value = false, variant: ButtonVariant.Outline),
                        new Button(submitTitle ?? formBuilder.SubmitTitle).HandleClick(new Action(HandleSubmit).ToEventHandler<Button>())
                            .Loading(loading).Disabled(loading)
                    )
                ).Width(width ?? Dialog.DefaultWidth);
            }
        );
    }
}
