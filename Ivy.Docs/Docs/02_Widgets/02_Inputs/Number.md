# NumberInput

The NumberInput widget provides an input field specifically for numeric values. It includes validation for numeric entries and options for setting minimum/maximum values, step increments, and formatting.

## Basic Usage

Here's a simple example of a NumberInput that allows users to input a number:

```csharp
double value = 0;
new NumberInput<double>(value, newValue => value = newValue);
```

```csharp
new NumberInput<double>(0, newValue => Console.WriteLine($"New value: {newValue}"))
```

## Variants

NumberInputs come in several variants to suit different use cases:

```csharp 
Layout.Horizontal()
    | new NumberInput<int>(0)
    | new NumberInput<int>(0).Variant(NumberInputs.Slider)
```

## Event Handling

NumberInputs can handle change and blur events:

```csharp
var onChangedState = UseState(0);
var onChangeLabel = UseState("");

new NumberInput<int>(onChangedState.Value, e =>
{
    onChangedState.Set(e);
    onChangeLabel.Set("Changed");
});
```

## Styling

NumberInputs can be customized with various styling options:

```csharp
new NumberInput<double>(0)
    .Min(0)
    .Max(100)
    .Step(0.5)
    .Precision(2)
    .FormatStyle(NumberFormatStyle.Currency)
```

## Examples

### Advanced Usage

```csharp
var intValue = UseState(12345);
intValue.ToNumberInput().Placeholder("Enter a number")
    .Min(0)
    .Max(10000)
    .Step(1)
    .Precision(0)
    .FormatStyle(NumberFormatStyle.Decimal);
``` 

<WidgetDocs Type="Ivy.NumberInput" ExtensionTypes="Ivy.NumberInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/NumberInput.cs"/>