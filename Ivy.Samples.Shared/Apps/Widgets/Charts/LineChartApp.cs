﻿using Ivy.Charts;
using Ivy.Shared;
using Ivy.Views.Charts;

namespace Ivy.Samples.Shared.Apps.Widgets.Charts;

[App(icon: Icons.ChartLine, searchHints: ["visualization", "graph", "analytics", "data", "trends", "statistics"])]
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
            | new LineChart6View()
            | new LineChart7View()
            | new LineChart8View()
            | new LineChart9View()
            | new LineChart10View()
            | new LineChart11View()
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
            | data.ToLineChart(style: LineChartStyles.Default)
                .Dimension("Quarter", e => e.Quarter)
                .Measure("ProductA", e => e.Sum(f => f.ProductA))
                .Measure("ProductB", e => e.Sum(f => f.ProductB))
                .Measure("ProductC", e => e.Sum(f => f.ProductC))
        ;
    }
}

public class LineChart4View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Users = 1200, Sessions = 1400, Conversions = 1100 },
            new { Month = "Feb", Users = 1400, Sessions = 1600, Conversions = 1300 },
            new { Month = "Mar", Users = 1600, Sessions = 1800, Conversions = 1500 },
            new { Month = "Apr", Users = 1800, Sessions = 2000, Conversions = 1700 },
            new { Month = "May", Users = 2000, Sessions = 2200, Conversions = 1900 },
            new { Month = "Jun", Users = 2200, Sessions = 2400, Conversions = 2100 },
        };

        return new Card().Title("Website Analytics (Step Lines)")
            | data.ToLineChart(style: LineChartStyles.Default)
                .Dimension("Month", e => e.Month)
                .Measure("Users", e => e.Sum(f => f.Users))
                .Measure("Sessions", e => e.Sum(f => f.Sessions))
                .Measure("Conversions", e => e.Sum(f => f.Conversions))
        ;
    }
}

public class LineChart5View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Week = "Week 1", Temperature = 22, Humidity = 65, Pressure = 25 },
            new { Week = "Week 2", Temperature = 24, Humidity = 60, Pressure = 28 },
            new { Week = "Week 3", Temperature = 26, Humidity = 55, Pressure = 30 },
            new { Week = "Week 4", Temperature = 28, Humidity = 50, Pressure = 32 },
            new { Week = "Week 5", Temperature = 30, Humidity = 45, Pressure = 35 },
            new { Week = "Week 6", Temperature = 32, Humidity = 40, Pressure = 38 },
        };

        return new Card().Title("Weather Monitoring (Mixed Styles)")
            | data.ToLineChart(style: LineChartStyles.Default)
                .Dimension("Week", e => e.Week)
                .Measure("Temperature", e => e.Sum(f => f.Temperature))
                .Measure("Humidity", e => e.Sum(f => f.Humidity))
                .Measure("Pressure", e => e.Sum(f => f.Pressure))
        ;
    }
}

public class LineChart6View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Period = "Start", Value = 0, Negative = -1000, Large = 999999999 },
            new { Period = "Mid", Value = 50, Negative = -500, Large = 500000000 },
            new { Period = "End", Value = 100, Negative = 0, Large = 1 },
        };

        return new Card().Title("Extreme Values Test (Zero, Negative, Very Large)")
            | data.ToLineChart(style: LineChartStyles.Default)
                .Dimension("Period", e => e.Period)
                .Measure("Normal", e => e.Sum(f => f.Value))
                .Measure("Negative", e => e.Sum(f => f.Negative))
                .Measure("Very Large", e => e.Sum(f => f.Large))
        ;
    }
}

public class LineChart7View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Day = "Mon", Sales = 100, Marketing = 0, Support = 50 },
            new { Day = "Tue", Sales = 0, Marketing = 75, Support = 0 },
            new { Day = "Wed", Sales = 150, Marketing = 0, Support = 0 },
            new { Day = "Thu", Sales = 0, Marketing = 0, Support = 200 },
            new { Day = "Fri", Sales = 200, Marketing = 100, Support = 0 },
        };

        return new Card().Title("Sparse Data Test (Many Zero Values)")
            | data.ToLineChart(style: LineChartStyles.Dashboard)
                .Dimension("Day", e => e.Day)
                .Measure("Sales", e => e.Sum(f => f.Sales))
                .Measure("Marketing", e => e.Sum(f => f.Marketing))
                .Measure("Support", e => e.Sum(f => f.Support))
        ;
    }
}

public class LineChart8View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Point = "Single", Value = 42 },
        };

        return new Card().Title("Single Data Point Test")
            | data.ToLineChart(style: LineChartStyles.Custom)
                .Dimension("Point", e => e.Point)
                .Measure("Value", e => e.Sum(f => f.Value))
        ;
    }
}

public class LineChart9View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Label = "Very Long Label Name That Might Cause Issues", Value = 100 },
            new { Label = "Another Extremely Long Label That Could Break Layout", Value = 200 },
            new { Label = "Short", Value = 50 },
            new { Label = "Special Chars: !@#$%^&*()", Value = 75 },
            new { Label = "Unicode: 中文 🚀 αβγ", Value = 125 },
        };

        return new Card().Title("Long Labels & Special Characters Test")
            | data.ToLineChart(style: LineChartStyles.Default)
                .Dimension("Label", e => e.Label)
                .Measure("Value", e => e.Sum(f => f.Value))
        ;
    }
}

public class LineChart10View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Time = "T1", Flat = 100, Identical = 50, Same = 50 },
            new { Time = "T2", Flat = 100, Identical = 50, Same = 50 },
            new { Time = "T3", Flat = 100, Identical = 50, Same = 50 },
            new { Time = "T4", Flat = 100, Identical = 50, Same = 50 },
            new { Time = "T5", Flat = 100, Identical = 50, Same = 50 },
        };

        return new Card().Title("Flat Lines Test (Identical Values)")
            | data.ToLineChart(style: LineChartStyles.Dashboard)
                .Dimension("Time", e => e.Time)
                .Measure("Flat Line", e => e.Sum(f => f.Flat))
                .Measure("Identical", e => e.Sum(f => f.Identical))
                .Measure("Same Value", e => e.Sum(f => f.Same))
        ;
    }
}

public class LineChart11View : ViewBase
{
    public override object? Build()
    {
        var data = new[]
        {
            new { Scale = "Micro", Tiny = 0.001, Small = 1, Medium = 1000, Large = 1000000 },
            new { Scale = "Small", Tiny = 0.01, Small = 10, Medium = 10000, Large = 10000000 },
            new { Scale = "Large", Tiny = 0.1, Small = 100, Medium = 100000, Large = 100000000 },
        };

        return new Card().Title("Mixed Scales Test (Very Different Ranges)")
            | data.ToLineChart(style: LineChartStyles.Custom)
                .Dimension("Scale", e => e.Scale)
                .Measure("Tiny (0.001-0.1)", e => e.Sum(f => f.Tiny))
                .Measure("Small (1-100)", e => e.Sum(f => f.Small))
                .Measure("Medium (1K-100K)", e => e.Sum(f => f.Medium))
                .Measure("Large (1M-100M)", e => e.Sum(f => f.Large))
        ;
    }
}
