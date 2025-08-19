# Box

<Ingress>
Create versatile container elements with customizable borders, colors, and padding for grouping content and structuring layouts.
</Ingress>

The `Box` widget is a versatile container element that provides customizable borders, colors, padding, margins, and content alignment. It's perfect for visually grouping related content, creating distinct sections in your UI, and building card-based layouts.

## Basic Usage

The simplest way to create a Box is by passing content directly to the constructor.

```csharp demo-tabs
public class BasicBoxExample : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Box("Simple content")
            | new Box("Content with padding").Padding(8)
            | new Box("Colored box").Color(Colors.Blue).Padding(8);
    }
}
```

## Colors

Boxes support a wide range of predefined colors that automatically adapt to light/dark themes.

```csharp demo-tabs
public class ColorExamplesView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Box("Primary Color").Color(Colors.Primary).Padding(8)
            | new Box("Success").Color(Colors.Green).Padding(8)
            | new Box("Warning").Color(Colors.Yellow).Padding(8)
            | new Box("Error").Color(Colors.Red).Padding(8)
            | new Box("Info").Color(Colors.Blue).Padding(8)
            | new Box("Secondary").Color(Colors.Secondary).Padding(8);
    }
}
```

## Border Styling

Boxes support various border styles, thicknesses, and radius options.

### Border Styles

```csharp demo-tabs
public class BorderStyleExamplesView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Box("Solid Border").BorderStyle(BorderStyle.Solid).Padding(8)
            | new Box("Dashed Border").BorderStyle(BorderStyle.Dashed).Padding(8)
            | new Box("Dotted Border").BorderStyle(BorderStyle.Dotted).Padding(8)
            | new Box("No Border").BorderStyle(BorderStyle.None).Padding(8);
    }
}
```

### Border Thickness

```csharp demo-tabs
public class BorderThicknessExamplesView : ViewBase
{
    public override object? Build()
    {
        return Layout.Horizontal().Gap(4)
            | new Box("Thin Border").BorderThickness(1).Padding(8)
            | new Box("Medium Border").BorderThickness(2).Padding(8)
            | new Box("Thick Border").BorderThickness(4).Padding(8);
    }
}
```

### Border Radius

```csharp demo-tabs
public class BorderRadiusExamplesView : ViewBase
{
    public override object? Build()
    {
        return Layout.Horizontal().Gap(4)
            | new Box("No Radius").BorderRadius(BorderRadius.None).Padding(8)
            | new Box("Rounded").BorderRadius(BorderRadius.Rounded).Padding(8)
            | new Box("Full Radius").BorderRadius(BorderRadius.Full).Padding(8);
    }
}
```

## Spacing

Control internal and external spacing using padding and margins.

### Basic Spacing

```csharp demo-tabs
public class SpacingExamplesView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Box("No Padding").Padding(0)
            | new Box("Small Padding").Padding(4)
            | new Box("Large Padding").Padding(16)
            | new Box("With Margin").Margin(8).Padding(8);
    }
}
```

### Advanced Spacing

```csharp demo-tabs
public class AdvancedSpacingView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Box("Uniform Padding")
                .Padding(new Thickness(8))
                .Width(Size.Units(20))
            | new Box("Horizontal/Vertical")
                .Padding(new Thickness(16, 8))
                .Width(Size.Units(20));
    }
}
```

## Content Alignment

Control how content is positioned within the box.

```csharp demo-tabs
public class ContentAlignmentView : ViewBase
{
    public override object? Build()
    {
        var content = Layout.Vertical().Gap(2)
            | new Box("Small").Width(Size.Units(8)).Height(Size.Units(8)).Color(Colors.Blue)
            | new Box("Medium").Width(Size.Units(12)).Height(Size.Units(8)).Color(Colors.Green);

        return Layout.Grid().Columns(3).Gap(8)
            | new Box("Top Left").ContentAlign(Align.TopLeft).Height(Size.Units(20)).Padding(8) | content
            | new Box("Center").ContentAlign(Align.Center).Height(Size.Units(20)).Padding(8) | content
            | new Box("Bottom Right").ContentAlign(Align.BottomRight).Height(Size.Units(20)).Padding(8) | content;
    }
}
```

## Sizing

Control the dimensions of your boxes.

```csharp demo-tabs
public class SizingExamplesView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Box("Auto Width").Width(Size.Auto()).Padding(8)
            | new Box("Fixed Width").Width(Size.Units(20)).Padding(8)
            | new Box("Full Width").Width(Size.Full()).Padding(8)
            | new Box("Fixed Size").Width(Size.Units(15)).Height(Size.Units(10)).Padding(8);
    }
}
```

## Practical Examples

### Status Dashboard

```csharp demo-tabs
public class StatusDashboardView : ViewBase
{
    public override object? Build()
    {
        return Layout.Grid().Columns(3).Gap(4)
            | new Box("System Online")
                .Color(Colors.Green)
                .BorderRadius(BorderRadius.Rounded)
                .Padding(8)
            | new Box("Warning: High CPU Usage")
                .Color(Colors.Yellow)
                .BorderStyle(BorderStyle.Dashed)
                .BorderThickness(2)
                .Padding(8)
            | new Box("Database Error")
                .Color(Colors.Red)
                .BorderThickness(2)
                .Padding(8);
    }
}
```

### Card Layout

```csharp demo-tabs
public class CardLayoutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Grid().Columns(2).Gap(8)
            | new Box()
                .Color(Colors.White)
                .BorderRadius(BorderRadius.Rounded)
                .BorderThickness(1)
                .Padding(12)
                .Content(
                    Text.H3("User Profile"),
                    Text.P("John Doe"),
                    Text.P("Software Developer")
                )
            | new Box()
                .Color(Colors.White)
                .BorderRadius(BorderRadius.Rounded)
                .BorderThickness(1)
                .Padding(12)
                .Content(
                    Text.H3("Statistics"),
                    Text.P("Projects: 15"),
                    Text.P("Experience: 5 years")
                );
    }
}
```

<WidgetDocs Type="Ivy.Box" ExtensionTypes="Ivy.BoxExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Box.cs"/>
