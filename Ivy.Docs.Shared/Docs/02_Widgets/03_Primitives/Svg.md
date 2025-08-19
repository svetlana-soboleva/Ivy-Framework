# Svg

The `Svg` widget renders scalable vector graphics directly in your app. SVGs are resolution-independent and perfect for icons, illustrations, charts, and other graphics that need to scale without losing quality.

## Basic Shapes

Create simple geometric shapes with SVG:

```csharp demo-tabs 
public class BasicShapesView : ViewBase
{
    public override object? Build()
    {
        var basicShapes = """
            <svg width="200" height="150" viewBox="0 0 200 150">
                <rect x="10" y="10" width="40" height="40" fill="#3b82f6" />
                <circle cx="80" cy="30" r="20" fill="#ef4444" />
                <ellipse cx="130" cy="30" rx="25" ry="15" fill="#10b981" />
                <line x1="10" y1="70" x2="190" y2="70" stroke="#f59e0b" stroke-width="3" />
                <polygon points="10,120 30,100 50,120 30,140" fill="#8b5cf6" />
                <path d="M 60 100 L 80 120 L 100 100 L 120 120 L 140 100" 
                      stroke="#06b6d4" stroke-width="2" fill="none" />
            </svg>
            """;
            
        return new Svg(basicShapes);
    }
}
```

## Progress Indicators

Create animated progress circles and bars:

```csharp demo-tabs 
public class ProgressIndicatorsView : ViewBase
{
    public override object? Build()
    {
        string GetCircleProgress(int percentage)
        {
            var radius = 40;
            var circumference = 2 * Math.PI * radius;
            var dashOffset = circumference * (1 - percentage / 100.0);
            
            return $"""
                <svg width="100" height="100" viewBox="0 0 100 100">
                  <circle 
                    cx="50" 
                    cy="50" 
                    r="{radius}" 
                    fill="none" 
                    stroke="#e5e7eb" 
                    stroke-width="8" 
                  />
                  <circle 
                    cx="50" 
                    cy="50" 
                    r="{radius}" 
                    fill="none" 
                    stroke="#3b82f6" 
                    stroke-width="8" 
                    stroke-dasharray="{circumference}" 
                    stroke-dashoffset="{dashOffset}" 
                    transform="rotate(-90 50 50)" 
                    stroke-linecap="round" 
                  />
                  <text 
                    x="50" 
                    y="60" 
                    text-anchor="middle" 
                    font-size="20" 
                    font-weight="bold" 
                    fill="#3b82f6"
                  >
                    {percentage}%
                  </text>
                </svg>
                """;
        }
         
        return Layout.Horizontal().Gap(4)
            | new Svg(GetCircleProgress(25))
            | new Svg(GetCircleProgress(50))
            | new Svg(GetCircleProgress(75))
            | new Svg(GetCircleProgress(90));
    }
}
```

## Icons and Symbols

Create custom icons and symbols:

```csharp demo-tabs 
public class CustomIconsView : ViewBase
{
    public override object? Build()
    {
        var icons = new[]
        {
            // Home icon
            """
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/>
                <polyline points="9,22 9,12 15,12 15,22"/>
            </svg>
            """,
            
            // Settings icon
            """
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <circle cx="12" cy="12" r="3"/>
                <path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"/>
            </svg>
            """,
            
            // User icon
            """
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
                <circle cx="12" cy="7" r="4"/>
            </svg>
            """
        };
        
        return Layout.Horizontal().Gap(4)
            | icons.Select(icon => new Svg(icon));
    }
}
```

## Data Visualization

Create simple charts and data visualizations:

```csharp demo-tabs 
public class DataVisualizationView : ViewBase
{
    public override object? Build()
    {
        var barChart = """
            <svg width="300" height="200" viewBox="0 0 300 200">
                <rect x="20" y="160" width="30" height="40" fill="#3b82f6" />
                <rect x="60" y="140" width="30" height="60" fill="#ef4444" />
                <rect x="100" y="120" width="30" height="80" fill="#10b981" />
                <rect x="140" y="100" width="30" height="100" fill="#f59e0b" />
                <rect x="180" y="80" width="30" height="120" fill="#8b5cf6" />
                <rect x="220" y="60" width="30" height="140" fill="#06b6d4" />
                
                <text x="35" y="195" text-anchor="middle" font-size="12">A</text>
                <text x="75" y="195" text-anchor="middle" font-size="12">B</text>
                <text x="115" y="195" text-anchor="middle" font-size="12">C</text>
                <text x="155" y="195" text-anchor="middle" font-size="12">D</text>
                <text x="195" y="195" text-anchor="middle" font-size="12">E</text>
                <text x="235" y="195" text-anchor="middle" font-size="12">F</text>
            </svg>
            """;
            
        return new Svg(barChart);
    }
}
```

## Interactive Elements

Create SVG elements with hover effects and animations:

```csharp demo-tabs 
public class InteractiveElementsView : ViewBase
{
    public override object? Build()
    {
        var interactiveSvg = """
            <svg width="200" height="200" viewBox="0 0 200 200">
                <defs>
                    <linearGradient id="grad1" x1="0%" y1="0%" x2="100%" y2="100%">
                        <stop offset="0%" style="stop-color:#3b82f6;stop-opacity:1" />
                        <stop offset="100%" style="stop-color:#8b5cf6;stop-opacity:1" />
                    </linearGradient>
                    <filter id="shadow" x="-20%" y="-20%" width="140%" height="140%">
                        <feDropShadow dx="2" dy="2" stdDeviation="3" flood-color="#000000" flood-opacity="0.3"/>
                    </filter>
                </defs>
                
                <circle cx="100" cy="100" r="60" fill="url(#grad1)" filter="url(#shadow)" />
                <text x="100" y="110" text-anchor="middle" font-size="24" font-weight="bold" fill="white">Hover Me</text>
                
                <g opacity="0.7">
                    <circle cx="50" cy="50" r="8" fill="#ef4444" />
                    <circle cx="150" cy="50" r="8" fill="#10b981" />
                    <circle cx="50" cy="150" r="8" fill="#f59e0b" />
                    <circle cx="150" cy="150" r="8" fill="#8b5cf6" />
                </g>
            </svg>
            """;
            
        return new Svg(interactiveSvg);
    }
}
```

## Complex Graphics

Create more complex SVG graphics with multiple elements:

```csharp demo-tabs 
public class ComplexGraphicsView : ViewBase
{
    public override object? Build()
    {
        var complexSvg = """
            <svg width="400" height="300" viewBox="0 0 400 300">
                <!-- Background -->
                <rect width="400" height="300" fill="#f8fafc" />
                
                <!-- Mountains -->
                <polygon points="0,300 100,200 200,250 300,150 400,200 400,300" fill="#64748b" />
                <polygon points="0,300 50,220 100,200 150,230 200,250 250,200 300,150 350,180 400,200 400,300" fill="#475569" />
                
                <!-- Sun -->
                <circle cx="350" cy="50" r="30" fill="#fbbf24" />
                <g opacity="0.6">
                    <line x1="320" y1="20" x2="300" y2="10" stroke="#fbbf24" stroke-width="3" />
                    <line x1="380" y1="20" x2="400" y2="10" stroke="#fbbf24" stroke-width="3" />
                    <line x1="320" y1="80" x2="300" y2="90" stroke="#fbbf24" stroke-width="3" />
                    <line x1="380" y1="80" x2="400" y2="90" stroke="#fbbf24" stroke-width="3" />
                </g>
                
                <!-- Trees -->
                <g fill="#059669">
                    <polygon points="50,300 70,250 90,300" />
                    <polygon points="60,300 80,250 100,300" />
                    <polygon points="70,300 90,250 110,300" />
                </g>
                
                <!-- House -->
                <rect x="150" y="200" width="100" height="80" fill="#dc2626" />
                <polygon points="150,200 200,150 250,200" fill="#991b1b" />
                <rect x="170" y="220" width="20" height="30" fill="#1e40af" />
                <rect x="210" y="220" width="20" height="30" fill="#1e40af" />
                <rect x="190" y="240" width="20" height="40" fill="#92400e" />
                
                <!-- Road -->
                <rect x="0" y="280" width="400" height="20" fill="#6b7280" />
                <line x1="0" y1="290" x2="400" y2="290" stroke="white" stroke-width="2" stroke-dasharray="10,10" />
            </svg>
            """;
            
        return new Svg(complexSvg);
    }
}
```

## Properties

The `Svg` widget has the following properties:

- **Content** (string): The SVG markup content to render
- **Width** (Size): The width of the SVG container (defaults to Auto)
- **Height** (Size): The height of the SVG container (defaults to Auto)

## Best Practices

1. **Use viewBox**: Always include a `viewBox` attribute for responsive scaling
2. **Optimize paths**: Use simplified path data when possible for better performance
3. **Accessibility**: Include `aria-label` or `title` elements for screen readers
4. **Responsive design**: Use relative units and viewBox for responsive behavior
5. **Performance**: Keep SVG files small and avoid unnecessary elements

<WidgetDocs Type="Ivy.Svg" ExtensionTypes="Ivy.SvgExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Svg.cs"/>
