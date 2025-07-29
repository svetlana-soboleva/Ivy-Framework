using Ivy.Shared;

namespace Ivy.Samples.Apps.Concepts;

[App(icon:Icons.Bug)]
public class ExceptionHandlingApp : ViewBase
{
    public override object? Build()
    {
        UseEffect(() => throw new Exception("this is an unhandled exception"));
        
        var button = new Button("Click me to throw an exception")
        {
            OnClick = _ => throw new Exception("this is an unhandled exception from a button click")
        };
        
        return button;
    }
}