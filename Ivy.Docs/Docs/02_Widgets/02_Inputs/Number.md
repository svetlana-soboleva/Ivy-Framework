# NumberInput
The `NumberInput` widget provides an input field specifically for numeric values. It includes validation for numeric entries and options for 
setting minimum/maximum values, step increments, and formatting.

## Basic Usage
Here's a simple example of a `NumberInput` that allows users to input a number. It also allows to set a minimum
and a maximum limit.

```csharp demo-below
public class SimpleNumericValueDemo : ViewBase
{
    public override object? Build()
    {
        var value = UseState(0);
        return Layout.Horizontal() 
           | new NumberInput<double>(value)
             .Min(-10)
             .Max(10);
    }
}
```

## Variants
NumberInputs come in several variants to suit different use cases

### Slider
This variant helps create a slider that changes the value as the slider is pulled.
This creates the `NumberInputs.Slider` variant.

```csharp demo-below
public class NumberSliderInput : ViewBase 
{
    public override object? Build()
    {        
        var books = UseState(1);
        var cart = UseState("");
        return Layout.Horizontal()
            | Text.Block("Books")
            | new NumberInput<int>(
                books.Value,
                e => {
                        books.Set(e);
                        cart.Set($"Added {books} books to your cart"); 
                    }
            )
            .Min(1)
            .Max(5)
            .Variant(NumberInputs.Slider)
            | Text.Block(cart);
    }
}
```


### Money
To enable users to enter money amounts, this variant should be used. The extension function `ToMoneyInput`
should be used to create this variant. This is the idiomatic way to use Ivy.

```csharp demo-below
public class MoneyInputDemo : ViewBase 
{
    public override object? Build()
    {
        var moneyInUSD = UseState(0);
        var moneyInGBP = UseState(0);
        var moneyInEUR = UseState(0);
        return Layout.Vertical()
            | moneyInUSD.ToMoneyInput(placeholder: "Amount in USD", currency : "USD")
            | moneyInGBP.ToMoneyInput(placeholder: "Amount in GBP", currency : "GBP")
            | moneyInEUR.ToMoneyInput(placeholder: "Amount in EUR", currency : "EUR");      
    }
}
```

## Styling
`NumberInput`s can be customized with various styling options:

### Invalid
To render a red border around the number input and mark the input as invalid, this style should be used.
This can be used via the extension function `Invalid`.

```csharp demo-below
public class InvalidDemo : ViewBase
{
    public override object? Build()
    {
        var num = UseState(3.14);
        return Layout.Vertical()
            | num.ToNumberInput().Invalid("Value should be less than 3.1");
    }
}
```

### Disabled
To disable a number input this style should be used. This can be used via the extension function `Disabled`.

```csharp demo-below
public class DisabledDemo : ViewBase
{
    public override object? Build()
    {
        var num = UseState(3.14);
        return Layout.Vertical()
            | num.ToNumberInput().Disabled();
    }
}
```

### FormatStyle
There are three different kinds of formats that a number input can have. The following shows these in action.

```csharp demo-below

public class FormatStyleDemos : ViewBase
{
    public override object? Build()
    {
        var num = UseState(3.14);
        var amount = UseState(30.14);
        var passingPercentage = UseState(0.35);
        
        return Layout.Vertical()
            | num.ToNumberInput().FormatStyle(NumberFormatStyle.Decimal)
            | amount.ToNumberInput().FormatStyle(NumberFormatStyle.Currency).Currency("GBP")
            | passingPercentage.ToNumberInput(variant: NumberInputs.Number).FormatStyle(NumberFormatStyle.Percent);
    }
}

```

### Currency

```csharp demo-below
public class MoneyPrecisionDemo : ViewBase
{
    public override object? Build()
    {
        var precValue = UseState(0.50);
      return Layout.Horizontal() 
         |    new NumberInput<double>(precValue)
              .Min(0.0)
              .Max(100.0)
              .Step(0.5)
              .Precision(2)
              .FormatStyle(NumberFormatStyle.Currency)
              .Currency("USD");
    }
}
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

<WidgetDocs Type="Ivy.NumberInput" ExtensionTypes="Ivy.NumberInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/NumberInput.cs"/>


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