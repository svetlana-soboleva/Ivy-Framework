using Ivy.Shared;

namespace Ivy.Samples.Apps.Tests;

[App(icon: Icons.Code)]
public class ShouldFailWithAnalyser : ViewBase
{
    public override object? Build()
    {
        var handler = (Event<Button> e) =>
        {
            var s = UseState(false);
        };
        
        // This code is intentionally incorrect to demonstrate the analyzer's functionality.
        return new Button().HandleClick(handler);
    }

}
