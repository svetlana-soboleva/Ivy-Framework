# Svg

<Ingress>
Create beautiful, scalable vector graphics directly in your app with the `Svg` widget. Perfect for icons, illustrations, charts, and other graphics that need to scale without losing quality.
</Ingress>

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

### Basic Shapes

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
            
        var moreShapes = """
            <svg width="300" height="150" viewBox="0 0 300 150">
                <!-- Triangles -->
                <polygon points="20,120 40,80 60,120" fill="purple" />
                <polygon points="80,120 100,80 120,120" fill="teal" />
                
                <!-- Simple Star -->
                <polygon points="150,80 155,95 170,95 157,105 162,120 150,110 138,120 143,105 130,95 145,95" fill="gold" />
                
                <!-- Arcs -->
                <path d="M 200 50 A 25 25 0 0 1 250 100" stroke="brown" stroke-width="4" fill="none" />
                <path d="M 200 100 A 25 25 0 0 0 250 150" stroke="brown" stroke-width="4" fill="none" />
            </svg>
            """;
            
        var advancedShapes = """
            <svg width="300" height="100" viewBox="0 0 300 100">
                <!-- Hexagon -->
                <polygon points="25,50 35,30 55,30 65,50 55,70 35,70" fill="pink" />
                
                <!-- Diamond -->
                <polygon points="100,30 120,50 100,70 80,50" fill="cyan" />
                
                <!-- Cross -->
                <rect x="140" y="35" width="30" height="30" fill="lime" />
                <rect x="155" y="20" width="30" height="30" fill="lime" />
                
                <!-- Wave -->
                <path d="M 200 30 Q 220 10 240 30 T 280 30" stroke="navy" stroke-width="3" fill="none" />
                <path d="M 200 50 Q 220 30 240 50 T 280 50" stroke="navy" stroke-width="3" fill="none" />
            </svg>
            """;
            
        return Layout.Vertical().Gap(4)
            | new Svg(shapes)
            | new Svg(moreShapes)
            | new Svg(advancedShapes);
    }
}
```

<Callout Type="Tip">
The `Svg` widget has the following properties:
- Content (string): The SVG markup content to render
- Width (Size): The width of the SVG container (defaults to Auto)
- Height (Size): The height of the SVG container (defaults to Auto)
</Callout>

### Simple Icons

Create basic icons with minimal SVG:

```csharp demo-tabs
public class SimpleIconsView : ViewBase
{
    public override object? Build()
    {
        var icons = new[]
        {
            // Plus icon - much thicker and visible
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <rect x="10" y="5" width="4" height="14" fill="green"/>
                <rect x="5" y="10" width="14" height="4" fill="green"/>
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
            """,
            
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
            """,
            
            // Simple Star
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <polygon points="12,2 15,9 22,9 16,14 18,21 12,17 6,21 8,14 2,9 9,9" fill="gold"/>
            </svg>
            """,
            
            // Lightning bolt
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <polygon points="13,2 3,14 12,14 11,22 21,10 12,10" fill="orange"/>
            </svg>
            """,
            
            // Lock icon
            """
            <svg width="24" height="24" viewBox="0 0 24 24">
                <rect x="5" y="11" width="14" height="10" fill="gray"/>
                <circle cx="12" cy="7" r="4" fill="gray"/>
            </svg>
            """
        };
        
        return Layout.Horizontal().Gap(2)
            | icons.Select(icon => new Svg(icon));
    }
}
```

<Callout Type="Warning">
For better icon management, consider using the comprehensive icon library available in Ivy Icons instead of custom SVG implementations.
</Callout>

To explore the available icons, visit the [Ivy Icons](../../03_ApiReference/IvyShared/Icons.md) page.

### Progress Bar

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
        
        var verticalBar = """
            <svg width="30" height="100" viewBox="0 0 30 100">
                <rect x="5" y="10" width="20" height="80" fill="#e5e7eb" rx="10"/>
                <rect x="5" y="50" width="20" height="40" fill="#10b981" rx="10"/>
            </svg>
            """;
            
        var stripedBar = """
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
            """;
            
        var circularProgress = """
            <svg width="60" height="60" viewBox="0 0 60 60">
                <circle cx="30" cy="30" r="25" fill="none" stroke="#e5e7eb" stroke-width="5"/>
                <circle cx="30" cy="30" r="25" fill="none" stroke="#8b5cf6" stroke-width="5" 
                        stroke-dasharray="157" stroke-dashoffset="78" transform="rotate(-90 30 30)"/>
            </svg>
            """;
            
        var gradientBar = """
            <svg width="200" height="20" viewBox="0 0 200 20">
                <defs>
                    <linearGradient id="grad1" x1="0%" y1="0%" x2="100%" y2="0%">
                        <stop offset="0%" style="stop-color:#3b82f6;stop-opacity:1" />
                        <stop offset="100%" style="stop-color:#8b5cf6;stop-opacity:1" />
                    </linearGradient>
                </defs>
                <rect width="200" height="20" fill="#e5e7eb" rx="10"/>
                <rect width="150" height="20" fill="url(#grad1)" rx="10"/>
            </svg>
            """;
            
        var animatedBar = """
            <svg width="200" height="20" viewBox="0 0 200 20">
                <rect width="200" height="20" fill="#e5e7eb" rx="10"/>
                <rect width="80" height="20" fill="#ef4444" rx="10">
                    <animate attributeName="width" values="80;160;80" dur="3s" repeatCount="indefinite"/>
                </rect>
            </svg>
            """;
         
        return Layout.Vertical().Gap(4)
            | new Svg(GetProgressBar(25))
            | new Svg(GetProgressBar(50))
            | new Svg(GetProgressBar(75))
            | Layout.Horizontal().Gap(4)
                | new Svg(verticalBar)
                | new Svg(stripedBar)
                | new Svg(circularProgress)
            | Layout.Horizontal().Gap(4)
                | new Svg(gradientBar)
                | new Svg(animatedBar);
    }
}
```

<Callout Type="Warning">
For better progress bar management, consider using the comprehensive progress bar library available in Ivy Progress instead of custom SVG implementations.
</Callout>

To explore the available progress bars, visit the [Ivy Progress](../../02_Widgets/01_Common/Progress.md) page.

### Simple Chart

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
            
        var pieChart = """
            <svg width="150" height="150" viewBox="0 0 150 150">
                <circle cx="75" cy="75" r="60" fill="none" stroke="#3b82f6" stroke-width="60" 
                        stroke-dasharray="113 226" transform="rotate(-90 75 75)"/>
                <circle cx="75" cy="75" r="60" fill="none" stroke="#ef4444" stroke-width="60" 
                        stroke-dasharray="113 113" transform="rotate(23 75 75)"/>
                <circle cx="75" cy="75" r="60" fill="none" stroke="#10b981" stroke-width="60" 
                        stroke-dasharray="226 113" transform="rotate(113 75 75)"/>
            </svg>
            """;
            
        var lineChart = """
            <svg width="200" height="100" viewBox="0 0 200 100">
                <polyline points="20,80 60,60 100,40 140,70 180,20" 
                          fill="none" stroke="#3b82f6" stroke-width="3"/>
                <circle cx="20" cy="80" r="3" fill="#3b82f6"/>
                <circle cx="60" cy="60" r="3" fill="#3b82f6"/>
                <circle cx="100" cy="40" r="3" fill="#3b82f6"/>
                <circle cx="140" cy="70" r="3" fill="#3b82f6"/>
                <circle cx="180" cy="20" r="3" fill="#3b82f6"/>
            </svg>
            """;
            
        var stackedBars = """
            <svg width="200" height="100" viewBox="0 0 200 100">
                <rect x="20" y="60" width="30" height="20" fill="#3b82f6"/>
                <rect x="20" y="40" width="30" height="20" fill="#ef4444"/>
                <rect x="20" y="20" width="30" height="20" fill="#10b981"/>
                
                <rect x="60" y="50" width="30" height="30" fill="#3b82f6"/>
                <rect x="60" y="30" width="30" height="20" fill="#ef4444"/>
                <rect x="60" y="10" width="30" height="20" fill="#10b981"/>
            </svg>
            """;
            
        var areaChart = """
            <svg width="200" height="100" viewBox="0 0 200 100">
                <path d="M 20 80 L 60 60 L 100 40 L 140 70 L 180 20 L 180 100 L 20 100 Z" 
                      fill="#3b82f6" opacity="0.3"/>
                <polyline points="20,80 60,60 100,40 140,70 180,20" 
                          fill="none" stroke="#3b82f6" stroke-width="2"/>
            </svg>
            """;
            
        var scatterPlot = """
            <svg width="200" height="100" viewBox="0 0 200 100">
                <circle cx="30" cy="70" r="4" fill="#ef4444"/>
                <circle cx="60" cy="50" r="4" fill="#10b981"/>
                <circle cx="90" cy="30" r="4" fill="#f59e0b"/>
                <circle cx="120" cy="60" r="4" fill="#8b5cf6"/>
                <circle cx="150" cy="20" r="4" fill="#06b6d4"/>
                <circle cx="180" cy="80" r="4" fill="#ec4899"/>
            </svg>
            """;
            
        return Layout.Vertical().Gap(4)
            | new Svg(chart)
            | Layout.Horizontal().Gap(4)
                | new Svg(pieChart)
                | new Svg(lineChart)
                | new Svg(stackedBars)
            | Layout.Horizontal().Gap(4)
                | new Svg(areaChart)
                | new Svg(scatterPlot);
    }
}
```

<Callout Type="Warning">
For better chart management, consider using the comprehensive chart library available in Ivy Charts instead of custom SVG implementations.
</Callout>

To explore the available charts, visit the [Ivy Charts](../../02_Widgets/06_Charts/BarChart.md) page.

<WidgetDocs Type="Ivy.Svg" ExtensionTypes="Ivy.SvgExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Svg.cs"/>
