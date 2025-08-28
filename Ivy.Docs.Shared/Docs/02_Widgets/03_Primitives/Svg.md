# Svg

<Ingress>
Create beautiful, scalable vector graphics directly in your app with the `Svg` widget. Perfect for icons, illustrations, charts, and other graphics that need to scale without losing quality.
</Ingress>

The `Svg` widget renders scalable vector graphics directly in your app. SVGs are resolution-independent and perfect for icons, illustrations, charts, and other graphics that need to scale without losing quality.

## Basic Usage

The simplest way to create an SVG is to pass SVG markup as a string:

```csharp demo-below
public class SimpleSvgView : ViewBase
{
    public override object? Build()
    {
        var simpleCircle = """
            <svg width="100" height="100">
                <circle cx="50" cy="50" r="40" fill="green" />
            </svg>
            """;
            
        return new Svg(simpleCircle);
    }
}
```

<Callout Type="Tip">
The `Svg` widget has the following properties:
- Content (string): The SVG markup content to render
- Width (Size): The width of the SVG container (defaults to Auto)
- Height (Size): The height of the SVG container (defaults to Auto)
</Callout>

<WidgetDocs Type="Ivy.Svg" ExtensionTypes="Ivy.SvgExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Svg.cs"/>
