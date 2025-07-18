using Ivy.Charts;
using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Charts;

[App(icon: Icons.ChartLine)]
public class LineChartApp : ViewBase
{
    public override object? Build()
    {
        return Layout.Grid().Columns(3)
            | new LineChart0View()
            | new LineChart1View()
            | new LineChart2View()
            | new LineChart3View()
            | new LineChart4View()
            | new LineChart5View()
        ;
    }
}

public class LineChart0View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "January", Desktop = 186, Mobile = 100 },
            new { Month = "February", Desktop = 305, Mobile = 200 },
            new { Month = "March", Desktop = 237, Mobile = 300 },
            new { Month = "April", Desktop = 73, Mobile = 400 },
            new { Month = "May", Desktop = 209, Mobile = 30 },
            new { Month = "June", Desktop = 214, Mobile = 45 },
        };

        return new Card().Title("Basic Line Chart (Default Style)")
            | data.ToLineChart(style: LineChartStyles.Default)
                .Dimension("Month", e => e.Month)
                .Measure("Desktop", e => e.Sum(f => f.Desktop))
                .Measure("Mobile", e => e.Sum(f => f.Mobile))
        ;
    }
}

public class LineChart1View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Sales = 1200, Marketing = 800, Development = 600 },
            new { Month = "Feb", Sales = 1500, Marketing = 950, Development = 750 },
            new { Month = "Mar", Sales = 1800, Marketing = 1100, Development = 900 },
            new { Month = "Apr", Sales = 2100, Marketing = 1250, Development = 1050 },
            new { Month = "May", Sales = 2400, Marketing = 1400, Development = 1200 },
            new { Month = "Jun", Sales = 2700, Marketing = 1550, Development = 1350 },
        };

        return new Card().Title("Department Performance (Dashboard Style)")
            | data.ToLineChart(style: LineChartStyles.Dashboard)
                .Dimension("Month", e => e.Month)
                .Measure("Sales", e => e.Sum(f => f.Sales))
                .Measure("Marketing", e => e.Sum(f => f.Marketing))
                .Measure("Development", e => e.Sum(f => f.Development))
        ;
    }
}

public class LineChart2View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Year = "2020", Revenue = 50000, Profit = 15000 },
            new { Year = "2021", Revenue = 65000, Profit = 20000 },
            new { Year = "2022", Revenue = 80000, Profit = 25000 },
            new { Year = "2023", Revenue = 95000, Profit = 30000 },
            new { Year = "2024", Revenue = 110000, Profit = 35000 },
        };

        return new Card().Title("Financial Growth (Custom Style)")
            | data.ToLineChart(style: LineChartStyles.Custom)
                .Dimension("Year", e => e.Year)
                .Measure("Revenue", e => e.Sum(f => f.Revenue))
                .Measure("Profit", e => e.Sum(f => f.Profit))
        ;
    }
}

public class LineChart3View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Quarter = "Q1", ProductA = 1200, ProductB = 800, ProductC = 600 },
            new { Quarter = "Q2", ProductA = 1500, ProductB = 950, ProductC = 750 },
            new { Quarter = "Q3", ProductA = 1800, ProductB = 1100, ProductC = 900 },
            new { Quarter = "Q4", ProductA = 2100, ProductB = 1250, ProductC = 1050 },
        };

        return new Card().Title("Product Sales Trends (Rainbow Colors)")
            | new LineChart(data)
                .ColorScheme(ColorScheme.Rainbow)
                .Line("ProductA")
                .Line("ProductB")
                .Line("ProductC")
                .CartesianGrid()
                .Tooltip()
                .XAxis("Quarter")
                .YAxis()
                .Legend()
        ;
    }
}

public class LineChart4View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Users = 1200, Sessions = 3500, Conversions = 180 },
            new { Month = "Feb", Users = 1400, Sessions = 4200, Conversions = 220 },
            new { Month = "Mar", Users = 1600, Sessions = 4800, Conversions = 260 },
            new { Month = "Apr", Users = 1800, Sessions = 5400, Conversions = 300 },
            new { Month = "May", Users = 2000, Sessions = 6000, Conversions = 340 },
            new { Month = "Jun", Users = 2200, Sessions = 6600, Conversions = 380 },
        };

        return new Card().Title("Website Analytics (Step Lines)")
            | new LineChart(data)
                .Line(new Line("Users").CurveType(CurveTypes.Step).StrokeWidth(3))
                .Line(new Line("Sessions").CurveType(CurveTypes.Step).StrokeWidth(3))
                .Line(new Line("Conversions").CurveType(CurveTypes.Step).StrokeWidth(3))
                .CartesianGrid(new CartesianGrid().Horizontal().Vertical())
                .Tooltip()
                .XAxis("Month")
                .YAxis("Users")
                .Legend()
        ;
    }
}

public class LineChart5View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Week = "Week 1", Temperature = 22, Humidity = 65, Pressure = 1013 },
            new { Week = "Week 2", Temperature = 24, Humidity = 60, Pressure = 1015 },
            new { Week = "Week 3", Temperature = 26, Humidity = 55, Pressure = 1012 },
            new { Week = "Week 4", Temperature = 28, Humidity = 50, Pressure = 1010 },
            new { Week = "Week 5", Temperature = 30, Humidity = 45, Pressure = 1008 },
            new { Week = "Week 6", Temperature = 32, Humidity = 40, Pressure = 1005 },
        };

        return new Card().Title("Weather Monitoring (Mixed Styles)")
            | new LineChart(data)
                .Line(new Line("Temperature").CurveType(CurveTypes.Monotone).StrokeWidth(4).Stroke(Colors.Blue))
                .Line(new Line("Humidity").CurveType(CurveTypes.Linear).StrokeWidth(2).Stroke(Colors.Green).StrokeDashArray("5,5"))
                .Line(new Line("Pressure").CurveType(CurveTypes.Basis).StrokeWidth(3).Stroke(Colors.Red))
                .CartesianGrid(new CartesianGrid().Horizontal())
                .Tooltip(new Ivy.Charts.Tooltip().Animated(true))
                .XAxis("Week")
                .YAxis("Temperature")
                .Legend()
        ;
    }
}