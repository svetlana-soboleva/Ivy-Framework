---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# ColorInput

The ColorInput widget provides a color picker interface for selecting color values. It allows users to visually choose colors and returns the selected color in a format suitable for use in styles and themes.

## Basic Usage

Here's a simple example of a ColorInput that updates a state with the selected color:

```csharp demo-below
public class ColorDemo : ViewBase
{
    public override object? Build()
    {    
        var colorState = this.UseState("#ff0000");
        return Layout.Horizontal()
                | colorState.ToColorInput();
    }   
}
```


```csharp demo-below
public class ColorDemo : ViewBase
{
    public override object? Build()
    {    
        var colorState = this.UseState("#ff0000");
        return Layout.Horizontal()
                | colorState.ToColorInput().Variant(ColorInputVariant.Text);
    }   
}
```


## Variants

`ColorInput` has three variants. `ColorInputVariant.Text` variant should be used to let users enter color hex codes
manually. `ColorInputVariant.TextAndPicker` variant should be used in most cases as it allows users to 
select the color and copy the selected color in the textbox. This is also the default variant. 
Sometimes, it may be necessary to just use the color picker. In such situations `ColorInputVariant.Picker`
should be used. 

The following code shows all these tree variants in a vertical layout. 

```csharp demo-below
public class ColorDemo : ViewBase
{
    public override object? Build()
    {    
        var textColorState = UseState("#ff0000");
        var pickerColorState = UseState("#ff0000");
        var textAndPickerColorState = UseState("#ff0000");
        return Layout.Horizontal()
                | textColorState.ToColorInput().Variant(ColorInputVariant.Text)
                | pickerColorState.ToColorInput().Variant(ColorInputVariant.Picker)
                | textAndPickerColorState.ToColorInput().Variant(ColorInputVariant.TextAndPicker);
    }   
}
```


## Event Handling

ColorInput can handle change events using the `onChange` parameter. 
The following demo shows how the `Picker` variant can be used with a code 
block so that 

```csharp demo-below
public class ColorChangedDemo : ViewBase
{
    public override object? Build()
    {    
        var colorState = this.UseState("#ff0000");
        var colorName = UseState("");
        var onChangeHandler = (Event<IInput<string>, string> e) =>
        {
            colorName.Set(e.Value);
            colorState.Set(e.Value);
        };
        return Layout.Vertical() 
                | new ColorInput<string>(colorState.Value, onChangeHandler)
                      .Variant(ColorInputVariant.Picker) 
                | new Code(colorName.Value)
                     .ShowCopyButton()
                     .ShowBorder();
    }    
}    
```

## Styling

`ColorInput` can be customized with various styling options, such as setting a placeholder or disabling the input.

### Disabled

To render a disabled ColorInput
```csharp
new ColorInput<string>("#ff0000")
    .Placeholder("Select a color")
    .Disabled(false);
```

<WidgetDocs Type="Ivy.ColorInput" ExtensionTypes="Ivy.ColorInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/ColorInput.cs"/>

