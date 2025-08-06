---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# Thickness

`Thickness` is a value type that represents spacing values for the four sides of a rectangular area. It's commonly used for padding, margins, borders, and offsets in Ivy Framework widgets and layouts.
## Base usage
We recomment to use thickness this way:
```csharp demo-tabs
new Box("Content")
    .Padding(new Thickness(8))
```

### Also you can add Horizontal/Vertical Thickness
```csharp demo-tabs
new Box("Content")
    .Padding(new Thickness(16, 8))
```

### Individual Side Thickness
```csharp demo-tabs
new Box("Content")
    .Padding(new Thickness(4, 8, 4, 8))
```

### Zero Thickness
```csharp demo-tabs
new Box("Content")
    .Padding(Thickness.Zero)
```

## Common Use Cases

### Widget Padding
```csharp demo-tabs
Layout.Vertical()
    | new Box("Card 1").Padding(new Thickness(8))
    | new Box("Card 2").Padding(new Thickness(16, 8))
    | new Box("Card 3").Padding(new Thickness(4, 8, 4, 8))
```

### Layout Margins
```csharp demo-tabs
Layout.Vertical()
    .Margin(4, 2)  // Creates Thickness(4, 2) internally
    | new Box("Content with margins")
    | new Box("Another box")
```

### Border Thickness
```csharp demo-tabs
Layout.Horizontal()
    | new Box("Thin Border").BorderThickness(new Thickness(1))
    | new Box("Thick Border").BorderThickness(new Thickness(4))
```

## Extension Method Support

Many widgets provide convenient extension methods that accept Thickness:

### Layout Views
```csharp demo-tabs
Layout.Vertical()
    .Padding(8)  // Creates Thickness(8) internally
    .Margin(4)   // Creates Thickness(4) internally
    | new Box("Content")
```

### Box Widget
```csharp demo-tabs
new Box("Content")
    .Padding(new Thickness(8))
    .Margin(new Thickness(4))
    .BorderThickness(new Thickness(2))
```

### ToString()
Returns a string representation in the format: `"Left,Top,Right,Bottom"`

### Implicit String Conversion
Thickness can be implicitly converted to string:
```csharp demo-tabs
Layout.Vertical()
    | Text.Block($"Thickness: {new Thickness(4, 8, 4, 8)}")
    | Text.Block($"Zero: {Thickness.Zero}")
```

## Best Practices

### When to Use Each Constructor
- **Uniform**: Use when all sides need the same spacing
- **Horizontal/Vertical**: Use for common layout patterns (e.g., cards, buttons)
- **Individual**: Use when precise control is needed for each side

### Common Patterns
```csharp demo-tabs
Layout.Vertical()
    | new Box("Standard Box").Padding(new Thickness(16, 8))
    | Layout.Horizontal().Margin(4)  // Creates Thickness(4) internally
        | new Box("Left")
        | new Box("Right")
```

<WidgetDocs Type="Ivy.Shared.Thickness" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Shared/Thickness.cs"/>
