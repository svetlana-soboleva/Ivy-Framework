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
        
        return Layout.Vertical()
               | Text.H1("Buttons")
               | Text.H2("Variants")
               | (Layout.Grid().Columns(6)
                  | Text.Block("Default")
                  | Text.Block("Destructive")
                  | Text.Block("Secondary")
                  | Text.Block("Outline")
                  | Text.Block("Ghost")
                  | Text.Block("Link")

                  | new Button("Default", eventHandler, variant: ButtonVariant.Default)
                  | new Button("Destructive", eventHandler, variant: ButtonVariant.Destructive)
                  | new Button("Secondary", eventHandler, variant: ButtonVariant.Secondary)
                  | new Button("Outline", eventHandler, variant: ButtonVariant.Outline)
                  | new Button("Ghost", eventHandler, variant: ButtonVariant.Ghost)
                  | new Button("Link", eventHandler, variant: ButtonVariant.Link)
               )
               
               | Text.H2("States")
               | (Layout.Grid().Columns(6)
                  | Text.Block("Default")
                  | Text.Block("Destructive")
                  | Text.Block("Secondary")
                  | Text.Block("Outline")
                  | Text.Block("Ghost")
                  | Text.Block("Link")

                  | new Button("Default", eventHandler, variant: ButtonVariant.Default)
                  | new Button("Destructive", eventHandler, variant: ButtonVariant.Destructive)
                  | new Button("Secondary", eventHandler, variant: ButtonVariant.Secondary)
                  | new Button("Outline", eventHandler, variant: ButtonVariant.Outline)
                  | new Button("Ghost", eventHandler, variant: ButtonVariant.Ghost)
                  | new Button("Link", eventHandler, variant: ButtonVariant.Link)

                  | new Button("Default", eventHandler, variant: ButtonVariant.Default).Disabled()
                  | new Button("Destructive", eventHandler, variant: ButtonVariant.Destructive).Disabled()
                  | new Button("Secondary", eventHandler, variant: ButtonVariant.Secondary).Disabled()
                  | new Button("Outline", eventHandler, variant: ButtonVariant.Outline).Disabled()
                  | new Button("Ghost", eventHandler, variant: ButtonVariant.Ghost).Disabled()
                  | new Button("Link", eventHandler, variant: ButtonVariant.Link).Disabled()

                  | new Button("Default", eventHandler, variant: ButtonVariant.Default).Loading()
                  | new Button("Destructive", eventHandler, variant: ButtonVariant.Destructive).Loading()
                  | new Button("Secondary", eventHandler, variant: ButtonVariant.Secondary).Loading()
                  | new Button("Outline", eventHandler, variant: ButtonVariant.Outline).Loading()
                  | new Button("Ghost", eventHandler, variant: ButtonVariant.Ghost).Loading()
                  | new Button("Link", eventHandler, variant: ButtonVariant.Link).Loading()
               )
               
               | Text.H2("Sizes")
               | (Layout.Grid().Columns(6)
                  | Text.Block("Default")
                  | Text.Block("Destructive")
                  | Text.Block("Secondary")
                  | Text.Block("Outline")
                  | Text.Block("Ghost")
                  | Text.Block("Link")

                  // Small
                  | new Button("Small", eventHandler, variant: ButtonVariant.Default).Small()
                  | new Button("Small", eventHandler, variant: ButtonVariant.Destructive).Small()
                  | new Button("Small", eventHandler, variant: ButtonVariant.Secondary).Small()
                  | new Button("Small", eventHandler, variant: ButtonVariant.Outline).Small()
                  | new Button("Small", eventHandler, variant: ButtonVariant.Ghost).Small()
                  | new Button("Small", eventHandler, variant: ButtonVariant.Link).Small()

                  // Default
                  | new Button("Default", eventHandler, variant: ButtonVariant.Default)
                  | new Button("Default", eventHandler, variant: ButtonVariant.Destructive)
                  | new Button("Default", eventHandler, variant: ButtonVariant.Secondary)
                  | new Button("Default", eventHandler, variant: ButtonVariant.Outline)
                  | new Button("Default", eventHandler, variant: ButtonVariant.Ghost)
                  | new Button("Default", eventHandler, variant: ButtonVariant.Link)

                  // Large
                  | new Button("Large", eventHandler, variant: ButtonVariant.Default).Large()
                  | new Button("Large", eventHandler, variant: ButtonVariant.Destructive).Large()
                  | new Button("Large", eventHandler, variant: ButtonVariant.Secondary).Large()
                  | new Button("Large", eventHandler, variant: ButtonVariant.Outline).Large()
                  | new Button("Large", eventHandler, variant: ButtonVariant.Ghost).Large()
                  | new Button("Large", eventHandler, variant: ButtonVariant.Link).Large()
               )
               
               | Text.H2("With Icons")
               | (Layout.Grid().Columns(6)
                  | Text.Block("Default")
                  | Text.Block("Destructive")
                  | Text.Block("Secondary")
                  | Text.Block("Outline")
                  | Text.Block("Ghost")
                  | Text.Block("Link")

                  // Icon Left
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Default, icon: Icons.MessageSquareX)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Destructive, icon: Icons.MessageSquareX)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Secondary, icon: Icons.MessageSquareX)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Outline, icon: Icons.MessageSquareX)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Ghost, icon: Icons.MessageSquareX)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Link, icon: Icons.MessageSquareX)

                  // Icon Right
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Default, icon: Icons.MessageSquareX).Icon(Icons.MessageSquareX, Align.Right)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Destructive, icon: Icons.MessageSquareX).Icon(Icons.MessageSquareX, Align.Right)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Secondary, icon: Icons.MessageSquareX).Icon(Icons.MessageSquareX, Align.Right)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Outline, icon: Icons.MessageSquareX).Icon(Icons.MessageSquareX, Align.Right)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Ghost, icon: Icons.MessageSquareX).Icon(Icons.MessageSquareX, Align.Right)
                  | new Button("Button With Icon", eventHandler, variant: ButtonVariant.Link, icon: Icons.MessageSquareX).Icon(Icons.MessageSquareX, Align.Right)
               )
               
               | Text.H2("Styling")
               | (Layout.Grid().Columns(6)
                  | Text.Block("Default")
                  | Text.Block("Destructive")
                  | Text.Block("Secondary")
                  | Text.Block("Outline")
                  | Text.Block("Ghost")
                  | Text.Block("Link")

                  // Rounded
                  | new Button("Rounded", eventHandler, variant: ButtonVariant.Default).BorderRadius(BorderRadius.Rounded)
                  | new Button("Rounded", eventHandler, variant: ButtonVariant.Destructive).BorderRadius(BorderRadius.Rounded)
                  | new Button("Rounded", eventHandler, variant: ButtonVariant.Secondary).BorderRadius(BorderRadius.Rounded)
                  | new Button("Rounded", eventHandler, variant: ButtonVariant.Outline).BorderRadius(BorderRadius.Rounded)
                  | new Button("Rounded", eventHandler, variant: ButtonVariant.Ghost).BorderRadius(BorderRadius.Rounded)
                  | new Button("Rounded", eventHandler, variant: ButtonVariant.Link).BorderRadius(BorderRadius.Rounded)

                  // Full
                  | new Button("Full", eventHandler, variant: ButtonVariant.Default).BorderRadius(BorderRadius.Full)
                  | new Button("Full", eventHandler, variant: ButtonVariant.Destructive).BorderRadius(BorderRadius.Full)
                  | new Button("Full", eventHandler, variant: ButtonVariant.Secondary).BorderRadius(BorderRadius.Full)
                  | new Button("Full", eventHandler, variant: ButtonVariant.Outline).BorderRadius(BorderRadius.Full)
                  | new Button("Full", eventHandler, variant: ButtonVariant.Ghost).BorderRadius(BorderRadius.Full)
                  | new Button("Full", eventHandler, variant: ButtonVariant.Link).BorderRadius(BorderRadius.Full)

                  // With Tooltip
                  | new Button("With Tooltip", eventHandler, variant: ButtonVariant.Default).Tooltip("This is a tooltip!")
                  | new Button("With Tooltip", eventHandler, variant: ButtonVariant.Destructive).Tooltip("This is a tooltip!")
                  | new Button("With Tooltip", eventHandler, variant: ButtonVariant.Secondary).Tooltip("This is a tooltip!")
                  | new Button("With Tooltip", eventHandler, variant: ButtonVariant.Outline).Tooltip("This is a tooltip!")
                  | new Button("With Tooltip", eventHandler, variant: ButtonVariant.Ghost).Tooltip("This is a tooltip!")
                  | new Button("With Tooltip", eventHandler, variant: ButtonVariant.Link).Tooltip("This is a tooltip!")
               )
               
               | Text.H2("Icon Only")
               | Layout.Horizontal(
                   Icons.MessageSquareX.ToButton(eventHandler),
                   Icons.Heart.ToButton(eventHandler, ButtonVariant.Destructive),
                   Icons.Star.ToButton(eventHandler, ButtonVariant.Outline)
               )
               
               | Text.H2("Interactive Demo")
               | Text.Literal(label.Value)
            ;
    }
}