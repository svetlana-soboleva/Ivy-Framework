using Ivy.Shared;

namespace Ivy.Samples.Apps.Concepts;

[App(icon:Icons.Command, path: ["Concepts"])]
public class ClientExtensionsApp : SampleBase
{
    protected override object? BuildSample()
    {
        var client = UseService<IClientProvider>();
        
        return Layout.Vertical(
            new Button("OpenUrl", _ => client.OpenUrl("https://google.com"))
        );
    }
}