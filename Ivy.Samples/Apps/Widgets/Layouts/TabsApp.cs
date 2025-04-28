using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Layouts;

[App(icon:Icons.LayoutTemplate, path:["Widgets", "Layouts"])]
public class TabsApp : SampleBase
{
    protected override object? BuildSample()
    {
        var selectedIndex = this.UseState(0);
        var client = UseService<IClientProvider>();
        
        void HandleEvent(Event<TabsLayout> @event)
        {
            client.Toast(@event.EventName);
        }
        
        void OnTabSelect(Event<TabsLayout, int> @event)
        {
            selectedIndex.Set(@event.Value);
        }

        var defaultVariant = new TabsLayout(OnTabSelect, HandleEvent, HandleEvent, selectedIndex.Value,
            new Tab("Customers", "Customers").Icon(Icons.User).Badge("10"),
            new Tab("Orders", "Orders").Icon(Icons.DollarSign).Badge("0"),
            new Tab("Settings", "Settings").Icon(Icons.Settings).Badge("999")
        );

        var discreetVariant = Layout.Tabs(
            new Tab("Demo", new Button("Hello")),
            new Tab("Code", new Code("new Button(\"Hello\")"))
        ).Height(Size.Fit()).Padding(4);

        return discreetVariant;

        // return Layout.Vertical()
        //     | defaultVariant
        //     | discreetVariant;

    }
}