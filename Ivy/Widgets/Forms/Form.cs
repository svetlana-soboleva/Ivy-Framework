// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Container widget that groups form fields and input controls into a structured form layout.
/// Form widgets provide a semantic container for organizing related input fields, typically
/// generated automatically by FormBuilder from model objects. Forms handle layout, validation
/// display, and submission workflows for data collection interfaces.
/// </summary>
public record Form : WidgetBase<Form>
{
    /// <summary>
    /// Initializes a new instance of the Form class with the specified form fields and controls.
    /// This constructor is internal as Form widgets are typically created through FormBuilder
    /// rather than being instantiated directly.
    /// </summary>
    /// <param name="children">
    /// The form fields, input controls, and other widgets that make up the form content.
    /// This typically includes FormField widgets containing input controls, validation messages,
    /// layout containers, and other form-related elements. The children are arranged in a
    /// structured layout that provides a cohesive data collection interface.
    /// </param>
    internal Form(params object[] children) : base(children)
    {

    }
}