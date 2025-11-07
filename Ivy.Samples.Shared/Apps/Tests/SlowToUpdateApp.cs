namespace Ivy.Samples.Shared.Apps.Tests;

[App()]
public class SlowToUpdateApp : ViewBase
{
    public override object? Build()
    {
        var loading = UseState(false);

        async ValueTask OnClick()
        {
            loading.Set(true);
            // Simulate a slow update
            await Task.Delay(10000);
            loading.Set(false);
        }

        return new Button("Foo").HandleClick(OnClick).Loading(loading);
    }
}