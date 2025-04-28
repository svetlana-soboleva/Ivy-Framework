using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon:Icons.PaintBucket, path:["Widgets", "Inputs"])]
public class ColorInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        var colorState = this.UseState("#ff0000");
        return Layout.Horizontal(
            colorState.ToColorInput(),
            colorState
        );
    }
}