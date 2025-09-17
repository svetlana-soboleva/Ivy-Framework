using Ivy.Shared;
using Ivy.Views.Blades;

namespace Ivy.Samples.Shared.Apps.Concepts;

[App(icon: Icons.PanelLeft)]
public class BladesApp : SampleBase
{
    protected override object? BuildSample()
    {
        return this.UseBlades(() => new RootView("A"), "Blade 0");
    }
}

public class RootView(string someId) : ViewBase
{
    public override object? Build()
    {
        var bladeController = this.UseContext<IBladeController>();
        var index = bladeController.GetIndex(this);

        void OnClick(Event<Button> @event)
        {
            bladeController.Push(this, new RootView(@event.Sender.Tag?.ToString() ?? "?"), $"Blade {index + 1}");
        }

        void OnClickWithError(Event<Button> @event)
        {
            bladeController.Push(this, new BladeWithError(), "Blade With Error");
        }

        void OnClickWideTable(Event<Button> @event)
        {
            bladeController.Push(this, new WideTableBlade(), "Wide Table");
        }

        return Layout.Vertical(
            $"This is blade {index}",
            DateTime.Now.Ticks,
            someId,
            new Button("Push A", OnClick).Tag("A"),
            new Button("Push B", OnClick).Tag("B"),
            new Button("Push C", OnClick).Tag("C"),
            new Button("Blade With Error", OnClickWithError),
            new Button("Wide Table", OnClickWideTable)
        );
    }
}

public class BladeWithError : ViewBase
{
    public override object? Build()
    {
        throw new InvalidOperationException("This is a test error in a blade.");
    }
}

public class WideTableBlade : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical(
            Text.Literal("This table has extremely long column headers to test blade width adaptation:"),
            new Table(
                new TableRow(
                    new TableCell("Very Long Column Name That Should Cause The Blade To Expand Significantly").IsHeader(),
                    new TableCell("Another Extremely Long Column Header Name For Testing Purposes").IsHeader(),
                    new TableCell("Super Long Descriptive Column Name That Explains Everything In Detail").IsHeader(),
                    new TableCell("Ultra Wide Column Header With Lots Of Words And Characters").IsHeader()
                )
                { IsHeader = true },
                new TableRow(
                    new TableCell("Data 1"),
                    new TableCell("Value A"),
                    new TableCell("Item X"),
                    new TableCell("Result 1")
                ),
                new TableRow(
                    new TableCell("Data 2"),
                    new TableCell("Value B"),
                    new TableCell("Item Y"),
                    new TableCell("Result 2")
                ),
                new TableRow(
                    new TableCell("Data 3"),
                    new TableCell("Value C"),
                    new TableCell("Item Z"),
                    new TableCell("Result 3")
                )
            )
        );
    }
}