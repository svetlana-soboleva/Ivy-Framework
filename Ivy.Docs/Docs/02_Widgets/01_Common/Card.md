---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# Card

The Card widget is a versatile container used to group related content and actions in Ivy applications. It can hold text, buttons, charts, and other widgets, making it a fundamental building block for creating structured layouts.

## Basic Usage

Here's a simple example of a card containing text and a button that shows a toast message when clicked:

```csharp
var client = this.UseService<IClientProvider>();
new Card(
    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam nec purus nec nunc",
    new Button("Sign Me Up", _ => client.Toast("You have signed up!"))
).Title("Card App").Description("This is a card app.");
```

## Layout and Composition

Cards can be part of complex layouts, such as grids or vertical stacks, and can be combined with other UI elements:

```csharp demo-tabs
Layout.Vertical().Width(Size.Units(100))
   | new Card("Card 1")
   | new Card("Card 2")
   | new Card("Card 3")
```

## Styling

Cards can be customized with titles, icons, and dimensions to fit the application's theme:

```csharp
new Card().Title("Styled Card").Icon(Icons.ChartBarStacked).Width(1/2f)
```

<WidgetDocs Type="Ivy.Card" ExtensionsType="Ivy.CardExtensions"/>