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
    .Padding(new Thickness(10))
```

### Also you can add Horizontal/Vertical Thickness

```csharp demo-tabs
new Box("Content")
    .Width(Size.Units(10))
    .Height(Size.Units(30))
    .Padding(new Thickness(50, 15))
```

### Individual Side Thickness

```csharp demo-tabs
new Box("Content")
    .Width(Size.Units(30))
    .Height(Size.Units(30))
    .Padding(new Thickness(2, 10, 6, 4))
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
    | new Box("Card 1")
        .Width(Size.Units(170))
        .Height(Size.Units(20))
        .Padding(new Thickness(10))
    | new Box("Card 2")
        .Width(Size.Units(170))
        .Height(Size.Units(20))
        .Padding(new Thickness(20, 15))
    | new Box("Card 3")
        .Width(Size.Units(170))
        .Height(Size.Units(20))
        .Padding(new Thickness(5, 5, 100, 20))
```

### Layout Margins

```csharp demo-tabs
Layout.Vertical()
    | new Box("box without margins")
        .Width(Size.Units(170))
        .Height(Size.Units(20))
| Layout.Vertical()
    .Margin(15, 5)  // Larger margins for comparison
    | new Box("Large margins (15,5)")
        .Width(Size.Units(170))
        .Height(Size.Units(20))
    | new Box("Another box without margins")
        .Width(Size.Units(170))
        .Height(Size.Units(20))
```
## Horizontal layout Margins
```csharp demo-tabs
Layout.Horizontal()
    .Margin(50, 5)  // Creates Thickness(4, 2) internally
    | new Box("With margins (50, 5)").Width(Size.Units(30)).Height(Size.Units(20))
| Layout.Horizontal()
    .Margin(10, 5)
    | new Box("With margins (10, 5)").Width(Size.Units(30)).Height(Size.Units(20))
```
### Border Thickness

```csharp demo-tabs
Layout.Horizontal()
    | new Box("Thin Border")
        .Width(Size.Units(170))
        .Height(Size.Units(40))
        .BorderThickness(new Thickness(1))
    | new Box("Thick Border")
        .Width(Size.Units(170))
        .Height(Size.Units(40))
        .BorderThickness(new Thickness(10))
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
    .BorderThickness(new Thickness(8))
```

<WidgetDocs Type="Ivy.Shared.Thickness" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Shared/Thickness.cs"/>
