using Ivy.Shared;

namespace Ivy.Samples.Shared.Apps.Widgets;

[App(icon: Icons.IdCard, path: ["Widgets"])]
public class CardApp : SampleBase
{
    protected override object? BuildSample()
    {
        var client = UseService<IClientProvider>();

        var card1 = new Card(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam nec purus nec nunc",
            new Button("Sign Me Up", _ => client.Toast("You have signed up!"))
        ).Title("Card App").Description("This is a card app.");

        var card2 = new Card(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam nec purus nec nunc"
        ).Title("Card with Border")
         .Description("This card has a custom border.")
         .BorderThickness(3)
         .BorderStyle(BorderStyle.Dashed)
         .BorderColor(Colors.Primary)
         .BorderRadius(BorderRadius.Rounded);

        var card3 = new Card(
            "This card demonstrates the border color fix with a thick red border."
        ).Title("Border Color Test")
         .Description("Should now display with a red border")
         .BorderThickness(4)
         .BorderStyle(BorderStyle.Solid)
         .BorderColor(Colors.Red)
         .BorderRadius(BorderRadius.Rounded);

        return Layout.Vertical().Width(Size.Units(100))
               | card1
               | card2
               | card3
            ;
    }
}