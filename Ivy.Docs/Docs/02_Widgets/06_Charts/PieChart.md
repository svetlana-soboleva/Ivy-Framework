# PieChart

Pie charts represent parts of a whole. Each slice is drawn from the provided data.

```csharp demo-below

public class PieChartDemo : ViewBase 
{    
    
    public override object? Build()
    {    
        //Sales data
        var data = new[]
        {   
            new { Month = "January", Desktop = 186, Mobile = 100 },
            new { Month = "February", Desktop = 305, Mobile = 200 },
            new { Month = "March", Desktop = 237, Mobile = 300 },
        };  
         return Layout.Vertical()
            | new PieChart(data)
                .ColorScheme(ColorScheme.Rainbow)
                .Pie("Mobile", "Month")
                .Tooltip()
                .Legend()
          | new PieChart(data)
                .ColorScheme(ColorScheme.Default)
                .Pie("Desktop", "Month")
                .Tooltip()
                .Legend(new Legend()
                            .Align(Legend.Alignments.Right)
                            .VerticalAlign(Legend.VerticalAlignments.Bottom));
    }
}    
```

### Browser Market Share

```csharp demo-tabs
public class BrowserStatsPie : ViewBase 
{
    public override object? Build()
    {
        var years = new string[]{"2019","2025"};
        var year = this.UseState(years[0]);
        
        var map = new Dictionary<string,object>();
        var browserSharesYear1 = new []
        {
             //Rounded 
            new { Browser = "Chrome", Company = "Google", Share = 30},
            new { Browser = "Safari", Company = "Apple", Share = 16},
            new { Browser = "Edge", Company= "Microsoft", Share = 52},
            new { Browser = "Firefox", Company= "Mozilla" Share = 2}
        };    
        var browserSharesYear2 = new []
        {
            //Rounded 
            new { Browser = "Chrome", Company = "Google", Share = 70},
            new { Browser = "Safari", Company = "Apple", Share = 16},
            new { Browser = "Edge", Company = "Microsoft", Share = 12},
            new { Browser = "Firefox", Company = "Mozilla", Share = 2}
        };
        map.Add("2019", browserSharesYear1);
        map.Add("2025", browserSharesYear2);
        var yearInput = year.ToSelectInput(years.ToOptions())
                                   .Width(15);
        return Layout.Vertical()
            | H3("Browser Market share")
            | Text.Large("Select Year")
            | yearInput
            | new PieChart(map[year.Value])
                 .Pie(new Pie("Share","Browser")
                         .OuterRadius(150)
                         .InnerRadius(90))
                 .Tooltip(new Ivy.Charts.Tooltip().Animated(true))
                 .Legend(new Legend().IconSize(35));
    }    
}

```

