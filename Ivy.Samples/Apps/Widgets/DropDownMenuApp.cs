using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets;

[App(icon:Icons.Menu)]
public class DropDownMenuApp : SampleBase
{
    protected override object? BuildSample()
    {
        var client = this.UseService<IClientProvider>();
        return new DropDownMenu(@evt =>
               {
                   client.Toast(@evt.Value?.ToString() ?? throw new Exception("Missing value in event."));
               }, new Button("Open Menu")).Header(
                    Layout.Vertical().Gap(0) | Text.Muted("Signed in as") | Text.Small("niels@bosmainteractive.se")
                   )
                   .Left()
                    | MenuItem.Separator()
                    | MenuItem.Default("Ivy Github").Icon(Icons.Github)
                    | MenuItem.Default("Theme")
                        .Icon(Icons.SunMoon)
                        .Children(
                            MenuItem.Checkbox("Dark").Checked().Icon(Icons.Moon),
                            MenuItem.Checkbox("Light").Icon(Icons.Sun),
                            MenuItem.Checkbox("System").Icon(Icons.SunMoon)
                        )
                    | MenuItem.Default("Logout").Icon(Icons.LogOut);
    }
}