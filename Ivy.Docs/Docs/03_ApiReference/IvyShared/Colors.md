# Colors

Ivy provides a set of predefined colors.

```csharp demo-tabs
public class ColorsView : ViewBase
{
    public override object? Build()
    {
        //Get all colors:
        Colors[] colors = (Colors[])Enum.GetValues(typeof(Colors));

        var colorView = Layout.Vertical(
            colors.Select(color =>
                new Box(color.ToString())
                    .Width(Size.Auto())
                    .Height(10)
                    .Color(color).BorderRadius(BorderRadius.Rounded)
                    .Padding(3)
            )
        );
        
        return colorView;
    }
}
```