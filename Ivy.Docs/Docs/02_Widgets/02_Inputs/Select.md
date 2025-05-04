# SelectInput

The SelectInput widget provides a dropdown menu for selecting items from a predefined list of options. It supports single and multiple selections, option grouping, and custom rendering of option items.

## Basic Usage

Here's a simple example of a SelectInput with a few options:

```csharp
enum Color { Red, Green, Blue }

var colorState = this.UseState(Color.Red);
var colorInput = colorState.ToSelectInput(typeof(Color).ToOptions());
```

```csharp
var colorState = this.UseState(Color.Red);
var colorInput = colorState.ToSelectInput(typeof(Color).ToOptions());
```

## Variants

SelectInput supports several variants:

```csharp
Layout.Horizontal()
    | colorState.ToSelectInput(typeof(Color).ToOptions())
    | colorState.ToSelectInput(typeof(Color).ToOptions()).Variant(SelectInputs.List)
    | colorState.ToSelectInput(typeof(Color).ToOptions()).Variant(SelectInputs.Toggle)
```

## Multiple Selections

Enable multiple selections using the `SelectMany` property:

```csharp
var colorsState = this.UseState<Color[]>();
var colorsInput = colorsState.ToSelectInput(typeof(Color).ToOptions()).SelectMany();
```

## Event Handling

Handle change events using the `OnChange` parameter:

```csharp
var colorState = this.UseState(Color.Red);
var colorInput = colorState.ToSelectInput(typeof(Color).ToOptions(), onChange: e => Console.WriteLine($"Selected: {e.Value}"));
```

## Styling

Customize the SelectInput with various styling options:

```csharp
var colorInput = colorState.ToSelectInput(typeof(Color).ToOptions())
    .Placeholder("Select a color")
    .Disabled()
    .Invalid("Invalid selection");
```

<WidgetDocs Type="Ivy.SelectInput" ExtensionTypes="Ivy.SelectInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/SelectInput.cs"/>

## Examples

### Using with Different Data Types

```csharp
enum Size { Small, Medium, Large }

var sizeState = this.UseState(Size.Medium);
var sizeInput = sizeState.ToSelectInput(typeof(Size).ToOptions());
```

### Integrating into a Form

```csharp
var formState = this.UseState(new { Color = Color.Red, Size = Size.Medium });
var form = formState.ToForm()
    .Builder(m => m.Color, s => s.ToSelectInput(typeof(Color).ToOptions()))
    .Builder(m => m.Size, s => s.ToSelectInput(typeof(Size).ToOptions()));
``` 