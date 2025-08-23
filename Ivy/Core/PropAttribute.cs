namespace Ivy.Core;

/// <summary>
/// Attribute used to mark properties as serializable widget properties that should be included in the widget's JSON representation.
/// This attribute supports both regular properties and attached properties for parent-child widget relationships.
/// </summary>
/// <param name="attached">
/// Optional name of the attached property. When specified, this property becomes an attached property
/// that can be set on child widgets and accessed by the parent widget.
/// </param>
[AttributeUsage(AttributeTargets.Property)]
public class PropAttribute(string? attached = null) : Attribute
{
    /// <summary>
    /// Gets or sets the name of the attached property. 
    /// When not null, this indicates the property is an attached property that can be set on child widgets.
    /// </summary>
    /// <value>
    /// The name of the attached property, or null if this is a regular property.
    /// </value>
    public string? AttachedName { get; set; } = attached;

    /// <summary>
    /// Gets a value indicating whether this property is an attached property.
    /// Attached properties are used in parent-child widget relationships where the parent widget
    /// can define properties that are set on child widgets (e.g., GridLayout.GridColumn on child widgets).
    /// </summary>
    /// <value>
    /// true if this is an attached property (AttachedName is not null); otherwise, false.
    /// </value>
    public bool IsAttached => AttachedName != null;
}