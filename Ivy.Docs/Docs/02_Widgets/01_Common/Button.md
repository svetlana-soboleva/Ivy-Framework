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

Buttons come in several variants to suit different use cases:

```csharp demo-tabs
Layout.Horizontal()
    | new Button("Default")
    | new Button("Destructive").Destructive()
    | new Button("Secondary").Secondary()
    | new Button("Outline").Outline()
    | new Button("Ghost").Ghost()
    | new Button("Link").Link()

```

## Icons

Buttons can include icons to enhance their visual appearance and meaning:

```csharp demo-tabs
new Button("Button With Icon")
    .Icon(Icons.ArrowRight, Align.Right)
```

## Event Handling

Buttons can handle click events using the `onClick` parameter:

```csharp
var label = this.UseState("Click a button"); 
var eventHandler = (Event<Button> e) =>
{
    label.Set($"Button {e.Sender.Title} was clicked.");
};
return new Button("Click Me", eventHandler);
```

## Styling

Buttons can be customized with various styling options:

```csharp
new Button("Styled Button")
    .Icon(Icons.ArrowRight, Align.Right)
    .BorderRadius(BorderRadius.Full)
    .Large()
```

<WidgetDocs Type="Ivy.Button" ExtensionTypes="Ivy.ButtonExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Button.cs"/>

## Examples

### Navigation

```csharp
new Button("Delete").Destructive()
    .WithSheet(() => new ConfirmationView())
```