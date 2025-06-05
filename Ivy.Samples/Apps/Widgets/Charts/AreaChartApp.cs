using Ivy.Charts;
using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Charts;

[App(icon: Icons.ChartArea)]
public class AreaChartApp : ViewBase
{
    public override object? Build()
    {
        return Layout.Grid().Columns(3)
               | new AreaChart1View()
            ;
    }
}

public class AreaChart1View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Desktop = 186, Mobile = 80 },
            new { Month = "Feb", Desktop = 305, Mobile = 200 },
            new { Month = "Mar", Desktop = 237, Mobile = 120 },
            new { Month = "Apr", Desktop = 73, Mobile = 190 },
            new { Month = "May", Desktop = 209, Mobile = 130 },
            new { Month = "Jun", Desktop = 214, Mobile = 140 },
        };

        return new Card().Title("Area Chart")
               | new AreaChart(data)
                   .ColorScheme(ColorScheme.Default)
                   .Area(new Area("Mobile", 1).FillOpacity(0.5))
                   .Area(new Area("Desktop", 1).FillOpacity(0.5))
                   .CartesianGrid(new CartesianGrid().Horizontal())
                   .Tooltip()
                   .XAxis(new XAxis("Month").TickLine(false).AxisLine(false))
               ;
    }
}