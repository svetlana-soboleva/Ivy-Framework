# Widgets

<Ingress>
Discover the fundamental building blocks of Ivy applications - Widgets provide declarative UI components inspired by React's component model.
</Ingress>

Widgets are the fundamental building blocks of the Ivy framework. They represent the smallest unit of UI and are used to construct Views. Inspired by React's component model, Widgets provide a declarative way to build user interfaces.

## Widget Categories

Ivy provides a comprehensive set of widgets organized into several categories:

### Common Widgets

The most frequently used widgets for building user interfaces:

```csharp demo-tabs
Layout.Grid().Columns(2)
    | new Button("Primary Button")
    | new Badge("New", BadgeVariant.Primary)
    | new Card("Card Content", new Button("Action"))
    | new Progress(75).Goal("Upload Progress")
```

### Input Widgets

Widgets for capturing and validating user input:

```csharp demo-tabs
Layout.Grid().Columns(2)
    | new TextInput(UseState("")).Placeholder("Enter text...")
    | new NumberInput<int>(UseState(0)).Placeholder("Enter number")
    | new BoolInput(UseState(false)).Label("Enable feature")
    | new SelectInput<string>(UseState(""), [new Option<string>("Option 1", "1"), new Option<string>("Option 2", "2"), new Option<string>("Option 3", "3")])
```

### Primitive Widgets

Basic building blocks for text, layout, and content:

```csharp demo-tabs
Layout.Grid().Columns(2)
    | Text.Large("Hello World")
    | new Avatar("JD").Size(Size.Units(40))
    | new Icon(Icons.Star).Size(Size.Units(24))
    | new Box("Container").BorderRadius(BorderRadius.Rounded)
```

### Layout Widgets

Widgets for organizing and arranging other widgets:

```csharp demo-tabs
Layout.Grid().Columns(2)
    | Layout.Horizontal()
        | new Button("Left")
        | new Button("Right")
    | Layout.Vertical()
        | new Button("Top")
        | new Button("Bottom")
```

### Chart Widgets

Data visualization components:

```csharp demo-tabs
public class ChartDemo : ViewBase 
{    
    public override object? Build()
    {
        var data = new[]
        {
            new { Month = "Jan", Apples = 100, Oranges = 40, Blueberry = 35 },
            new { Month = "Feb", Apples = 150, Oranges = 60, Blueberry = 55 },
            new { Month = "Mar", Apples = 170, Oranges = 70, Blueberry = 65 }
        };

        return Layout.Vertical()
            | new BarChart(data,
                new Bar("Apples")
                    .Fill(Colors.Red)
                    .LegendType(LegendTypes.Square))
                .Bar(new Bar("Oranges")
                    .Fill(Colors.Orange)
                    .LegendType(LegendTypes.Square))
                .Bar(new Bar("Blueberry")
                    .Fill(Colors.Blue)
                    .Name("Blueberries")
                    .LegendType(LegendTypes.Square))
                .Tooltip()
                .Legend();
    }
}
```

## Widget Properties and Methods

All widgets in Ivy follow a consistent pattern for configuration:

### Fluent API

Widgets use a fluent API for easy configuration:

```csharp demo-tabs
Layout.Grid().Columns(2)
    | new Button("Styled Button")
        .Primary()
        .Large()
        .Icon(Icons.Star)
        .BorderRadius(BorderRadius.Full)
    | new Card("Styled Card")
        .Title("Card Title")
        .Description("Card description")
        .BorderColor(Colors.Blue)
        .BorderRadius(BorderRadius.Rounded)
```

### State Binding

Widgets can be bound to state for reactive updates:

```csharp demo-tabs
public class StateBindingDemo : ViewBase
{
    public override object? Build()
    {
        var count = UseState(0);
        var text = UseState("");
        
        return Layout.Vertical()
            | new TextInput(text).Placeholder("Enter text...")
            | new Button($"Count: {count.Value}", onClick: _ => count.Value++)
            | new Progress(count.Value).Goal("Progress to 100");
    }
}
```

## Widget Composition

Widgets can be combined and nested to create complex interfaces:

### Nested Layouts

Combine multiple layout widgets for sophisticated arrangements:

```csharp demo-tabs
Layout.Grid().Columns(2)
    | Layout.Vertical()
        | new Card("Header Card")
        | Layout.Horizontal()
            | new Button("Left")
            | new Button("Right")
        | new Card("Footer Card")
    | Layout.Grid().Columns(3)
        | new Badge("1", BadgeVariant.Primary)
        | new Badge("2", BadgeVariant.Secondary)
        | new Badge("3", BadgeVariant.Outline)
        | new Badge("4", BadgeVariant.Destructive)
        | new Badge("5", BadgeVariant.Primary)
        | new Badge("6", BadgeVariant.Secondary)
```

### Conditional Rendering

Use conditional logic to show/hide widgets:

```csharp demo-tabs
public class ConditionalDemo : ViewBase
{
    public override object? Build()
    {
        var showDetails = UseState(false);
        
        return Layout.Vertical()
            | new Button("Toggle Details", onClick: _ => showDetails.Value = !showDetails.Value)
            | (showDetails.Value ? 
                new Card("Hidden Details", "This content is conditionally shown")
                    .BorderColor(Colors.Green) : null);
    }
}
```

## Widget Variants and Styling

Most widgets support multiple visual variants and styling options:

### Color Variants

```csharp demo-tabs
Layout.Grid().Columns(4)
    | new Button("Primary").Primary()
    | new Button("Secondary").Secondary()
    | new Button("Destructive").Destructive()
    | new Button("Outline").Outline()
```

### Size Variants

```csharp demo-tabs
Layout.Grid().Columns(3)
    | new Button("Small").Small()
    | new Button("Medium")
    | new Button("Large").Large()
```

### Border and Spacing

```csharp demo-tabs
Layout.Grid().Columns(2)
    | new Box("Rounded Box")
        .BorderRadius(BorderRadius.Rounded)
        .Color(Colors.Blue)
        .Padding(new Thickness(16))
    | new Box("Full Border Box")
        .BorderRadius(BorderRadius.Full)
        .Color(Colors.Green)
        .BorderThickness(new Thickness(3))
```

## Advanced Widget Features

### Event Handling

Widgets support various event handlers:

```csharp demo-tabs
public class EventHandlingDemo : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return Layout.Grid().Columns(2)
            | new Button("Click Me", onClick: _ => client.Toast("Button clicked!"))
            | new TextInput(UseState(""))
                .Placeholder("Text input with change handling")
            | new Button("Hover Me")
                .Tooltip("Hover over me!")
            | new Button("Focus Me")
                .Tooltip("Click to focus!");
    }
}
```

### Validation and Feedback

Input widgets provide built-in validation through the `Invalid` property:

```csharp demo-tabs
public class ValidationDemo : ViewBase
{
    public override object? Build()
    {
        var email = UseState("");
        var password = UseState("");
        
        return Layout.Vertical()
            | new TextInput(email)
                .Placeholder("Enter email")
                .Variant(TextInputs.Email)
                .Invalid(email.Value != "" && !email.Value.Contains("@") ? "Invalid email format" : null)
            | new TextInput(password)
                .Placeholder("Enter password")
                .Variant(TextInputs.Password)
                .Invalid(password.Value != "" && password.Value.Length < 8 ? "Password must be at least 8 characters" : null);
    }
}
```

## Best Practices

### Widget Organization

- Group related widgets together using layout containers
- Use consistent spacing and alignment
- Leverage the grid system for responsive layouts

### Performance Considerations

- Use state efficiently to avoid unnecessary re-renders
- Implement proper disposal patterns for long-lived widgets
- Consider lazy loading for complex widget trees

### Accessibility

- Provide meaningful labels and descriptions
- Use semantic HTML equivalents where possible
- Ensure keyboard navigation support

## Next Steps

Now that you understand the basics of widgets, explore the specific widget documentation:

- **Common Widgets** - Buttons, Cards, Badges, and more
- **Input Widgets** - Text, Numbers, Selects, and validation
- **Primitive Widgets** - Text, Icons, Boxes, and basic elements
- **Layout Widgets** - Grids, Flexbox, and positioning
- **Chart Widgets** - Data visualization components
- **Advanced Widgets** - Complex patterns and custom widgets

Each widget category includes detailed examples, API documentation, and best practices for effective usage.
