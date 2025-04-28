using Ivy.Shared;

namespace Ivy.Samples.Apps.Concepts;

public record LinksAppArgs(string Foo, int Bar);

[App(icon:Icons.PanelLeft)]
public class LinksApp : SampleBase
{
    protected override object? BuildSample()
    {
        LinksAppArgs? args = this.UseArgs<LinksAppArgs>();
        
        if(args != null)
        {
            return args;
        }
        
        return new Button("Go").HandleClick(() =>
        {
            this.OpenTab<LinksApp>(new LinksAppArgs("Niels", 123));
        });

    }
}

