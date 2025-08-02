using Ivy.Shared;

namespace Ivy.Samples.Apps.Tests;

[App(icon: Icons.Code)]
public class ShouldFailWithAnalyser : ViewBase
{
    public override object? Build()
    {
        // Direct UseState call that should trigger analyzer error
        var handler = (Event<Button> e) =>
        {
            UseState(false);
        };
        
        // Also test inside a local function
        void LocalFunction()
        {
            UseState(42);
        }
        
        // This code is intentionally incorrect to demonstrate the analyzer's functionality.
        return new Button().HandleClick(handler);
    }

}
