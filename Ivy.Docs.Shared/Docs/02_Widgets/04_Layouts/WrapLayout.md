# WrapLayout

The `WrapLayout` widget arranges child elements in rows that automatically wrap to the next line when they reach the end of the available horizontal space. It's perfect for creating responsive layouts like tag lists, button groups, or card grids that adapt to different screen sizes.

## Basic Usage

Here's a simple wrap layout with text elements:

```csharp demo-tabs
new WrapLayout([
    Text.Literal("First Item"),
    Text.Literal("Second Item"), 
    Text.Literal("Third Item"),
    Text.Literal("Fourth Item")
])
```

`WrapLayout` also works effectively with badge components:

```csharp demo-tabs
new WrapLayout([
    new Badge("React", BadgeVariant.Secondary),
    new Badge("Vue", BadgeVariant.Secondary),
    new Badge("Angular", BadgeVariant.Secondary),
    new Badge("Svelte", BadgeVariant.Secondary),
    new Badge("Next.js", BadgeVariant.Secondary),
    new Badge("Nuxt.js", BadgeVariant.Secondary)
])
```

## Properties

### Gap

Control the spacing between items in the wrap layout:

```csharp demo-tabs
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

```csharp demo-tabs
new WrapLayout([
    new Badge("Padded"),
    new Badge("Content"),
    new Badge("Items")
], padding: new Thickness(4))
```

### Margin

Add external spacing around the wrap layout:

```csharp demo-tabs
new WrapLayout([
    new Badge("With"),
    new Badge("Margin")
], margin: new Thickness(4))
```

### Combined Properties

Combining different properties for various layouts:

```csharp demo-tabs
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
], gap: 6, padding: new Thickness(8), margin: new Thickness(4))
```

<WidgetDocs Type="Ivy.WrapLayout" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Layouts/WrapLayout.cs"/>

## Advanced Examples

### Tag List

Create a responsive tag list that wraps nicely:

```csharp demo-tabs
new WrapLayout([
    new Badge("JavaScript", BadgeVariant.Primary),
    new Badge("TypeScript", BadgeVariant.Secondary),
    new Badge("React", BadgeVariant.Primary),
    new Badge("Vue.js", BadgeVariant.Secondary),
    new Badge("Angular", BadgeVariant.Outline),
    new Badge("Svelte", BadgeVariant.Destructive),
    new Badge("Node.js", BadgeVariant.Primary),
    new Badge("Express", BadgeVariant.Secondary),
    new Badge("MongoDB", BadgeVariant.Primary),
    new Badge("PostgreSQL", BadgeVariant.Outline)
], gap: 2)
```

### Button Group

Create responsive button groups:

```csharp demo-tabs
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

```csharp demo-tabs
new WrapLayout([
    new Card().Title("Project Alpha").Description("Development project"),
    new Card().Title("Project Beta").Description("Testing phase")
], gap: 4)
```
