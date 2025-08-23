using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Container widget that displays a collection of Detail widgets as a structured list of label-value pairs.
/// Details widgets are commonly used to present object properties, model fields, or any structured data
/// in a formatted, readable layout. They are typically generated automatically from objects using
/// the ToDetails() extension method and DetailsBuilder.
/// </summary>
public record Details : WidgetBase<Details>
{
    /// <summary>
    /// Initializes a new instance of the Details class with the specified collection of Detail widgets.
    /// Each Detail represents a single label-value pair, and together they form a comprehensive
    /// view of structured data such as object properties or database records.
    /// </summary>
    /// <param name="items">
    /// A collection of <see cref="Detail"/> widgets to be displayed in this Details container.
    /// Each Detail typically represents a property or field from an object, containing a label
    /// (property name) and a formatted value. The Details container will render these items
    /// in a structured, vertically-aligned list format that's easy to read and scan.
    /// Common sources include DetailsBuilder output from the ToDetails() extension method.
    /// </param>
    public Details(IEnumerable<Detail> items) : base(items.Cast<object>().ToArray())
    {
    }
}