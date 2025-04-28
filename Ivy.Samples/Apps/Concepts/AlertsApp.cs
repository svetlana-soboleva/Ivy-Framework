using Ivy.Shared;

namespace Ivy.Samples.Apps.Concepts;

[App(icon:Icons.Bell, path: ["Concepts"])]
public class AlertsApp : SampleBase
{
    protected override object? BuildSample()
    {
        var (alertView, alertResult, showAlert) = this.UseAlert();

        return Layout.Vertical(
            new Button("YesNoCancel", _ => showAlert("Hello, World!", "Alert Title", AlertButtonSet.YesNoCancel)),
            alertResult switch
            {
                AlertResult.Ok => "You clicked OK!",
                AlertResult.Cancel => "You clicked Cancel!",
                AlertResult.Yes => "You clicked Yes!",
                AlertResult.No => "You clicked No!",
                _ => "You didn't click anything!"
            },
            alertView
        );
    }
}