# WrapLayout

<Ingress Text="Create flexible, responsive layouts that automatically wrap content to new lines when horizontal space runs out, perfect for tags, buttons, and card grids." />

The `WrapLayout` widget arranges child elements in rows that automatically wrap to the next line when they reach the end of the available horizontal space. It's perfect for creating responsive layouts like tag lists, button groups, or card grids that adapt to different screen sizes.

## Basic Usage

Here's a simple wrap layout with text elements:

```csharp demo-tabs ivy-bg
new WrapLayout([
    Text.Literal("First Item"),
    Text.Literal("Second Item"), 
    Text.Literal("Third Item"),
    Text.Literal("Fourth Item")
])
```

`WrapLayout` also works effectively with badge components:

```csharp demo-tabs ivy-bg
new WrapLayout([
    new Badge("React").Secondary(),
    new Badge("Vue").Secondary(),
    new Badge("Angular").Secondary(),
    new Badge("Svelte").Secondary(),
    new Badge("Next.js").Secondary(),
    new Badge("Nuxt.js").Secondary(),
    new Badge("Tailwind CSS").Secondary(),
    new Badge("TypeScript").Secondary()
])
```

### Gap

Control the spacing between items in the wrap layout:

```csharp demo-tabs ivy-bg
Layout.Vertical()
| new WrapLayout([
    new Badge("Small Gap (1)"),
    new Badge("Small Gap (1)"),
    new Badge("Small Gap (1)")
], gap: 1)
| new WrapLayout([
    new Badge("Medium Gap (4)"),
    new Badge("Medium Gap (4)"), 
    new Badge("Medium Gap (4)")
], gap: 4)
| new WrapLayout([
    new Badge("Large Gap (8)"),
    new Badge("Large Gap (8)"),
    new Badge("Large Gap (8)")
], gap: 8)
```

### Padding

Add internal spacing around the entire wrap layout:

```csharp demo-tabs ivy-bg
new WrapLayout([
    new Badge("Padded"),
    new Badge("Content"),
    new Badge("Items")
], padding: new Thickness(4))
```

### Margin

Add external spacing around the wrap layout:

```csharp demo-tabs ivy-bg
new WrapLayout([
    new Badge("With"),
    new Badge("Margin")
], margin: new Thickness(4))
```

### Combined Properties

Combining different properties for various layouts:

```csharp demo-tabs ivy-bg
Layout.Vertical()
| Text.Literal("Minimal spacing:")
| new WrapLayout([
    new Badge("Tight"),
    new Badge("Layout"),
    new Badge("Example")
], gap: 1, padding: new Thickness(2))
| Text.Literal("Generous spacing:")
| new WrapLayout([
    new Badge("Spacious"),
    new Badge("Layout"), 
    new Badge("Example")
], gap: 6, padding: new Thickness(4), margin: new Thickness(4))
```

<WidgetDocs Type="Ivy.WrapLayout" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Layouts/WrapLayout.cs"/>

## Advanced Examples

### Responsive Technology Tags

Create a responsive tag list with varying sizes that demonstrates natural wrapping:

```csharp demo-tabs ivy-bg
new WrapLayout([
    new Badge("JavaScript").Large().Primary(),
    new Badge("TS").Small().Secondary(),
    new Badge("React Framework").Large().Primary(),
    new Badge("Vue").Small().Secondary(),
    new Badge("Angular Development").Large().Outline(),
    new Badge("Svelte").Small().Destructive(),
    new Badge("Node.js Runtime").Large().Primary(),
    new Badge("API").Small().Secondary(),
    new Badge("MongoDB Database").Large().Primary(),
    new Badge("SQL").Small().Outline(),
    new Badge("Full Stack Development").Large().Primary(),
    new Badge("UI").Small().Secondary(),
    new Badge("DevOps").Secondary()
], gap: 2)
```

### Advanced Size & Gap Controls

Enhanced slider controls with more size options and mixed badge configurations:

```csharp demo-tabs
public class AdvancedSizeControlDemo : ViewBase
{
    public override object? Build()
    {
        var sizeScale = UseState(50); // 0-100 scale for more granular control
        var gap = UseState(4);
        var mixedSizes = UseState(false); // Toggle for mixed vs uniform sizes

        // Enhanced size mapping with more options
        (Sizes size, string label) GetSizeInfo(int scale) => scale switch
        {
            < 20 => (Sizes.Small, "Extra Small"),
            < 35 => (Sizes.Small, "Small"),
            < 50 => (Sizes.Medium, "Small-Medium"),
            < 65 => (Sizes.Medium, "Medium"),
            < 80 => (Sizes.Medium, "Medium-Large"),
            < 95 => (Sizes.Large, "Large"),
            _ => (Sizes.Large, "Extra Large")
        };

        // Create badges with different size strategies
        object[] CreateBadges()
        {
            var technologies = new[] 
            { 
                ("React", BadgeVariant.Primary),
                ("Vue", BadgeVariant.Secondary),
                ("Angular", BadgeVariant.Outline),
                ("TypeScript", BadgeVariant.Primary),
                ("JavaScript", BadgeVariant.Secondary),
                ("Node.js", BadgeVariant.Outline),
                ("Python", BadgeVariant.Destructive),
                ("C#", BadgeVariant.Primary)
            };

            if (mixedSizes.Value)
            {
                // Mixed sizes based on index and scale
                return technologies.Select((tech, index) =>
                {
                    var adjustedScale = sizeScale.Value + (index % 3 - 1) * 25; // Vary by ±25
                    adjustedScale = Math.Max(0, Math.Min(100, adjustedScale)); // Clamp to 0-100
                    var (size, _) = GetSizeInfo(adjustedScale);
                    return new Badge(tech.Item1).Size(size).Variant(tech.Item2);
                }).Cast<object>().ToArray();
            }
            else
            {
                // Uniform sizes
                var (uniformSize, _) = GetSizeInfo(sizeScale.Value);
                return technologies.Select(tech => 
                    new Badge(tech.Item1).Size(uniformSize).Variant(tech.Item2)
                ).Cast<object>().ToArray();
            }
        }

        var (currentSize, sizeLabel) = GetSizeInfo(sizeScale.Value);

        return Layout.Vertical()
            | Layout.Horizontal()
                | Text.Label("Size Scale:").Width(12)
                | sizeScale.ToNumberInput()
                    .Min(0)
                    .Max(100)
                    .ShowArrows()
                    .Width(10)
            | Layout.Horizontal()
                | Text.Label("Gap:").Width(12)
                | gap.ToNumberInput()
                    .Min(0)
                    .Max(20)
                    .ShowArrows()
                    .Width(10)
            | Layout.Horizontal()
            | new WrapLayout(CreateBadges(), gap: gap.Value);
    }
}
```

### Button Group

Create responsive button groups:

```csharp demo-tabs ivy-bg
new WrapLayout([
    new Button("Save", variant: ButtonVariant.Primary),
    new Button("Cancel", variant: ButtonVariant.Secondary),
    new Button("Delete", variant: ButtonVariant.Destructive),
    new Button("Export", variant: ButtonVariant.Outline),
    new Button("Import", variant: ButtonVariant.Outline),
    new Button("Settings", variant: ButtonVariant.Ghost)
], gap: 2)
```

### Card Grid

Create a responsive grid of cards:

```csharp demo-tabs ivy-bg
new WrapLayout([
    new Card().Title("Project Alpha").Description("Development project"),
    new Card().Title("Project Beta").Description("Testing phase")
], gap: 4)
```
