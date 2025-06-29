using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets;

[App(icon: Icons.Pill, path: ["Widgets"])]
public class BadgeApp : SampleBase
{
    private static readonly BadgeVariant[] Variants = [
        BadgeVariant.Default,
        BadgeVariant.Destructive,
        BadgeVariant.Secondary,
        BadgeVariant.Outline
    ];

    private static readonly string[] VariantNames = [
        "Default",
        "Destructive",
        "Secondary",
        "Outline"
    ];

    protected override object? BuildSample()
    {
        var clickCount = this.UseState(0);
        var lastClicked = this.UseState("None");

        var eventHandler = (Event<Button> e) =>
        {
            clickCount.Set(clickCount.Value + 1);
            lastClicked.Set(e.Sender.Title ?? "Unknown");
        };

        var createBadgeRow = (Func<BadgeVariant, Badge> badgeFactory) =>
            Layout.Grid().Columns(Variants.Length)
            | VariantNames.Select(name => Text.Block(name)).ToArray()
            | Variants.Select(badgeFactory).ToArray();

        return Layout.Vertical()
               | Text.H1("Badges")
               | Text.H2("Variants")
               | createBadgeRow(variant => new Badge(VariantNames[Array.IndexOf(Variants, variant)], eventHandler, variant: variant))

               | Text.H2("States")
               | (Layout.Grid().Columns(Variants.Length)
                  | VariantNames.Select(name => Text.Block(name)).ToArray()

                  // Normal state
                  | Variants.Select(variant => new Badge(VariantNames[Array.IndexOf(Variants, variant)], eventHandler, variant: variant)).ToArray()

                  // Disabled state
                  | Variants.Select(variant => new Badge(VariantNames[Array.IndexOf(Variants, variant)], eventHandler, variant: variant).Disabled()).ToArray()
               )

               | Text.H2("Sizes")
               | (Layout.Grid().Columns(Variants.Length)
                  | VariantNames.Select(name => Text.Block(name)).ToArray()

                  // Small
                  | Variants.Select(variant => new Badge("Small", eventHandler, variant: variant).Small()).ToArray()

                  // Default
                  | Variants.Select(variant => new Badge("Default", eventHandler, variant: variant)).ToArray()

                  // Large
                  | Variants.Select(variant => new Badge("Large", eventHandler, variant: variant).Large()).ToArray()
               )

               | Text.H2("With Icons")
               | (Layout.Grid().Columns(Variants.Length)
                  | VariantNames.Select(name => Text.Block(name)).ToArray()

                  // Bell icon
                  | Variants.Select(variant => new Badge("With Bell", eventHandler, variant: variant, icon: Icons.Bell)).ToArray()

                  // Heart icon
                  | Variants.Select(variant => new Badge("With Heart", eventHandler, variant: variant, icon: Icons.Heart)).ToArray()

                  // Star icon
                  | Variants.Select(variant => new Badge("With Star", eventHandler, variant: variant, icon: Icons.Star)).ToArray()

                  // Check icon
                  | Variants.Select(variant => new Badge("With Check", eventHandler, variant: variant, icon: Icons.Check)).ToArray()
               )

               | Text.H2("Icon Positioning")
               | (Layout.Grid().Columns(Variants.Length)
                  | VariantNames.Select(name => Text.Block(name)).ToArray()

                  // Icon on left (default)
                  | Variants.Select(variant => new Badge("Left Icon", eventHandler, variant: variant).Icon(Icons.Bell, Align.Left)).ToArray()

                  // Icon on right
                  | Variants.Select(variant => new Badge("Right Icon", eventHandler, variant: variant).Icon(Icons.ArrowRight, Align.Right)).ToArray()
               )

               | Text.H2("Icon Only")
               | Layout.Horizontal(
                   new Badge(icon: Icons.Bell, onClick: eventHandler),
                   new Badge(icon: Icons.Heart, onClick: eventHandler, variant: BadgeVariant.Destructive),
                   new Badge(icon: Icons.Star, onClick: eventHandler, variant: BadgeVariant.Outline),
                   new Badge(icon: Icons.Check, onClick: eventHandler, variant: BadgeVariant.Secondary)
               )

               | Text.H2("Disabled vs Enabled (Clear Comparison)")
               | Layout.Horizontal(
                   Layout.Vertical(
                       Text.Block("Enabled:"),
                       new Badge("Clickable", eventHandler, variant: BadgeVariant.Default),
                       new Badge("Clickable", eventHandler, variant: BadgeVariant.Destructive),
                       new Badge("Clickable", eventHandler, variant: BadgeVariant.Secondary),
                       new Badge("Clickable", eventHandler, variant: BadgeVariant.Outline)
                   ),
                   Layout.Vertical(
                       Text.Block("Disabled:"),
                       new Badge("Not Clickable", eventHandler, variant: BadgeVariant.Default).Disabled(),
                       new Badge("Not Clickable", eventHandler, variant: BadgeVariant.Destructive).Disabled(),
                       new Badge("Not Clickable", eventHandler, variant: BadgeVariant.Secondary).Disabled(),
                       new Badge("Not Clickable", eventHandler, variant: BadgeVariant.Outline).Disabled()
                   )
               )

               | Text.H2("Interactive Demo")
               | Text.Literal($"Clicks: {clickCount.Value}")
               | Text.Literal($"Last clicked: {lastClicked.Value}")
            ;
    }
}