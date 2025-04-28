using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets;

[App(icon:Icons.SquareChevronRight, path: ["Widgets"])]
public class ButtonApp() : SampleBase
{
    protected override object? BuildSample()
    {
        var label = this.UseState("Click a button"); 
        
        var eventHandler = (Event<Button> e) =>
        {
            label.Set($"Button {e.Sender.Title} was clicked.");
        };
        
        return Layout.Vertical(
            Layout.Horizontal(
                new Button("Default", eventHandler, variant: ButtonVariant.Default),
                new Button("Destructive", eventHandler, variant: ButtonVariant.Destructive),
                new Button("Secondary", eventHandler, variant: ButtonVariant.Secondary),
                new Button("Outline", eventHandler, variant: ButtonVariant.Outline),
                new Button("Ghost", eventHandler, variant: ButtonVariant.Ghost),
                new Button("Link", eventHandler, variant: ButtonVariant.Link)
            ),
            Layout.Horizontal(new Button("Button With Icon", eventHandler, icon:Icons.MessageSquareX)),
            Text.Literal(label.Value)
        );
    }
}