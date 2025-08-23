using Ivy.Core;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Wrapper widget that provides structured layout and metadata for form input controls.
/// FormField widgets combine input controls with labels, descriptions, and validation indicators
/// to create complete, accessible form field experiences. They handle the integration between
/// input controls and their associated metadata, ensuring consistent form field presentation.
/// </summary>
public record FormField : WidgetBase<FormField>
{
    /// <summary>
    /// Initializes a new instance of the FormField class with the specified input control and metadata.
    /// This constructor is internal as FormField widgets are typically created through FormBuilder
    /// rather than being instantiated directly. It intelligently handles label and description
    /// assignment, preferring input-specific values when available.
    /// </summary>
    /// <param name="input">
    /// The input control to wrap within this form field. Must implement <see cref="IAnyInput"/>.
    /// This is the interactive element that users will engage with for data entry.
    /// </param>
    /// <param name="label">
    /// The optional label text to display for this form field. If the input control already
    /// has a label property, that value takes precedence. If null, no additional label is added.
    /// </param>
    /// <param name="description">
    /// The optional description or help text to display for this form field. Provides additional
    /// context or instructions to help users understand the expected input. If the input control
    /// already has a description property, that value takes precedence.
    /// </param>
    /// <param name="required">
    /// Indicates whether this form field is required for form submission. When true, the field
    /// will be marked as required and may display visual indicators such as asterisks or
    /// different styling to communicate the requirement to users.
    /// </param>
    internal FormField(IAnyInput input, string? label = null, string? description = null, bool required = false) : base([input])
    {
        var labelProp = input.GetType().GetProperty("Label");
        if (labelProp != null && labelProp.PropertyType == typeof(string))
        {
            //Input handles label on its own
            var inputLabel = (string?)labelProp.GetValue(input);
            labelProp.SetValue(input, inputLabel ?? label);
            label = null;
        }

        var descriptionProp = input.GetType().GetProperty("Description");
        if (descriptionProp != null && descriptionProp.PropertyType == typeof(string))
        {
            //Input handles description on its own
            var inputDescription = (string?)descriptionProp.GetValue(input);
            descriptionProp.SetValue(input, inputDescription ?? description);
            description = null;
        }
        Label = label;
        Description = description;
        Required = required;
    }

    /// <summary>
    /// Gets or sets the label text displayed for this form field.
    /// The label provides a clear identifier for the input control, helping users understand
    /// what information is expected. When null, no additional label is displayed beyond
    /// what the input control itself might provide.
    /// </summary>
    /// <value>The label text, or null if no additional label should be displayed.</value>
    [Prop] public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the description or help text displayed for this form field.
    /// The description provides additional context, instructions, or examples to help users
    /// understand the expected input format or purpose of the field.
    /// </summary>
    /// <value>
    /// The description text, or null if no additional description should be displayed.
    /// This text typically appears below the input control in a smaller, muted style.
    /// </value>
    [Prop] public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this form field is required for form submission.
    /// Required fields are typically marked with visual indicators (such as asterisks) and
    /// may prevent form submission if left empty during validation.
    /// </summary>
    /// <value>
    /// true if the field is required for form submission; false if the field is optional.
    /// Default is false.
    /// </value>
    [Prop] public bool Required { get; set; }

    /// <summary>
    /// Overrides the | operator to prevent adding children to FormField widgets.
    /// FormField widgets are designed to wrap a single input control and provide metadata.
    /// The input control is set through the constructor, and additional children would
    /// disrupt the structured form field layout and accessibility.
    /// </summary>
    /// <param name="widget">The FormField widget.</param>
    /// <param name="child">The child object attempting to be added.</param>
    /// <returns>This method always throws an exception.</returns>
    /// <exception cref="NotSupportedException">
    /// Always thrown because FormField widgets wrap a single input control.
    /// Use the constructor to specify the input control for this form field.
    /// </exception>
    public static FormField operator |(FormField widget, object child)
    {
        throw new NotSupportedException("FormField does not support children.");
    }
}