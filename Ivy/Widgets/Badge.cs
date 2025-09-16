using Ivy.Core;
using Ivy.Core.Docs;
using Ivy.Shared;

namespace Ivy;

/// <summary>
/// Defines the visual style variants available for badge widgets, controlling their
/// appearance, color scheme, and visual emphasis to suit different use cases and
/// design requirements.
/// </summary>
public enum BadgeVariant
{
    /// <summary>Default primary badge style with prominent visual emphasis, typically used for main actions or important information.</summary>
    Primary,
    /// <summary>Destructive badge style with warning or error colors, typically used for critical actions or negative status indicators.</summary>
    Destructive,
    /// <summary>Outline badge style with bordered appearance, typically used for secondary actions or subtle status indicators.</summary>
    Outline,
    /// <summary>Secondary badge style with reduced visual emphasis, typically used for supporting information or less prominent status indicators.</summary>
    Secondary,
    /// <summary>Success badge style with positive or confirmation colors, typically used for successful operations or positive status indicators.</summary>
    Success,
    /// <summary>Warning badge style with cautionary colors, typically used for alerts or important notices that require attention.</summary>
    Warning,
    /// <summary>Info badge style with informational colors, typically used for general information or help indicators.</summary>
    Info
}

/// <summary>
/// Represents a badge widget that displays small pieces of information like counts,
/// statuses, or labels in a compact, styled format. This widget is versatile and
/// can be used for various purposes including notifications, status indicators,
/// categories, and priority levels.
/// 
/// The Badge widget supports multiple visual variants, sizes, and icon integration
/// to create informative and visually appealing indicators that enhance user
/// interface clarity and information hierarchy.
/// </summary>
public record Badge : WidgetBase<Badge>
{
    /// <summary>
    /// Initializes a new instance of the Badge class with the specified title, variant,
    /// and icon. The badge will display the provided information with appropriate
    /// styling based on the variant and size settings.
    /// </summary>
    /// <param name="title">The text content to display in the badge. This can be a count,
    /// status, label, or any short text that needs to be highlighted. When null, creates
    /// an icon-only badge.</param>
    /// <param name="variant">The visual style variant for the badge, controlling its
    /// appearance and color scheme. Default is <see cref="BadgeVariant.Primary"/>.</param>
    /// <param name="icon">The icon to display alongside or instead of the title text.
    /// This enhances visual meaning and can be positioned on either side of the text.
    /// Default is <see cref="Icons.None"/>.</param>
    public Badge(string? title = null, BadgeVariant variant = BadgeVariant.Primary, Icons icon = Icons.None)
    {
        Title = title;
        Variant = variant;
        Icon = icon;
    }

    /// <summary>
    /// Gets or sets the text content displayed in the badge.
    /// This property contains the primary information that the badge communicates,
    /// such as counts, status text, labels, or other short descriptive content.
    /// 
    /// When null, the badge becomes icon-only, displaying only the icon without
    /// any text content. This is useful for creating compact visual indicators
    /// where the icon alone conveys the necessary information.
    /// Default is null (icon-only badge).
    /// </summary>
    [Prop] public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the visual style variant for the badge.
    /// This property controls the badge's appearance, color scheme, and visual
    /// emphasis, allowing you to choose the most appropriate style for your
    /// use case and design requirements.
    /// 
    /// Different variants provide different levels of visual emphasis and are
    /// suited for different purposes, from primary actions to subtle indicators.
    /// Default is <see cref="BadgeVariant.Primary"/>.
    /// </summary>
    [Prop] public BadgeVariant Variant { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed in the badge.
    /// This property allows you to add visual context and meaning to the badge,
    /// enhancing its communicative value and visual appeal.
    /// 
    /// Icons can be positioned on either side of the text (or used alone for
    /// icon-only badges) and should be chosen to complement the badge's purpose
    /// and content.
    /// Default is <see cref="Icons.None"/>.
    /// </summary>
    [Prop] public Icons? Icon { get; set; }

    /// <summary>
    /// Gets or sets the size of the badge.
    /// This property controls the overall dimensions and visual prominence of
    /// the badge, allowing you to match it to your design requirements and
    /// the importance of the information being displayed.
    /// 
    /// Different sizes provide different levels of visual emphasis and are
    /// suited for different contexts, from compact indicators to prominent
    /// status displays.
    /// Default is <see cref="Sizes.Medium"/>.
    /// </summary>
    [Prop] public Sizes Size { get; set; } = Sizes.Medium;

    /// <summary>
    /// Gets or sets the position of the icon relative to the title text.
    /// This property controls whether the icon appears to the left or right
    /// of the text content, allowing you to create visually balanced and
    /// contextually appropriate badge layouts.
    /// 
    /// Icon positioning can affect the visual flow and readability of the
    /// badge, with left positioning being more common for most use cases.
    /// Default is <see cref="Align.Left"/>.
    /// </summary>
    [Prop] public Align IconPosition { get; set; } = Align.Left;

    /// <summary>
    /// Operator overload that prevents adding children to the Badge using the pipe operator.
    /// Badge widgets are self-contained components that don't support additional
    /// child content beyond their configured title, icon, and styling properties.
    /// </summary>
    /// <param name="badge">The Badge widget.</param>
    /// <param name="child">The child content to add (not supported).</param>
    /// <returns>This method always throws a NotSupportedException.</returns>
    /// <exception cref="NotSupportedException">Always thrown, as Badge does not support additional children.</exception>
    public static Badge operator |(Badge badge, object child)
    {
        throw new NotSupportedException("Badge does not support children.");
    }
}

/// <summary>
/// Provides extension methods for the Badge widget that enable a fluent API for
/// configuring badge appearance and behavior. These methods allow you to easily
/// set variants, sizes, icons, and icon positioning for optimal badge presentation.
/// </summary>
public static class BadgeExtensions
{
    /// <summary>
    /// Sets the icon and its position for the badge.
    /// This method allows you to configure both the icon display and its positioning
    /// relative to the title text, creating visually balanced and contextually
    /// appropriate badge layouts.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <param name="icon">The icon to display in the badge, or null to remove the icon.</param>
    /// <param name="position">The position of the icon relative to the title text. Default is <see cref="Align.Left"/>.</param>
    /// <returns>A new Badge instance with the updated icon and position settings.</returns>
    public static Badge Icon(this Badge badge, Icons? icon, Align position = Align.Left)
    {
        return badge with { Icon = icon, IconPosition = position };
    }

    /// <summary>
    /// Sets the visual style variant for the badge.
    /// This method allows you to change the badge's appearance and color scheme
    /// after creation, enabling dynamic styling based on context or user preferences.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <param name="variant">The visual style variant to apply to the badge.</param>
    /// <returns>A new Badge instance with the updated variant setting.</returns>
    public static Badge Variant(this Badge badge, BadgeVariant variant)
    {
        return badge with { Variant = variant };
    }

    /// <summary>
    /// Sets the size of the badge.
    /// This method allows you to control the badge's dimensions and visual prominence
    /// after creation, enabling dynamic sizing based on context or design requirements.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <param name="size">The size to apply to the badge.</param>
    /// <returns>A new Badge instance with the updated size setting.</returns>
    public static Badge Size(this Badge badge, Sizes size)
    {
        return badge with { Size = size };
    }

    /// <summary>
    /// Sets the badge size to large for prominent display.
    /// This convenience method creates a large badge that provides maximum
    /// visual emphasis and is ideal for important status indicators or
    /// prominent information display.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with large size applied.</returns>
    [RelatedTo(nameof(Badge.Size))]
    public static Badge Large(this Badge badge)
    {
        return badge.Size(Sizes.Large);
    }

    /// <summary>
    /// Sets the badge size to small for compact display.
    /// This convenience method creates a small badge that provides minimal
    /// visual footprint and is ideal for subtle indicators or space-constrained
    /// layouts.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with small size applied.</returns>
    [RelatedTo(nameof(Badge.Size))]
    public static Badge Small(this Badge badge)
    {
        return badge.Size(Sizes.Small);
    }

    /// <summary>
    /// Sets the badge variant to secondary for reduced visual emphasis.
    /// This convenience method creates a secondary badge that provides
    /// subtle visual styling suitable for supporting information or
    /// less prominent status indicators.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with secondary variant applied.</returns>
    [RelatedTo(nameof(Badge.Variant))]
    public static Badge Secondary(this Badge badge)
    {
        return badge with { Variant = BadgeVariant.Secondary };
    }

    /// <summary>
    /// Sets the badge variant to destructive for warning or error styling.
    /// This convenience method creates a destructive badge that provides
    /// prominent visual styling suitable for critical actions, warnings,
    /// or negative status indicators.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with destructive variant applied.</returns>
    [RelatedTo(nameof(Badge.Variant))]
    public static Badge Destructive(this Badge badge)
    {
        return badge with { Variant = BadgeVariant.Destructive };
    }

    /// <summary>
    /// Sets the badge variant to outline for bordered appearance.
    /// This convenience method creates an outline badge that provides
    /// subtle visual styling with borders, suitable for secondary actions
    /// or subtle status indicators.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with outline variant applied.</returns>
    [RelatedTo(nameof(Badge.Variant))]
    public static Badge Outline(this Badge badge)
    {
        return badge with { Variant = BadgeVariant.Outline };
    }

    /// <summary>
    /// Sets the badge variant to primary for maximum visual emphasis.
    /// This convenience method creates a primary badge that provides
    /// prominent visual styling suitable for main actions or important
    /// information display.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with primary variant applied.</returns>
    [RelatedTo(nameof(Badge.Variant))]
    public static Badge Primary(this Badge badge)
    {
        return badge with { Variant = BadgeVariant.Primary };
    }

    /// <summary>
    /// Sets the badge variant to success for positive or confirmation styling.
    /// This convenience method creates a success badge that provides
    /// positive visual styling suitable for successful operations,
    /// confirmations, or positive status indicators.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with success variant applied.</returns>
    [RelatedTo(nameof(Badge.Variant))]
    public static Badge Success(this Badge badge)
    {
        return badge with { Variant = BadgeVariant.Success };
    }

    /// <summary>
    /// Sets the badge variant to warning for cautionary styling.
    /// This convenience method creates a warning badge that provides
    /// cautionary visual styling suitable for alerts, important notices,
    /// or conditions that require user attention.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with warning variant applied.</returns>
    [RelatedTo(nameof(Badge.Variant))]
    public static Badge Warning(this Badge badge)
    {
        return badge with { Variant = BadgeVariant.Warning };
    }

    /// <summary>
    /// Sets the badge variant to info for informational styling.
    /// This convenience method creates an info badge that provides
    /// informational visual styling suitable for general information,
    /// help text, or neutral status indicators.
    /// </summary>
    /// <param name="badge">The Badge to configure.</param>
    /// <returns>A new Badge instance with info variant applied.</returns>
    [RelatedTo(nameof(Badge.Variant))]
    public static Badge Info(this Badge badge)
    {
        return badge with { Variant = BadgeVariant.Info };
    }
}
