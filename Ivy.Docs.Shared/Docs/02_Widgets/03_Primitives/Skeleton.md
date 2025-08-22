# Skeleton

<Ingress>
Create elegant loading placeholders that mimic your content structure to improve perceived performance during data loading.
</Ingress>

The `Skeleton` widget creates placeholder loading indicators that mimic the shape of your content. It improves perceived performance by showing users the layout of the page while data is loading, reducing the jarring effect of content suddenly appearing.

## Basic Usage

The simplest way to create a skeleton is to instantiate it directly:

```csharp demo-tabs
public class BasicSkeletonView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(3)
            | new Skeleton().Height(Size.Units(24)).Width(Size.Units(200))
            | new Skeleton().Height(Size.Units(16)).Width(Size.Full())
            | new Skeleton().Height(Size.Units(16)).Width(Size.Units(150));
    }
}
```

## Common Patterns

### Text Content Skeleton

Mimic text content with varying widths to simulate realistic text layouts:

```csharp demo-tabs
public class TextSkeletonView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(2).Padding(4)
            | new Skeleton().Height(Size.Units(28)).Width(Size.Units(250)) // Title
            | new Skeleton().Height(Size.Units(16)).Width(Size.Full())     // Line 1
            | new Skeleton().Height(Size.Units(16)).Width(Size.Full())     // Line 2
            | new Skeleton().Height(Size.Units(16)).Width(Size.Units(180)) // Line 3 (shorter)
            | new Skeleton().Height(Size.Units(16)).Width(Size.Units(220)) // Line 4
            | new Skeleton().Height(Size.Units(16)).Width(Size.Units(160)); // Line 5 (shortest)
    }
}
```

### Card Layout Skeleton

Create skeleton placeholders for card-based layouts:

```csharp demo-tabs
public class CardSkeletonView : ViewBase
{
    public override object? Build()
    {
        return new Card(
            Layout.Vertical().Gap(3).Padding(4)
                | new Skeleton().Height(Size.Units(160)).Width(Size.Full()) // Image placeholder
                | new Skeleton().Height(Size.Units(20)).Width(Size.Units(180)) // Title
                | new Skeleton().Height(Size.Units(14)).Width(Size.Full()) // Description line 1
                | new Skeleton().Height(Size.Units(14)).Width(Size.Units(200)) // Description line 2
                | Layout.Horizontal().Gap(2)
                    | new Skeleton().Height(Size.Units(32)).Width(Size.Units(80)) // Price
                    | new Skeleton().Height(Size.Units(32)).Width(Size.Units(100)) // Button
        );
    }
}
```

### List Item Skeleton

Perfect for loading states in lists and feeds:

```csharp demo-tabs
public class ListSkeletonView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Enumerable.Range(0, 5).Select(_ =>
                Layout.Horizontal().Gap(3).Padding(3)
                    | new Skeleton().Height(Size.Units(48)).Width(Size.Units(48)) // Avatar (circular)
                    | Layout.Vertical().Gap(2).Width(Size.Full())
                        | new Skeleton().Height(Size.Units(16)).Width(Size.Units(120)) // Name
                        | new Skeleton().Height(Size.Units(14)).Width(Size.Units(200)) // Message preview
            );
    }
}
```

## Loading State Integration

### Data Loading Example

Show how to integrate skeletons with actual data loading:

```csharp demo-tabs
public class ProductCardView : ViewBase
{
    public record ProductData(
        string Name, 
        string Description, 
        decimal Price, 
        string ImageUrl, 
        double Rating, 
        int Stock);
    
    public override object? Build()
    {
        var isLoading = UseState(true);
        var product = UseState<ProductData?>();
        
        // Simulate loading data
        UseEffect(() => {
            var timer = new System.Threading.Timer(_ => {
                product.Set(new ProductData(
                    "Premium Wireless Headphones",
                    "Experience crystal-clear sound with our premium noise-cancelling wireless headphones. Features 30-hour battery life and memory foam ear cushions for all-day comfort.",
                    149.99m,
                    "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=300&h=200&fit=crop",
                    4.7,
                    12
                ));
                isLoading.Set(false);
            }, null, 2000, Timeout.Infinite);
            return timer;
        }, []);
        
        return new Card(
            isLoading.Value
                ? Layout.Vertical().Gap(3).Padding(4)
                    | new Skeleton().Height(Size.Units(200)).Width(Size.Full())
                    | new Skeleton().Height(Size.Units(24)).Width(Size.Units(200))
                    | new Skeleton().Height(Size.Units(16)).Width(Size.Full())
                    | new Skeleton().Height(Size.Units(16)).Width(Size.Full())
                    | new Skeleton().Height(Size.Units(16)).Width(Size.Units(150))
                    | Layout.Horizontal().Gap(2)
                        | new Skeleton().Height(Size.Units(24)).Width(Size.Units(80))
                        | new Skeleton().Height(Size.Units(36)).Width(Size.Units(100))
                : Layout.Vertical().Gap(3).Padding(4)
                    | new Image(product.Value?.ImageUrl)
                        .Height(Size.Units(200))
                        .Width(Size.Full())
                    | Text.H3(product.Value?.Name)
                    | Text.P(product.Value?.Description)
                    | Text.Strong($"Rating: {product.Value?.Rating}/5")
                    | Layout.Horizontal().Gap(2)
                        | Text.H4($"${product.Value?.Price}")
                        | new Button("Add to Cart")
        ).Width(Size.Units(300));
    }
}
```

### Dashboard Metrics

Use skeletons in dashboard components while metrics load:

```csharp demo-tabs
public class MetricCardView : ViewBase
{
    public override object? Build()
    {
        var isLoading = UseState(true);
        var metric = UseState<string?>();
        
        UseEffect(() => {
            var timer = new System.Threading.Timer(_ => {
                metric.Set("$12,345");
                isLoading.Set(false);
            }, null, 1500, Timeout.Infinite);
            return timer;
        }, []);
        
        return new Card().Title("Revenue").Icon(Icons.DollarSign).Height(Size.Units(120))
            | (isLoading.Value
                ? new Skeleton().Height(Size.Units(40)).Width(Size.Units(120))
                : Text.H2(metric.Value));
    }
}
```

## Best Practices

### Sizing Guidelines

Match skeleton dimensions to your actual content:

```csharp demo-tabs
public class SizingGuidelinesView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4).Padding(4)
            | Text.H4("Headings")
            | Layout.Vertical().Gap(2)
                | new Skeleton().Height(Size.Units(32)).Width(Size.Units(300)) // H1 equivalent
                | new Skeleton().Height(Size.Units(28)).Width(Size.Units(250)) // H2 equivalent
                | new Skeleton().Height(Size.Units(24)).Width(Size.Units(200)) // H3 equivalent
                | new Skeleton().Height(Size.Units(20)).Width(Size.Units(180)) // H4 equivalent
            
            | Text.H4("Body Text")
            | Layout.Vertical().Gap(1)
                | new Skeleton().Height(Size.Units(16)).Width(Size.Full())     // Paragraph line
                | new Skeleton().Height(Size.Units(16)).Width(Size.Full())     // Paragraph line
                | new Skeleton().Height(Size.Units(16)).Width(Size.Units(180)) // Shorter line
            
            | Text.H4("Interactive Elements")
            | Layout.Horizontal().Gap(3)
                | new Skeleton().Height(Size.Units(36)).Width(Size.Units(100)) // Button
                | new Skeleton().Height(Size.Units(32)).Width(Size.Units(200)) // Input field
                | new Skeleton().Height(Size.Units(40)).Width(Size.Units(40)); // Icon button
    }
}
```

### Layout Consistency

Ensure skeleton layouts match your actual content structure:

```csharp demo-tabs
public class LayoutConsistencyView : ViewBase
{
    public override object? Build()
    {
        var showContent = UseState(false);
        
        return Layout.Vertical().Gap(4)
            | new Button(showContent.Value ? "Show Skeleton" : "Show Content", 
                onClick: _ => showContent.Set(!showContent.Value))
            
            | (showContent.Value
                ? Layout.Horizontal().Gap(4).Padding(4) // Actual content
                    | new Avatar("John Doe", "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=100&h=100&fit=crop&crop=face")
                    | Layout.Vertical().Gap(2)
                        | Text.H4("John Doe")
                        | Text.Small("Software Engineer")
                        | Text.P("Passionate about creating amazing user experiences with modern web technologies.")
                
                : Layout.Horizontal().Gap(4).Padding(4) // Skeleton version
                    | new Skeleton().Height(Size.Units(64)).Width(Size.Units(64)) // Avatar placeholder
                    | Layout.Vertical().Gap(2)
                        | new Skeleton().Height(Size.Units(20)).Width(Size.Units(120))
                        | new Skeleton().Height(Size.Units(14)).Width(Size.Units(100))
                        | new Skeleton().Height(Size.Units(16)).Width(Size.Units(280))
            );
    }
}
```

## Advanced Usage

### Responsive Skeletons

Create skeletons that adapt to different screen sizes:

```csharp demo-tabs
public class ResponsiveSkeletonView : ViewBase
{
    public override object? Build()
    {
        return Layout.Grid().Columns(3).Gap(4).Padding(4) // Grid layout
            | Enumerable.Range(0, 6).Select(_ =>
                new Card(
                    Layout.Vertical().Gap(2).Padding(3)
                        | new Skeleton().Height(Size.Units(120)).Width(Size.Full()) // Image
                        | new Skeleton().Height(Size.Units(18)).Width(Size.Fraction(0.8f)) // Title
                        | new Skeleton().Height(Size.Units(14)).Width(Size.Full()) // Description
                        | new Skeleton().Height(Size.Units(14)).Width(Size.Fraction(0.6f)) // Price
                )
            );
    }
}
```

### Animated Loading States

Combine skeletons with loading indicators:

```csharp demo-tabs
public class AnimatedLoadingView : ViewBase
{
    public override object? Build()
    {
        var progress = UseState(0);
        
        UseEffect(() => {
            var timer = new System.Threading.Timer(_ => {
                progress.Set(p => Math.Min(p + 10, 100));
            }, null, 100, 200);
            return timer;
        }, []);
        
        return Layout.Vertical().Gap(4).Padding(4)
            | new Progress(progress.Value).Width(Size.Full())
            | Text.Small($"Loading... {progress.Value}%")
            | Layout.Vertical().Gap(2)
                | new Skeleton().Height(Size.Units(24)).Width(Size.Units(200))
                | new Skeleton().Height(Size.Units(16)).Width(Size.Full())
                | new Skeleton().Height(Size.Units(16)).Width(Size.Units(180));
    }
}
```

<WidgetDocs Type="Ivy.Skeleton" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Skeleton.cs"/>
