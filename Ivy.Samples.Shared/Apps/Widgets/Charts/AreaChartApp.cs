using Ivy.Charts;
using Ivy.Shared;
using System.Linq.Expressions;
using Ivy.Views.Charts;

namespace Ivy.Samples.Shared.Apps.Widgets.Charts;

[App(icon: Icons.ChartArea, searchHints: ["visualization", "graph", "analytics", "data", "trends", "statistics"])]
public class AreaChartApp : ViewBase
{
    public override object? Build()
    {
        return Layout.Grid().Columns(3)
            | new AreaChart0View()
            | new AreaChart1View()
            | new AreaChart2View()
            | new AreaChart3View()
            | new AreaChart4View()
            | new AreaChart5View()
            | new AreaChart6View()
            | new AreaChart7View()
            | new AreaChart8View()
            | new AreaChart9View()
        ;
    }
}

public class AreaChart0View : ViewBase
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

        return new Card().Title("Basic Area Chart")
            | data.ToAreaChart()
                .Dimension("Month", e => e.Month)
                .Measure("Desktop", e => e.Sum(f => f.Desktop))
                .Measure("Mobile", e => e.Sum(f => f.Mobile))
        ;
    }
}

public class AreaChart1View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Desktop = 186, Mobile = 80, Tablet = 45 },
            new { Month = "Feb", Desktop = 305, Mobile = 200, Tablet = 65 },
            new { Month = "Mar", Desktop = 237, Mobile = 120, Tablet = 85 },
            new { Month = "Apr", Desktop = 73, Mobile = 190, Tablet = 55 },
            new { Month = "May", Desktop = 209, Mobile = 130, Tablet = 75 },
            new { Month = "Jun", Desktop = 214, Mobile = 140, Tablet = 95 },
        };

        return new Card().Title("Multi-Series Area Chart")
            | data.ToAreaChart()
                .Dimension("Month", e => e.Month)
                .Measure("Desktop", e => e.Sum(f => f.Desktop))
                .Measure("Mobile", e => e.Sum(f => f.Mobile))
                .Measure("Tablet", e => e.Sum(f => f.Tablet))
        ;
    }
}

public class AreaChart2View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Revenue = 18600, Expenses = 12000 },
            new { Month = "Feb", Revenue = 30500, Expenses = 18000 },
            new { Month = "Mar", Revenue = 23700, Expenses = 15000 },
            new { Month = "Apr", Revenue = 7300, Expenses = 22000 },
            new { Month = "May", Revenue = 20900, Expenses = 16000 },
            new { Month = "Jun", Revenue = 21400, Expenses = 14000 },
        };

        return new Card().Title("Financial Overview")
            | data.ToAreaChart()
                .Dimension("Month", e => e.Month)
                .Measure("Revenue", e => e.Sum(f => f.Revenue))
                .Measure("Expenses", e => e.Sum(f => f.Expenses))
        ;
    }
}

public class AreaChart3View : ViewBase
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

        return new Card().Title("Product Sales by Quarter")
            | data.ToAreaChart()
                .Dimension("Quarter", e => e.Quarter)
                .Measure("ProductA", e => e.Sum(f => f.ProductA))
                .Measure("ProductB", e => e.Sum(f => f.ProductB))
                .Measure("ProductC", e => e.Sum(f => f.ProductC))
        ;
    }
}

public class AreaChart4View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Year = "2020", North = 150, South = 120, East = 180, West = 90 },
            new { Year = "2021", North = 180, South = 140, East = 200, West = 110 },
            new { Year = "2022", North = 220, South = 160, East = 240, West = 130 },
            new { Year = "2023", North = 250, South = 180, East = 280, West = 150 },
            new { Year = "2024", North = 280, South = 200, East = 320, West = 170 },
        };

        return new Card().Title("Regional Growth Trends")
            | data.ToAreaChart()
                .Dimension("Year", e => e.Year)
                .Measure("North", e => e.Sum(f => f.North))
                .Measure("South", e => e.Sum(f => f.South))
                .Measure("East", e => e.Sum(f => f.East))
                .Measure("West", e => e.Sum(f => f.West))
        ;
    }
}

public class AreaChart5View : ViewBase
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

        return new Card().Title("Website Analytics")
            | data.ToAreaChart()
                .Dimension("Month", e => e.Month)
                .Measure("Users", e => e.Sum(f => f.Users))
                .Measure("Sessions", e => e.Sum(f => f.Sessions))
                .Measure("Conversions", e => e.Sum(f => f.Conversions))
        ;
    }
}

public class AreaChart6View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Week = "Week 1", CPU = 45, Memory = 60, Disk = 30, Network = 25 },
            new { Week = "Week 2", CPU = 52, Memory = 65, Disk = 35, Network = 30 },
            new { Week = "Week 3", CPU = 48, Memory = 58, Disk = 32, Network = 28 },
            new { Week = "Week 4", CPU = 55, Memory = 70, Disk = 40, Network = 35 },
            new { Week = "Week 5", CPU = 60, Memory = 75, Disk = 45, Network = 40 },
            new { Week = "Week 6", CPU = 58, Memory = 72, Disk = 42, Network = 38 },
        };

        return new Card().Title("System Performance Metrics")
            | data.ToAreaChart()
                .Dimension("Week", e => e.Week)
                .Measure("CPU Usage (%)", e => e.Sum(f => f.CPU))
                .Measure("Memory Usage (%)", e => e.Sum(f => f.Memory))
                .Measure("Disk Usage (%)", e => e.Sum(f => f.Disk))
                .Measure("Network Usage (%)", e => e.Sum(f => f.Network))
        ;
    }
}

public class AreaChart7View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Temperature = 15, Humidity = 45, Pressure = 1013, WindSpeed = 12 },
            new { Month = "Feb", Temperature = 18, Humidity = 50, Pressure = 1015, WindSpeed = 15 },
            new { Month = "Mar", Temperature = 22, Humidity = 55, Pressure = 1012, WindSpeed = 18 },
            new { Month = "Apr", Temperature = 25, Humidity = 60, Pressure = 1010, WindSpeed = 20 },
            new { Month = "May", Temperature = 28, Humidity = 65, Pressure = 1008, WindSpeed = 22 },
            new { Month = "Jun", Temperature = 32, Humidity = 70, Pressure = 1005, WindSpeed = 25 },
            new { Month = "Jul", Temperature = 35, Humidity = 75, Pressure = 1003, WindSpeed = 28 },
            new { Month = "Aug", Temperature = 33, Humidity = 72, Pressure = 1006, WindSpeed = 26 },
            new { Month = "Sep", Temperature = 29, Humidity = 68, Pressure = 1009, WindSpeed = 23 },
            new { Month = "Oct", Temperature = 24, Humidity = 62, Pressure = 1011, WindSpeed = 19 },
            new { Month = "Nov", Temperature = 19, Humidity = 55, Pressure = 1014, WindSpeed = 16 },
            new { Month = "Dec", Temperature = 16, Humidity = 48, Pressure = 1016, WindSpeed = 13 },
        };

        return new Card().Title("Weather Data Trends (Annual)")
            | data.ToAreaChart()
                .Dimension("Month", e => e.Month)
                .Measure("Temperature (°C)", e => e.Sum(f => f.Temperature))
                .Measure("Humidity (%)", e => e.Sum(f => f.Humidity))
                .Measure("Pressure (hPa)", e => e.Sum(f => f.Pressure))
                .Measure("Wind Speed (km/h)", e => e.Sum(f => f.WindSpeed))
        ;
    }
}

public class AreaChart8View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", High = 15, Low = 5, Average = 10 },
            new { Month = "Feb", High = 18, Low = 7, Average = 12 },
            new { Month = "Mar", High = 22, Low = 10, Average = 16 },
            new { Month = "Apr", High = 25, Low = 12, Average = 18 },
            new { Month = "May", High = 28, Low = 15, Average = 21 },
            new { Month = "Jun", High = 32, Low = 18, Average = 25 },
            new { Month = "Jul", High = 35, Low = 20, Average = 27 },
            new { Month = "Aug", High = 33, Low = 19, Average = 26 },
            new { Month = "Sep", High = 29, Low = 16, Average = 22 },
            new { Month = "Oct", High = 24, Low = 12, Average = 18 },
            new { Month = "Nov", High = 19, Low = 8, Average = 13 },
            new { Month = "Dec", High = 16, Low = 6, Average = 11 },
        };

        return new Card().Title("Temperature Trends (°C)")
            | data.ToAreaChart()
                .Dimension("Month", e => e.Month)
                .Measure("High", e => e.Sum(f => f.High))
                .Measure("Average", e => e.Sum(f => f.Average))
                .Measure("Low", e => e.Sum(f => f.Low))
        ;
    }
}

public class AreaChart9View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Quarter = "Q1 2023", CompanyA = 35, CompanyB = 28, CompanyC = 22, Others = 15 },
            new { Quarter = "Q2 2023", CompanyA = 38, CompanyB = 25, CompanyC = 24, Others = 13 },
            new { Quarter = "Q3 2023", CompanyA = 42, CompanyB = 23, CompanyC = 26, Others = 9 },
            new { Quarter = "Q4 2023", CompanyA = 45, CompanyB = 20, CompanyC = 28, Others = 7 },
            new { Quarter = "Q1 2024", CompanyA = 48, CompanyB = 18, CompanyC = 30, Others = 4 },
            new { Quarter = "Q2 2024", CompanyA = 50, CompanyB = 16, CompanyC = 32, Others = 2 },
        };

        return new Card().Title("Market Share Analysis (%)")
            | data.ToAreaChart()
                .Dimension("Quarter", e => e.Quarter)
                .Measure("CompanyA", e => e.Sum(f => f.CompanyA))
                .Measure("CompanyB", e => e.Sum(f => f.CompanyB))
                .Measure("CompanyC", e => e.Sum(f => f.CompanyC))
                .Measure("Others", e => e.Sum(f => f.Others))
        ;
    }
}