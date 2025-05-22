# PieChart

Pie charts represent parts of a whole. Each slice is drawn from the provided data.

```csharp demo
var data = new[]
{
    new { Month = "January", Desktop = 186, Mobile = 100 },
    new { Month = "February", Desktop = 305, Mobile = 200 },
    new { Month = "March", Desktop = 237, Mobile = 300 },
};

new PieChart(data)
    .ColorScheme(ColorScheme.Rainbow)
    .Pie("Mobile", "Month")
    .Tooltip()
    .Legend();
```
