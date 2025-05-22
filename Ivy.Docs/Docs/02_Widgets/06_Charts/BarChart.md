# BarChart

Bar charts compare values across categories. The sample below shows a stacked bar
chart with two series.

```csharp
var data = new[]
{
    new { Month = "Jan", Desktop = 186, Mobile = 100 },
    new { Month = "Feb", Desktop = 305, Mobile = 200 },
    new { Month = "Mar", Desktop = 237, Mobile = 300 },
};

new BarChart(data)
    .ColorScheme(ColorScheme.Emerald)
    .Bar(new Bar("Mobile", 1).Radius(0,8))
    .Bar(new Bar("Desktop", 1).Radius(8,0))
    .CartesianGrid(new CartesianGrid().Horizontal())
    .Tooltip()
    .XAxis(new XAxis("Month").TickLine(false).AxisLine(false))
    .Legend();
```
