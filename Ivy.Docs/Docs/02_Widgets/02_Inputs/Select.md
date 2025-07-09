# SelectInput

The `SelectInput` widget provides a dropdown menu for selecting items from a predefined list of options. It supports single 
and multiple selections, option grouping, and custom rendering of option items.

## Basic Usage

Here's a simple example of a SelectInput with a few options:

```csharp demo-below
public class SelectColorDemo : ViewBase
{
    
    public override object? Build()
    {    
        var fruits = new string[]{"Apple","Guava","Banana","Watermelon"};
        var dishes = new string[]{"pie", "pickle", "shake", "juice"};
        var guess = this.UseState(fruits[0]);
        var fruitInput = guess.ToSelectInput(fruits.ToOptions())
                                   .Width(35);
        return Layout.Vertical() 
                | Text.Label("Your favourite fruit")
                | fruitInput
                | Text.Label($"{guess}  {dishes[Array.IndexOf(fruits,guess.Value)]} is delicious!");
    }
}    
```


## Variants

`SelectInput` supports several variants

### Select 

Using this variant `SelectInputs.Select`, the options can be rendered as a list of items. The following shows an example usage
of this variant. This is the default variant. This should be used when only one item is expected
to be selected from the inputs.

```csharp demo-below
public class SelectVariantDemo : ViewBase
{
    public override object? Build()
    {
        var langs = new string[]{"C#","Java","Go","JavaScript","F#","Kotlin","VB.NET","Rust"};
        
        var favLang = UseState("");
        return Layout.Vertical() 
                | Text.Label("Select your favourite progrmming language")
                | favLang.ToSelectInput(langs.ToOptions()).Variant(SelectInputs.Select);
    }    
}
```

### List 

If it is required to render the select options as checkboxes, then this variant `SelectInputs.List` should be used.



```csharp demo-below
// Option 1: Single Select (probably what you want)
public class SelectVariantDemoSingleSelect : ViewBase
{
    public override object? Build()
    {
        var options = new List<string>() { "Email", "Phone", "Both" };
        var selectedNotice = UseState("");
        return Layout.Vertical() 
                | Text.Label("How would you like to be notified?") 
                | selectedNotice.ToSelectInput(options.ToOptions())
                                .Variant(SelectInputs.List);                
    }
}
```

### Toggle 

Sometimes it is visually useful to show different options in their own boxes so that those can be toggled. 
The following example shows how to use it. 


```csharp demo-below    
public class MealComboDemo : ViewBase
{
      // Price dictionaries
        private static readonly Dictionary<string, decimal> BurgerPrices = new()
        {
            ["Big Mac"] = 5.99m,
            ["McChicken"] = 3.49m,
            ["Quarter Pounder"] = 6.49m,
            ["Cheeseburger"] = 2.49m,
            [""]  = 0.0m
            
        };
        
        private static readonly Dictionary<string, decimal> FriesPrices = new()
        {
            ["Small Fries"] = 1.99m,
            ["Medium Fries"] = 2.49m,
            ["Large Fries"] = 2.99m,
            [""]  = 0.0m
        };
        
        private static readonly Dictionary<string, decimal> DrinkPrices = new()
        {
            ["Coke"] = 1.99m,
            ["Sprite"] = 1.99m,
            ["Orange Juice"] = 2.29m,
            ["Water"] = 0.00m, // Free!
            [""]  = 0.0m
        };
        
    public override object? Build()
    {
        var burger = UseState("Big Mac");
        var fries = UseState("Medium Fries");
        var drink = UseState("Water");
        
      
        
        return Layout.Vertical()
            | H3("Create your meal")
            | (Layout.Horizontal()
               | Text.Block("Choose Burger")
               | burger.ToSelectInput(new[]{"Big Mac", "McChicken", "Quarter Pounder", "Cheeseburger"}.ToOptions())
                  .Variant(SelectInputs.Toggle))
            | (Layout.Horizontal()
               | Text.Block("Choose Fries")    
               | fries.ToSelectInput(new[]{"Small Fries", "Medium Fries", "Large Fries"}.ToOptions())
                .Variant(SelectInputs.Toggle))
            | (Layout.Horizontal()
               | Text.Block("Choose Drink")
               | drink.ToSelectInput(new[]{"Coke", "Sprite", "Orange Juice", "Water"}.ToOptions())
                .Variant(SelectInputs.Toggle))
            | (Layout.Horizontal()
                | Text.Block("Bill")
                | new NumberInput<decimal>(BurgerPrices[burger.Value] + FriesPrices[fries.Value] +DrinkPrices[drink.Value], _ => { })
                           .FormatStyle(NumberFormatStyle.Currency)
                           .Currency("EUR")
                           .Disabled());
    }
}
```

## Multiple Selections

Enable multiple selections using the `SelectMany` property:


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

### Coffee Condiments Selection

The following 

```csharp demo-tabs
public class CoffeeShopDemo: ViewBase
{
    // Coffee varieties and their typical accompaniments
    private static readonly Dictionary<string, List<string>> CoffeeAccompaniments = new()
    {
        ["Cappuccino"] = new List<string> 
        { 
            "Cinnamon powder", "Cocoa powder", "Sugar cubes", "Biscotti", 
            "Cantuccini", "Amaretti", "Whipped cream" 
        },
        ["Espresso"] = new List<string> 
        { 
            "Lemon peel", "Sugar cubes", "Water", "Chocolate square", 
            "Praline", "Biscotti" 
        },
        ["Latte"] = new List<string> 
        { 
            "Vanilla syrup", "Caramel syrup", "Hazelnut syrup", "Cocoa powder", 
            "Cinnamon", "Croissant", "Muffin", "Steamed milk art" 
        },
        ["Mocha"] = new List<string> 
        { 
            "Whipped cream", "Chocolate shavings", "Cocoa powder", "Marshmallows", 
            "Cinnamon stick", "Caramel drizzle", "Vanilla syrup" 
        },
        ["Americano"] = new List<string> 
        { 
            "Cream", "Milk", "Sugar", "Sweetener", "Lemon slice", 
            "Honey", "Cinnamon" 
        },
        ["Macchiato"] = new List<string> 
        { 
            "Caramel drizzle", "Extra foam", "Vanilla syrup", "Cinnamon", 
            "Whipped cream", "Chocolate shavings" 
        },
        ["Turkish"] = new List<string> 
        { 
            "Turkish delight", "Lokum", "Water", "Brown sugar", 
            "Cardamom", "Rose water", "Pistachio" 
        },
        ["Irish"] = new List<string> 
        { 
            "Whipped cream", "Brown sugar", "Irish whiskey", "Cinnamon", 
            "Nutmeg", "Vanilla extract" 
        }
    };
    
    public override object? Build()
    {
        var coffee = UseState("Cappuccino");
        var selectedCondiments = UseState<List<string>>(new List<string>());
        var previousCoffee = UseState("Cappuccino");
        var lastClickTime = UseState(DateTime.MinValue);
        var coffeeSelected = coffee.Value;
        var coffeeInput = coffee.ToSelectInput(CoffeeAccompaniments.Keys.ToOptions());
        
        if (previousCoffee.Value != coffee.Value)
        {
            selectedCondiments.Set(new List<string>());
            previousCoffee.Set(coffee.Value);
        }
        
        var availableCondiments = CoffeeAccompaniments[coffee.Value];
        
        var condimentMenu = selectedCondiments.ToSelectInput(availableCondiments.ToOptions())
            .SelectMany()
            .Variant(SelectInputs.Toggle);
           
      
        if (selectedCondiments.Value.Count > 0)
        {
            if(selectedCondiments.Value.Count == 1)
            {
                coffeeSelected += "  with " + selectedCondiments
                                            .Value.Aggregate((a,b) =>  a + "," + b);
            }
            else
            {                  
                 coffeeSelected += "  with " + selectedCondiments
                                                 .Value.Take(selectedCondiments.Value.Count - 1)
                                                 .Aggregate((a,b) =>  a + "," + b)
                                                 + " and " + selectedCondiments.Value[selectedCondiments.Value.Count - 1];
            }
        }
        
        return Layout.Vertical()
                | H3("Coffee Condiment Menu")
                | Text.Block("Coffee ")
                | coffeeInput
                | Text.Block("Condiments ")
                | condimentMenu
                | Text.Small("You are ordering...")
                | new Icon(Icons.Coffee) 
                | Text.Small(coffeeSelected);
    }
}
```

