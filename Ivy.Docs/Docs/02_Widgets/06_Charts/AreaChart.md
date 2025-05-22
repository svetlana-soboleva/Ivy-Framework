# AreaChart

Area charts display quantitative data over time. Multiple series can be stacked
with different colors.

```csharp
var data = new[]
{
    new { Month = "Jan", Desktop = 186, Mobile = 80 },
    new { Month = "Feb", Desktop = 305, Mobile = 200 },
    new { Month = "Mar", Desktop = 237, Mobile = 120 },
};

new AreaChart(data)
    .ColorScheme(ColorScheme.Emerald)
    .Area(new Area("Mobile", 1).FillOpacity(0.5))
    .Area(new Area("Desktop", 1).FillOpacity(0.5))
    .CartesianGrid(new CartesianGrid().Horizontal())
    .Tooltip()
    .XAxis(new XAxis("Month").TickLine(false).AxisLine(false));
```
