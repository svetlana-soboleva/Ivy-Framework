﻿---
prepare: |
  var client = this.UseService<IClientProvider>();
searchHints:
  - click
  - action
  - submit
  - cta
  - interactive
  - control
---

# Button

<Ingress>
Create interactive buttons with multiple variants, states, sizes, and styling options for triggering actions in your Ivy applications.
</Ingress>

The `Button` widget is one of the most fundamental interactive elements in Ivy. It allows users to trigger actions and navigate through your project.

## Basic Usage

Here's a simple example of a button that shows a toast message when clicked:

```csharp
var client = this.UseService<IClientProvider>();
new Button("Click Me", onClick: _ => client.Toast("Hello!"));
```

```csharp demo
new Button("Click Me", onClick: _ => client.Toast("Hello!"))
```

### Variants

`Button`s come in several variants to suit different use cases.

<Callout Type="tip">
Primary is the default variant applied to all buttons. You don't need to explicitly call `.Primary()` unless you want to be explicit in your code for clarity.
</Callout>

```csharp demo-tabs
Layout.Horizontal()
    | new Button("Primary")
    | new Button("Destructive").Destructive()
    | new Button("Secondary").Secondary()
    | new Button("Outline").Outline()
    | new Button("Ghost").Ghost()
    | new Button("Link").Link()
```

### Semantic Button Variants

The Button widget now includes three new contextual variants to help communicate different types of actions to users: Success, Warning, and Info. These variants complement the existing Primary, Secondary, Destructive, Outline, Ghost, and Link options.

```csharp demo-tabs
Layout.Horizontal()
    | new Button("Success", variant: ButtonVariant.Success)
    | new Button("Warning", variant: ButtonVariant.Warning)
    | new Button("Info", variant: ButtonVariant.Info)
```

## Styling Options

### Button States

```csharp demo-tabs
Layout.Vertical().Gap(4)
    | Text.Large("Disabled State")
    | (Layout.Horizontal().Gap(4)
    | new Button("Primary").Disabled()
    | new Button("Secondary").Secondary().Disabled()
    | new Button("Destructive").Destructive().Disabled())
    | Text.Large("Loading State")
    | (Layout.Horizontal().Gap(4)
    | new Button("Primary").Loading()
    | new Button("Secondary").Secondary().Loading()
    | new Button("Destructive").Destructive().Loading())
```

### Border Radius

```csharp demo-tabs
Layout.Grid().Columns(3)
    | new Button("None").BorderRadius(BorderRadius.None)
    | new Button("Rounded").BorderRadius(BorderRadius.Rounded)
    | new Button("Full").BorderRadius(BorderRadius.Full)
    | new Button("None").Secondary().BorderRadius(BorderRadius.None)
    | new Button("Rounded").Secondary().BorderRadius(BorderRadius.Rounded)
    | new Button("Full").Secondary().BorderRadius(BorderRadius.Full)
    | new Button("None").Destructive().BorderRadius(BorderRadius.None)
    | new Button("Rounded").Destructive().BorderRadius(BorderRadius.Rounded)
    | new Button("Full").Destructive().BorderRadius(BorderRadius.Full)
```

### Icons

```csharp demo-tabs
Layout.Vertical().Gap(4)
    | Text.Large("Icons on the Left")
    | (Layout.Horizontal().Gap(4)
    | new Button("Save").Icon(Icons.Save)
    | new Button("Download").Icon(Icons.Download)
    | new Button("Upload").Icon(Icons.Upload)
    | new Button("Settings").Icon(Icons.Settings))
    | Text.Large("Icons on the Right")
    | (Layout.Horizontal().Gap(4)
    | new Button("Next").Icon(Icons.ArrowRight, Align.Right)
    | new Button("Continue").Icon(Icons.ChevronRight, Align.Right)
    | new Button("Submit").Icon(Icons.Send, Align.Right)
    | new Button("Export").Icon(Icons.ExternalLink, Align.Right))
    | Text.Large("Icon-Only Buttons")
    | (Layout.Horizontal().Gap(4)
    | new Button(null, icon: Icons.Plus)
    | new Button(null, icon: Icons.Pen, variant: ButtonVariant.Outline)
    | new Button(null, icon: Icons.Trash, variant: ButtonVariant.Destructive)
    | new Button(null, icon: Icons.Settings, variant: ButtonVariant.Ghost))
    | Text.Large("Combined styling")
    | new Button("Combined Styling Button")
        .Icon(Icons.ArrowRight, Align.Right)
        .BorderRadius(BorderRadius.Full)
        .Large()
```

<WidgetDocs Type="Ivy.Button" ExtensionTypes="Ivy.ButtonExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Button.cs"/>
