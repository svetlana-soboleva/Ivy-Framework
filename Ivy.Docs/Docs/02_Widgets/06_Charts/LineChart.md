# LineChart

Line charts show trends over a period of time. The example below renders desktop
and mobile metrics.

```csharp demo
var data = new[]
{
    new { Month = "January", Desktop = 186, Mobile = 100 },
    new { Month = "February", Desktop = 305, Mobile = 200 },
    new { Month = "March", Desktop = 237, Mobile = 300 },
};

new LineChart(data)
    .ColorScheme(ColorScheme.Rainbow)
    .Line("Mobile")
    .Line("Desktop")
    .CartesianGrid()
    .Tooltip()
    .XAxis("Month")
    .YAxis("Desktop")
    .Legend();
```
