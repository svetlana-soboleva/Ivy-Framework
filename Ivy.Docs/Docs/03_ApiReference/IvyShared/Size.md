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

This example shows mixing different size types - pixels for width, rem units for height, fractions for responsive width, and auto-sizing for height.

## Size Types

### Pixels (Px)

Use `Size.Px()` for precise pixel-based sizing:

```csharp demo-tabs
Layout.Vertical()
    | new Box("100px wide")
        .Width(Size.Px(100))
        .Height(Size.Px(50))
    | new Box("200px wide")
        .Width(Size.Px(200))
        .Height(Size.Px(50))
    | new Box("300px wide")
        .Width(Size.Px(300))
        .Height(Size.Px(75))
```

Pixel sizing provides exact control over element dimensions. Each box has a fixed width in pixels and varying heights to demonstrate the precision of pixel-based sizing.

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

Framework units provide a consistent sizing system across the application. These units scale with the overall design system and maintain proportional relationships.

### Fractions

Use `Size.Fraction()` for percentage-based sizing (0.0 to 1.0):

```csharp demo-tabs
Layout.Horizontal()
    | new Box("25%")
        .Width(Size.Fraction(0.25f))
        .Height(Size.Fraction(0.25f))
    | new Box("50%")
        .Width(Size.Fraction(0.5f))
        .Height(Size.Fraction(0.25f))
    | new Box("75%")
        .Width(Size.Fraction(0.75f))
        .Height(Size.Fraction(0.25f))
```

Fractional sizing creates responsive layouts that adapt to available space. The boxes take up 25%, 50%, and 75% of the container width respectively.

### Rem Units

Use `Size.Rem()` for responsive sizing based on root font size:

```csharp demo-tabs
Layout.Horizontal()
    | new Box("4rem wide")
        .Width(Size.Rem(4))
        .Height(Size.Rem(4))
    | new Box("6rem wide")
        .Width(Size.Rem(6))
        .Height(Size.Rem(4))
    | new Box("10rem wide")
        .Width(Size.Rem(10))
        .Height(Size.Rem(4))
```

Rem units scale with the user's font size preferences, making layouts more accessible. Each box is sized relative to the root font size, maintaining proportional relationships.

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

Full sizing expands elements to fill their container completely. The first box takes full width, the second takes full height, demonstrating how elements can expand in different dimensions.

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

Auto sizing allows elements to size themselves based on their content. Each box adjusts its width to fit its text content exactly.

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

Fit sizing is similar to auto but with more precise control over how content is measured and sized.

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

This creates a responsive 3-column grid where each column takes exactly one-third of the available width, automatically adjusting to different screen sizes.

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

This demonstrates combining different sizing strategies in a single layout - fixed pixel width, flexible fractional width, and auto-sizing based on content.

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

This shows how cards can use different sizing strategies - one that fills the entire width, one that takes half the width, and one with a fixed width in framework units.

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

Constraints allow you to set minimum and maximum bounds for element sizes. The first box has a minimum width, the second has a maximum width, and the third has both minimum and maximum constraints.

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

These special size values provide content-aware and screen-aware sizing options for advanced layout scenarios.

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

This demonstrates a typical form layout with a full-width header, a horizontal row with fixed-width label and flexible input field, and an auto-sized submit button.

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

This shows a dashboard grid layout where all cards take full width within their grid cells, with different heights for different content types.

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

This demonstrates a classic sidebar layout with a fixed-width sidebar and a flexible main content area that takes up the remaining space.