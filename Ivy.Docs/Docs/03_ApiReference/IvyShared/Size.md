# Size

`Size` represents width or height values used throughout the framework. You can
create sizes in pixels, rems, fractions or special values such as `Full` or
`Auto`.

## Basic Usage

The most common way to use Size is with widgets to set their dimensions:

```csharp demo-tabs
Layout.Horizontal()
    | new Box()
        .Width(Size.Px(100))
        .Height(Size.Rem(4))
    | new Box()
        .Width(Size.Fraction(1/2f))
        .Height(Size.Auto())
```

## Size Types

### Pixels (Px)

Use `Size.Px()` for precise pixel-based sizing:

```csharp demo-tabs
Layout.Vertical()
    | new Box("100px wide")
        .Width(Size.Px(100))
        .Height(Size.Units(20))
    | new Box("200px wide")
        .Width(Size.Px(200))
        .Height(Size.Units(20))
    | new Box("300px wide")
        .Width(Size.Px(300))
        .Height(Size.Units(20))
```

### Units

Use `Size.Units()` for framework-specific unit sizing:

```csharp demo-tabs
Layout.Vertical()
    | new Box("15 units wide")
        .Width(Size.Units(15))
        .Height(Size.Units(20))
    | new Box("20 units wide")
        .Width(Size.Units(20))
        .Height(Size.Units(20))
    | new Box("30 units wide")
        .Width(Size.Units(30))
        .Height(Size.Units(20))
```

### Fractions

Use `Size.Fraction()` for percentage-based sizing (0.0 to 1.0):

```csharp demo-tabs
Layout.Horizontal()
    | new Box("25%")
        .Width(Size.Fraction(0.25f))
        .Height(Size.Units(20))
    | new Box("50%")
        .Width(Size.Fraction(0.5f))
        .Height(Size.Units(20))
    | new Box("75%")
        .Width(Size.Fraction(0.75f))
        .Height(Size.Units(20))
```

### Rem Units

Use `Size.Rem()` for responsive sizing based on root font size:

```csharp demo-tabs
Layout.Vertical()
    | new Box("4rem wide")
        .Width(Size.Rem(4))
        .Height(Size.Units(20))
    | new Box("6rem wide")
        .Width(Size.Rem(6))
        .Height(Size.Units(20))
    | new Box("10rem wide")
        .Width(Size.Rem(10))
        .Height(Size.Units(20))
```

## Special Size Values

### Full Size

Use `Size.Full()` to take up all available space:

```csharp demo-tabs
Layout.Vertical()
    | new Box("Full width")
        .Width(Size.Full())
        .Height(Size.Units(20))
    | new Box("Full height")
        .Width(Size.Units(50))
        .Height(Size.Full())
```

### Auto Size

Use `Size.Auto()` to size based on content:

```csharp demo-tabs
Layout.Horizontal()
    | new Box("Auto width")
        .Width(Size.Auto())
        .Height(Size.Units(20))
    | new Box("Short")
        .Width(Size.Auto())
        .Height(Size.Units(20))
    | new Box("Longer text content")
        .Width(Size.Auto())
        .Height(Size.Units(20))
```

### Fit Content

Use `Size.Fit()` to size to fit content:

```csharp demo-tabs
Layout.Horizontal()
    | new Box("Fit")
        .Width(Size.Fit())
        .Height(Size.Units(20))
    | new Box("Small")
        .Width(Size.Fit())
        .Height(Size.Units(20))
    | new Box("Larger content")
        .Width(Size.Fit())
        .Height(Size.Units(20))
```

## Layout Examples

### Responsive Grid Layout

```csharp demo-tabs
Layout.Grid().Columns(3)
    | new Box("1/3")
        .Width(Size.Fraction(1/3f))
        .Height(Size.Units(30))
    | new Box("1/3")
        .Width(Size.Fraction(1/3f))
        .Height(Size.Units(30))
    | new Box("1/3")
        .Width(Size.Fraction(1/3f))
        .Height(Size.Units(30))
```

### Mixed Size Types

```csharp demo-tabs
Layout.Horizontal()
    | new Box("Fixed")
        .Width(Size.Px(150))
        .Height(Size.Units(30))
    | new Box("Flexible")
        .Width(Size.Fraction(0.5f))
        .Height(Size.Units(30))
    | new Box("Auto")
        .Width(Size.Auto())
        .Height(Size.Units(30))
```

### Cards with Different Sizes

```csharp demo-tabs
Layout.Vertical()
    | new Card("Full width card")
        .Width(Size.Full())
        .Height(Size.Units(40))
    | new Card("Half width card")
        .Width(Size.Fraction(0.5f))
        .Height(Size.Units(40))
    | new Card("Fixed width card")
        .Width(Size.Units(150))
        .Height(Size.Units(40))
```

## Size Constraints

### Min/Max Constraints

Use `.Min()` and `.Max()` to set size constraints:

```csharp demo-tabs
Layout.Vertical()
    | new Box("Min 100px")
        .Width(Size.Auto()
        .Min(Size.Px(100)))
        .Height(Size.Units(20))
    | new Box("Max 200px")
        .Width(Size.Full()
        .Max(Size.Px(200)))
        .Height(Size.Units(20))
    | new Box("Min 50px, Max 150px")
        .Width(Size.Fraction(0.3f)
        .Min(Size.Px(50))
        .Max(Size.Px(150)))
        .Height(Size.Units(20))
```

### Content-Based Constraints

```csharp demo-tabs
Layout.Vertical()
    | new Box("Min content")
        .Width(Size.MinContent())
        .Height(Size.Units(20))
    | new Box("Max content")
        .Width(Size.MaxContent())
        .Height(Size.Units(20))
    | new Box("Screen width")
        .Width(Size.Screen())
        .Height(Size.Units(20))
```

## Practical Examples

### Form Layout

```csharp demo-tabs
Layout.Vertical()
    | new Box("Full width form")
        .Width(Size.Full())
        .Height(Size.Units(30))
    | Layout.Horizontal()
        | new Box("Label")
            .Width(Size.Units(100))
            .Height(Size.Units(20))
        | new Box("Input")
            .Width(Size.Fraction(1f))
            .Height(Size.Units(20))
    | new Box("Submit button")
        .Width(Size.Auto())
        .Height(Size.Units(20))
```

### Dashboard Layout

```csharp demo-tabs
Layout.Grid().Columns(2)
    | new Card("Metric 1")
        .Width(Size.Full())
        .Height(Size.Units(60))
    | new Card("Metric 2")
        .Width(Size.Full())
        .Height(Size.Units(60))
    | new Card("Chart")
        .Width(Size.Full())
        .Height(Size.Units(100))
    | new Card("Table")
        .Width(Size.Full())
        .Height(Size.Units(100))
```

### Responsive Sidebar

```csharp demo-tabs
Layout.Horizontal()
    | new Box("Sidebar")
        .Width(Size.Units(250))
        .Height(Size.Full())
    | new Box("Main content")
        .Width(Size.Fraction(1f))
        .Height(Size.Full())
```