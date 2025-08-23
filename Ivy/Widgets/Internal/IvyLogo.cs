using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// This widget renders the Ivy logo with customizable colors and consistent sizing, making it ideal
/// for headers, navigation bars, branding sections, and any UI element that requires the Ivy Framework
/// visual identity.
/// </summary>
public record IvyLogo : WidgetBase<IvyLogo>
{
    /// <summary>
    /// Initializes a new instance of the IvyLogo class with the specified color.
    /// The logo is automatically sized to 25 units width with auto-height for proper proportions.
    /// </summary>
    /// <param name="color">The color to use for the Ivy logo. Default is <see cref="Colors.Primary"/>.</param>
    public IvyLogo(Colors color = Colors.Primary) : base([])
    {
        Color = color;
        Width = Size.Units(25);
        Height = Size.Auto();
    }

    /// <summary>
    /// Gets or sets the color used to render the Ivy logo.
    /// This allows the logo to be customized to match different design themes, color schemes,
    /// or branding requirements while maintaining the official Ivy Framework visual identity.
    /// 
    /// The logo color can be set to any predefined color from the <see cref="Colors"/> enum,
    /// including primary, secondary, accent, and semantic colors for different contexts.
    /// Default is <see cref="Colors.Primary"/>.
    /// </summary>
    [Prop] public Colors Color { get; set; } = Colors.Primary;
}