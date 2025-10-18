using Ivy.Shared;

namespace Ivy.Samples.Shared.Apps.Concepts;

[App(icon: Icons.BellRing, searchHints: ["notifications", "messages", "feedback", "snackbar", "alerts", "popup"])]
public class ToastApp : SampleBase
{
    protected override object? BuildSample()
    {
        var client = UseService<IClientProvider>();

        return Layout.Vertical(
            new Button("Toast", _ => client.Toast("Hello World")),
            new Button("Toast with Title", _ => client.Toast("Hello World", "Title"))
        );
    }
}