using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a card widget that provides a structured container for organizing
/// related content and actions in visually grouped layouts.
/// 
/// The Card widget supports content organization through slots, header information
/// with titles and descriptions, icon integration, and comprehensive border
/// customization to create visually appealing and well-structured content areas.
/// Cards can hold various content types including text, buttons, charts, and
/// other widgets, making them fundamental for creating organized layouts.
/// </summary>
public record Card : WidgetBase<Card>
{
    /// <summary>
    /// Initializes a new instance of the Card class with the specified content
    /// and footer. The card will organize the provided content in a structured
    /// container with appropriate visual styling and layout management.
    /// </summary>
    /// <param name="content">The main content to display within the card body.
    /// This can be any widget, text, or combination of elements that need to
    /// be grouped together in the card container.</param>
    /// <param name="footer">Optional footer content to display at the bottom
    /// of the card. This is typically used for actions, additional information,
    /// or secondary content that should be visually separated from the main content.</param>
    public Card(object? content = null, object? footer = null) : base([new Slot("Content", content), new Slot("Footer", footer!)])
    {
        Width = Size.Full();
    }

    /// <summary>
    /// Gets or sets the title text displayed at the top of the card.
    /// 
    /// The title is typically displayed prominently at the top of the card
    /// and should be concise but descriptive of the card's purpose.
    /// Default is null (no title displayed).
    /// </summary>
    [Prop] public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the description text displayed below the title.
    /// This property provides additional context and explanation about the
    /// card's content or purpose, helping users better understand what
    /// they can expect from the card.
    /// 
    /// The description is typically displayed below the title and can provide
    /// more detailed information about the card's functionality or content.
    /// Default is null (no description displayed).
    /// </summary>
    [Prop] public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed alongside the title and description.
    /// This property allows you to add visual context and meaning to the card,
    /// enhancing its communicative value and visual appeal.
    /// 
    /// Icons are typically displayed near the title area and should be chosen
    /// to complement the card's purpose and content.
    /// Default is null (no icon displayed).
    /// </summary>
    [Prop] public Icons? Icon { get; set; }

    /// <summary>
    /// Gets or sets the thickness of the card's border.
    /// This property controls the visual weight and prominence of the card's
    /// border, allowing you to create cards with different levels of visual
    /// emphasis and separation from surrounding content.
    /// 
    /// Border thickness can be specified as a single integer value for uniform
    /// borders or as a Thickness object for different values on each side.
    /// When null, the card uses default border styling.
    /// Default is null (uses default border thickness).
    /// </summary>
    [Prop] public Thickness? BorderThickness { get; set; }

    /// <summary>
    /// Gets or sets the border radius for the card's corners.
    /// This property controls the visual styling of the card's edges,
    /// allowing you to create cards with different levels of corner
    /// rounding to match your design requirements.
    /// 
    /// Different border radius values provide different visual styles,
    /// from sharp corners to fully rounded card shapes.
    /// When null, the card uses default border radius styling.
    /// Default is null (uses default border radius).
    /// </summary>
    [Prop] public BorderRadius? BorderRadius { get; set; }

    /// <summary>
    /// Gets or sets the visual style of the card's border.
    /// This property controls how the border appears, allowing you to choose
    /// between different border styles such as solid, dashed, or dotted
    /// to achieve the desired visual effect.
    /// 
    /// Border styles can be used to create different visual hierarchies
    /// or to match specific design requirements and brand guidelines.
    /// When null, the card uses default border style.
    /// Default is null (uses default border style).
    /// </summary>
    [Prop] public BorderStyle? BorderStyle { get; set; }

    /// <summary>
    /// Gets or sets the color of the card's border.
    /// This property allows you to customize the border color to match
    /// your application's design system, create visual emphasis, or
    /// establish color-coded categorization of different card types.
    /// 
    /// Border colors can be used to create visual hierarchy, indicate
    /// status, or maintain brand consistency across your interface.
    /// When null, the card uses default border color.
    /// Default is null (uses default border color).
    /// </summary>
    [Prop] public Colors? BorderColor { get; set; }

    /// <summary>
    /// Operator overload that allows adding a single child to the Card using the pipe operator.
    /// This operator adds the child content to the card's main content area, replacing
    /// any existing content while maintaining the card's structure and styling.
    /// 
    /// The operator automatically creates a new Slot for the content and clears
    /// the footer slot, ensuring the card maintains its intended layout structure.
    /// Multiple children are not supported to maintain the card's focused design.
    /// </summary>
    /// <param name="widget">The Card widget to add content to.</param>
    /// <param name="child">The child content to add to the card's main content area.</param>
    /// <returns>A new Card instance with the updated content.</returns>
    /// <exception cref="NotSupportedException">Thrown when attempting to add multiple children at once.</exception>
    public static Card operator |(Card widget, object child)
    {
        if (child is IEnumerable<object> _)
        {
            throw new NotSupportedException("Cards does not support multiple children.");
        }

        return widget with { Children = [new Slot("Content", child), new Slot("Footer", null!)] };
    }
}

/// <summary>
/// Provides extension methods for the Card widget that enable a fluent API for
/// configuring card appearance, content, and border styling. These methods
/// allow you to easily set titles, descriptions, icons, and border properties
/// for optimal card presentation and visual organization.
/// </summary>
public static class CardExtensions
{
    /// <summary>
    /// Sets the title text for the card.
    /// This method allows you to set or change the card's title content
    /// after creation, enabling dynamic card labeling based on context
    /// or user preferences.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="title">The title text to display at the top of the card.</param>
    /// <returns>A new Card instance with the updated title.</returns>
    public static Card Title(this Card card, string title)
    {
        return card with { Title = title };
    }

    /// <summary>
    /// Sets the description text for the card.
    /// This method allows you to set or change the card's description content
    /// after creation, enabling dynamic card context based on content
    /// or user requirements.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="description">The description text to display below the title.</param>
    /// <returns>A new Card instance with the updated description.</returns>
    public static Card Description(this Card card, string description)
    {
        return card with { Description = description };
    }

    /// <summary>
    /// Sets the icon for the card.
    /// This method allows you to add or change the card's icon display
    /// after creation, enabling dynamic visual context based on content
    /// or design requirements.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="icon">The icon to display alongside the title and description.</param>
    /// <returns>A new Card instance with the updated icon.</returns>
    public static Card Icon(this Card card, Icons? icon)
    {
        return card with { Icon = icon };
    }

    /// <summary>
    /// Sets the border thickness for the card using an integer value.
    /// This method allows you to control the visual weight of the card's
    /// border, creating uniform border thickness across all sides.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="thickness">The uniform border thickness to apply to all sides.</param>
    /// <returns>A new Card instance with the updated border thickness.</returns>
    public static Card BorderThickness(this Card card, int thickness) => card with { BorderThickness = new(thickness) };

    /// <summary>
    /// Sets the border thickness for the card using a Thickness object.
    /// This method allows you to control the border thickness on each side
    /// independently, enabling precise control over the card's border appearance.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="thickness">The Thickness object defining border thickness for each side.</param>
    /// <returns>A new Card instance with the updated border thickness.</returns>
    public static Card BorderThickness(this Card card, Thickness thickness) => card with { BorderThickness = thickness };

    /// <summary>
    /// Sets the border radius for the card's corners.
    /// This method allows you to control the visual styling of the card's
    /// edges, enabling you to create cards with different levels of corner
    /// rounding to match your design requirements.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="radius">The border radius to apply to the card's corners.</param>
    /// <returns>A new Card instance with the updated border radius.</returns>
    public static Card BorderRadius(this Card card, BorderRadius radius) => card with { BorderRadius = radius };

    /// <summary>
    /// Sets the visual style of the card's border.
    /// This method allows you to choose between different border styles
    /// such as solid, dashed, or dotted to achieve the desired visual effect.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="style">The border style to apply to the card.</param>
    /// <returns>A new Card instance with the updated border style.</returns>
    public static Card BorderStyle(this Card card, BorderStyle style) => card with { BorderStyle = style };

    /// <summary>
    /// Sets the color of the card's border.
    /// This method allows you to customize the border color to match
    /// your application's design system, create visual emphasis, or
    /// establish color-coded categorization of different card types.
    /// </summary>
    /// <param name="card">The Card to configure.</param>
    /// <param name="color">The color to apply to the card's border.</param>
    /// <returns>A new Card instance with the updated border color.</returns>
    public static Card BorderColor(this Card card, Colors color) => card with { BorderColor = color };
}