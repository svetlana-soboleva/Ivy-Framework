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

## Variants

Currently, the ColorInput does not have specific variants, but it can be customized using properties like `Disabled` and `Placeholder`.

## Event Handling

ColorInput can handle change events using the `OnChange` parameter:

```csharp
var colorState = this.UseState("#ff0000");
var onChangeHandler = (Event<IInput<string>, string> e) =>
{
    colorState.Set(e.Value);
};
return new ColorInput<string>(colorState.Value, onChangeHandler);
```

## Styling

ColorInput can be customized with various styling options, such as setting a placeholder or disabling the input:

```csharp
new ColorInput<string>("#ff0000")
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