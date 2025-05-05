---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# Button

The Button widget is one of the most fundamental interactive elements in Ivy. It allows users to trigger actions and navigate through your application.

## Basic Usage

Here's a simple example of a button that shows a toast message when clicked:

```csharp
var client = this.UseService<IClientProvider>();
new Button("Click Me", onClick: _ => client.Toast("Hello!"));
```

```csharp demo
new Button("Click Me", onClick: _ => client.Toast("Hello!"))
```

## Variants

Buttons come in several variants to suit different use cases.

```csharp demo-tabs
Layout.Horizontal()
    | new Button("Default")
    | new Button("Destructive").Destructive()
    | new Button("Secondary").Secondary()
    | new Button("Outline").Outline()
    | new Button("Ghost").Ghost()
    | new Button("Link").Link()
```

## Styling

Buttons can be customized with various styling options.

```csharp demo-below
new Button("Styled Button")
    .Icon(Icons.ArrowRight, Align.Right)
    .BorderRadius(BorderRadius.Full)
    .Large()
```

## States

```csharp demo-tabs
Layout.Horizontal()
    | new Button("Disabled").Disabled()
    | new Button("Loading").Loading()
```

<WidgetDocs Type="Ivy.Button" ExtensionTypes="Ivy.ButtonExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Button.cs"/>