---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# ColorInput

The ColorInput widget provides a color picker interface for selecting color values. It allows users to visually choose colors and returns the selected color in a format suitable for use in styles and themes.

## Basic Usage

Here's a simple example of a ColorInput that updates a state with the selected color:

```csharp
the var colorState = this.UseState("#ff0000");
return Layout.Horizontal(
    colorState.ToColorInput(),
    colorState
);
```

```csharp
var colorState = this.UseState("#ff0000");
return Layout.Horizontal(
    colorState.ToColorInput(),
    colorState
);
```

### Using the Non-Generic Constructor

For convenience, you can create a ColorInput without specifying the generic type, which defaults to `string`:

```csharp
// Using the non-generic constructor (defaults to string)
var colorInput = new ColorInput();

// With placeholder
var colorInputWithPlaceholder = new ColorInput("Choose a color");

// With all options
var colorInputFull = new ColorInput(
    placeholder: "Select your favorite color",
    disabled: false,
    variant: ColorInputVariant.TextAndPicker
);
```

## Variants

The ColorInput supports three variants:

- **Text**: Shows only a text input field
- **Picker**: Shows only a color picker
- **TextAndPicker**: Shows both text input and color picker (default)

```csharp
var colorState = this.UseState("#ff0000");

return Layout.Vertical(
    Text.H3("Text Only"),
    colorState.ToColorInput().Variant(ColorInputVariant.Text),
    
    Text.H3("Picker Only"),
    colorState.ToColorInput().Variant(ColorInputVariant.Picker),
    
    Text.H3("Text and Picker"),
    colorState.ToColorInput().Variant(ColorInputVariant.TextAndPicker)
);
```

## Event Handling

ColorInput can handle change events using the `OnChange` parameter:

```csharp
var colorState = this.UseState("#ff0000");
var onChangeHandler = (Event<IInput<string>, string> e) =>
{
    colorState.Set(e.Value);
};
return new ColorInput(colorState.Value, onChangeHandler);
```

## Styling

ColorInput can be customized with various styling options, such as setting a placeholder or disabling the input:

```csharp
new ColorInput("#ff0000")
    .Placeholder("Select a color")
    .Disabled(false);
```

<WidgetDocs Type="Ivy.ColorInput" ExtensionTypes="Ivy.ColorInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/ColorInput.cs"/>

## Examples

### Basic Color Picker

```csharp
var colorState = this.UseState("#ff0000");
return Layout.Horizontal(
    colorState.ToColorInput(),
    colorState
);
```

### Using Different Variants

```csharp
var colorState = this.UseState("#ff0000");

return Layout.Vertical(
    Text.H3("Text Input Only"),
    colorState.ToColorInput().Variant(ColorInputVariant.Text),
    
    Text.H3("Color Picker Only"),
    colorState.ToColorInput().Variant(ColorInputVariant.Picker),
    
    Text.H3("Both Text and Picker"),
    colorState.ToColorInput().Variant(ColorInputVariant.TextAndPicker)
);
```

### With Custom Styling

```csharp
var colorState = this.UseState("#ff0000");

return colorState.ToColorInput()
    .Placeholder("Choose your favorite color")
    .Disabled(false)
    .Invalid("Please select a valid color");
``` 