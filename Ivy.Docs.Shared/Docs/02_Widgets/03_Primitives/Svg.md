# Svg

The `Svg` widget renders scalable vector graphics directly in your app. SVGs are resolution-independent and perfect for icons, illustrations, charts, and other graphics that need to scale without losing quality.

## Basic Usage

The simplest way to create an SVG is to pass SVG markup as a string:

```csharp demo-tabs 
public class SimpleSvgView : ViewBase
{
    public override object? Build()
    {
        var simpleCircle = """
            <svg width="100" height="100">
                <circle cx="50" cy="50" r="40" fill="blue" />
            </svg>
            """;
            
        return new Svg(simpleCircle);
    }
}
```

## Basic Shapes

Create simple geometric shapes:

```csharp demo-tabs 
public class BasicShapesView : ViewBase
{
    public override object? Build()
    {
        var shapes = """
            <svg width="200" height="100" viewBox="0 0 200 100">
                <rect x="10" y="10" width="30" height="30" fill="red" />
                <circle cx="70" cy="25" r="15" fill="green" />
                <ellipse cx="120" cy="25" rx="20" ry="12" fill="blue" />
                <line x1="10" y1="60" x2="190" y2="60" stroke="orange" stroke-width="3" />
            </svg>
            """;
            
        return new Svg(shapes);
    }
}
```

More shape examples:

```csharp demo-tabs 
public class MoreShapesView : ViewBase
{
    public override object? Build()
    {
        var moreShapes = """
            <svg width="300" height="150" viewBox="0 0 300 150">
                <!-- Triangles -->
                <polygon points="20,120 40,80 60,120" fill="purple" />
                <polygon points="80,120 100,80 120,120" fill="teal" />
                
                <!-- Stars -->
                <polygon points="150,80 155,95 170,95 157,105 162,120 150,110 138,120 143,105 130,95 145,95" fill="gold" />
                
                <!-- Arcs -->
                <path d="M 200 50 A 25 25 0 0 1 250 100" stroke="brown" stroke-width="4" fill="none" />
                <path d="M 200 100 A 25 25 0 0 0 250 150" stroke="brown" stroke-width="4" fill="none" />
            </svg>
            """;
            
        return new Svg(moreShapes);
    }
}
```

## Simple Icons

Create basic icons with minimal SVG:

```csharp demo-tabs 
public class SimpleIconsView : ViewBase
{
    public override object? Build()
    {
        var icons = new[]
        {
            // Plus icon - thicker and more visible
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <rect x="11" y="5" width="2" height="14" fill="black"/>
                <rect x="5" y="11" width="14" height="2" fill="black"/>
            </svg>
            """,
            
            // Check icon
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <polyline points="20,6 9,17 4,12" stroke="green" stroke-width="3" fill="none"/>
            </svg>
            """,
            
            // Heart icon
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z" fill="red"/>
            </svg>
            """
        };
        
        return Layout.Horizontal().Gap(4)
            | icons.Select(icon => new Svg(icon));
    }
}
```

Additional icon examples:

```csharp demo-tabs 
public class MoreIconsView : ViewBase
{
    public override object? Build()
    {
        var moreIcons = new[]
        {
            // Arrow right
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <polyline points="9,18 15,12 9,6" stroke="blue" stroke-width="3" fill="none"/>
            </svg>
            """,
            
            // Square with X
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <rect x="3" y="3" width="18" height="18" fill="none" stroke="red" stroke-width="2"/>
                <line x1="9" y1="9" x2="15" y2="15" stroke="red" stroke-width="2"/>
                <line x1="15" y1="9" x2="9" y2="15" stroke="red" stroke-width="2"/>
            </svg>
            """,
            
            // Circle with dot
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <circle cx="12" cy="12" r="10" fill="none" stroke="green" stroke-width="2"/>
                <circle cx="12" cy="12" r="3" fill="green"/>
            </svg>
            """
        };
        
        return Layout.Horizontal().Gap(4)
            | moreIcons.Select(icon => new Svg(icon));
    }
}
```

## Progress Bar

Create a simple horizontal progress bar:

```csharp demo-tabs 
public class ProgressBarView : ViewBase
{
    public override object? Build()
    {
        string GetProgressBar(int percentage)
        {
            return $"""
                <svg width="200" height="20" viewBox="0 0 200 20">
                    <rect width="200" height="20" fill="#e5e7eb" rx="10"/>
                    <rect width="{percentage * 2}" height="20" fill="#3b82f6" rx="10"/>
                    <text x="100" y="14" text-anchor="middle" font-size="12" fill="black">{percentage}%</text>
                </svg>
                """;
        }
         
        return Layout.Vertical().Gap(2)
            | new Svg(GetProgressBar(25))
            | new Svg(GetProgressBar(50))
            | new Svg(GetProgressBar(75));
    }
}
```

Different progress bar styles:

```csharp demo-tabs 
public class ProgressBarStylesView : ViewBase
{
    public override object? Build()
    {
        var styles = new[]
        {
            // Vertical progress bar
            """
            <svg width="30" height="100" viewBox="0 0 30 100">
                <rect x="5" y="10" width="20" height="80" fill="#e5e7eb" rx="10"/>
                <rect x="5" y="50" width="20" height="40" fill="#10b981" rx="10"/>
            </svg>
            """,
            
            // Striped progress bar
            """
            <svg width="200" height="20" viewBox="0 0 200 20">
                <rect width="200" height="20" fill="#e5e7eb" rx="10"/>
                <rect width="120" height="20" fill="#f59e0b" rx="10"/>
                <rect width="120" height="20" fill="url(#stripes)" rx="10"/>
                <defs>
                    <pattern id="stripes" patternUnits="userSpaceOnUse" width="10" height="20">
                        <line x1="0" y1="0" x2="0" y2="20" stroke="white" stroke-width="2" opacity="0.3"/>
                    </pattern>
                </defs>
            </svg>
            """,
            
            // Circular progress indicator
            """
            <svg width="60" height="60" viewBox="0 0 60 60">
                <circle cx="30" cy="30" r="25" fill="none" stroke="#e5e7eb" stroke-width="5"/>
                <circle cx="30" cy="30" r="25" fill="none" stroke="#8b5cf6" stroke-width="5" 
                        stroke-dasharray="157" stroke-dashoffset="78" transform="rotate(-90 30 30)"/>
            </svg>
            """
        };
        
        return Layout.Horizontal().Gap(4)
            | styles.Select(style => new Svg(style));
    }
}
```

## Simple Chart

Create a basic bar chart:

```csharp demo-tabs 
public class SimpleChartView : ViewBase
{
    public override object? Build()
    {
        var chart = """
            <svg width="300" height="150" viewBox="0 0 300 150">
                <rect x="20" y="120" width="40" height="30" fill="#3b82f6" />
                <rect x="70" y="100" width="40" height="50" fill="#ef4444" />
                <rect x="120" y="80" width="40" height="70" fill="#10b981" />
                <rect x="170" y="60" width="40" height="90" fill="#f59e0b" />
                <rect x="220" y="40" width="40" height="110" fill="#8b5cf6" />
            </svg>
            """;
            
        return new Svg(chart);
    }
}
```

More chart types:

```csharp demo-tabs 
public class MoreChartsView : ViewBase
{
    public override object? Build()
    {
        var charts = new[]
        {
            // Pie chart
            """
            <svg width="150" height="150" viewBox="0 0 150 150">
                <circle cx="75" cy="75" r="60" fill="none" stroke="#3b82f6" stroke-width="60" 
                        stroke-dasharray="113 226" transform="rotate(-90 75 75)"/>
                <circle cx="75" cy="75" r="60" fill="none" stroke="#ef4444" stroke-width="60" 
                        stroke-dasharray="113 113" transform="rotate(23 75 75)"/>
                <circle cx="75" cy="75" r="60" fill="none" stroke="#10b981" stroke-width="60" 
                        stroke-dasharray="226 113" transform="rotate(113 75 75)"/>
            </svg>
            """,
            
            // Line chart
            """
            <svg width="200" height="100" viewBox="0 0 200 100">
                <polyline points="20,80 60,60 100,40 140,70 180,20" 
                          fill="none" stroke="#3b82f6" stroke-width="3"/>
                <circle cx="20" cy="80" r="3" fill="#3b82f6"/>
                <circle cx="60" cy="60" r="3" fill="#3b82f6"/>
                <circle cx="100" cy="40" r="3" fill="#3b82f6"/>
                <circle cx="140" cy="70" r="3" fill="#3b82f6"/>
                <circle cx="180" cy="20" r="3" fill="#3b82f6"/>
            </svg>
            """,
            
            // Stacked bars
            """
            <svg width="200" height="100" viewBox="0 0 200 100">
                <rect x="20" y="60" width="30" height="20" fill="#3b82f6"/>
                <rect x="20" y="40" width="30" height="20" fill="#ef4444"/>
                <rect x="20" y="20" width="30" height="20" fill="#10b981"/>
                
                <rect x="60" y="50" width="30" height="30" fill="#3b82f6"/>
                <rect x="60" y="30" width="30" height="20" fill="#ef4444"/>
                <rect x="60" y="10" width="30" height="20" fill="#10b981"/>
            </svg>
            """
        };
        
        return Layout.Horizontal().Gap(4)
            | charts.Select(chart => new Svg(chart));
    }
}
```

## Properties

The `Svg` widget has the following properties:

- **Content** (string): The SVG markup content to render
- **Width** (Size): The width of the SVG container (defaults to Auto)
- **Height** (Size): The height of the SVG container (defaults to Auto)

## Best Practices

1. **Keep it simple**: Start with basic shapes and build up complexity gradually
2. **Use viewBox**: Include a `viewBox` attribute for responsive scaling
3. **Minimize markup**: Use the simplest SVG elements needed
4. **Test rendering**: Verify your SVG displays correctly in different browsers
5. **Optimize paths**: Use simple path data when possible

<WidgetDocs Type="Ivy.Svg" ExtensionTypes="Ivy.SvgExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Svg.cs"/>
