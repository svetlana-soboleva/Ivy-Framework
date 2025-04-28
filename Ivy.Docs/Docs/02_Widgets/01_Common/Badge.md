---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# Badge

The Badge widget is a versatile component used to display small pieces of information, such as counts or statuses, in a compact form.

## Basic Usage

Here's a simple example of a badge.

```csharp demo-below
new Badge("Default")
```

## Variants

```csharp demo-tabs
Layout.Horizontal()
    | new Badge("Default")
    | new Badge("Destructive", variant:BadgeVariant.Destructive)
    | new Badge("Outline", variant:BadgeVariant.Outline)
    | new Badge("Secondary", variant:BadgeVariant.Secondary)
```

## Icons

Badges can include icons to enhance their visual appearance and meaning.

```csharp demo-tabs
new Badge("With Icon", icon:Icons.Bell)
```

<WidgetDocs Type="Ivy.Badge" ExtensionsType="Ivy.BadgeExtensions"/>