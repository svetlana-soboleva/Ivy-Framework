# State

<Ingress>
Master reactive state management in Ivy using hooks like UseState, UseSignal, and UseEffect to build dynamic, responsive applications.
</Ingress>

State management is a fundamental concept in Ivy that allows you to handle and update data within your views. Ivy provides several mechanisms for managing state, each suited for different use cases.

## Basic Usage

### Creating State with UseState

The `UseState` hook is the primary way to create reactive state in Ivy views:

```csharp demo-below
public class CounterApp : ViewBase
{
    public override object? Build()
    {
        var count = UseState(0);
        var name = UseState("World");
        
        return new Card(
            Layout.Vertical(
                Text.H2($"Hello, {name.Value}!"),
                Text.Literal($"Count: {count.Value}"),
                Layout.Horizontal(
                    new Button("Increment", _ => count.Set(count.Value + 1)),
                    new Button("Decrement", _ => count.Set(count.Value - 1)),
                    new Button("Reset", _ => count.Set(0))
                ),
                new Separator(),
                Layout.Horizontal(
                    name.ToTextInput().Placeholder("Your Name"),
                    new Button("Greet", _ => { /* Name will update automatically */ })
                )
            )
        ).Title("Counter Demo");
    }
}
```

### State with Factory Functions

For complex initialization or when you need to defer object creation:

```csharp demo-tabs
public class FactoryStateDemo : ViewBase
{
    public override object? Build()
    {
        // Lazy initialization - only computed once
        var expensiveData = UseState(() => ComputeExpensiveData());
        
        // Complex object with dependencies
        var service = UseState(() => new DataService(GetConfig()));
        
        return Layout.Vertical(
            Text.H2("Factory Functions Demo"),
            Text.Literal($"Data: {expensiveData.Value}"),
            Text.Literal($"Service: {service.Value.Status}"),
            new Button("Refresh", _ => expensiveData.Set(ComputeExpensiveData()))
        );
    }
    
    private string ComputeExpensiveData() => $"Computed at {DateTime.Now:HH:mm:ss}";
    private string GetConfig() => "production";
}

public class DataService
{
    public string Status { get; } = "Connected";
    public DataService(string config) { }
}
```

## State Types and Patterns

```csharp demo-tabs
public class StatePatternsDemo : ViewBase
{
    public override object? Build()
    {
        // Primitive types
        var count = UseState(0);
        var name = UseState("Guest");
        var isActive = UseState(false);
        
        // Collections - use factory functions to avoid ambiguity
        var items = UseState(() => new List<string> { "Item 1", "Item 2" });
        var settings = UseState(() => new Dictionary<string, object>());
        
        // Complex objects - use factory functions to avoid ambiguity
        var user = UseState(() => new User { Name = "John", Age = 25 });
        var config = UseState(() => new AppConfig { Theme = "Dark", Language = "EN" });
        
        // Nullable types
        var selectedItem = UseState(() => (string?)null);
        var lastError = UseState(() => (Exception?)null);
        
        return Layout.Vertical(
            Text.H2("State Types & Patterns"),
            
            // Primitive state
            Layout.Horizontal(
                new Button("Count: " + count.Value, _ => count.Set(count.Value + 1)),
                new Button(name.Value, _ => name.Set("User " + Random.Shared.Next(100))),
                new Button(isActive.Value ? "ON" : "OFF", _ => isActive.Set(!isActive.Value))
            ),
            
            // Collection state
            Layout.Vertical(
                Text.Literal($"Items: {string.Join(", ", items.Value)}"),
                new Button("Add Item", _ => {
                    var newList = new List<string>(items.Value) { $"Item {items.Value.Count + 1}" };
                    items.Set(newList);
                }),
                new Button("Clear", _ => {
                    var emptyList = new List<string>();
                    items.Set(emptyList);
                })
            ),
            
            // Object state
            Layout.Horizontal(
                new Button($"User: {user.Value.Name}", _ => {
                    var newUser = new User { Name = "Jane", Age = 30 };
                    user.Set(newUser);
                }),
                new Button($"Theme: {config.Value.Theme}", _ => {
                    var newConfig = new AppConfig { Theme = "Light", Language = "EN" };
                    config.Set(newConfig);
                })
            ),
            
            // Nullable state
            Layout.Horizontal(
                new Button("Select Item", _ => {
                    var firstItem = items.Value.FirstOrDefault();
                    selectedItem.Set(firstItem);
                }),
                new Button("Clear Selection", _ => {
                    string? nullValue = null;
                    selectedItem.Set(nullValue);
                }),
                new Button("Trigger Error", _ => {
                    Exception? error = new Exception("Demo error");
                    lastError.Set(error);
                })
            ),
            
            // Display current states
            new Separator(),
            Text.Literal($"Selected: {selectedItem.Value ?? "None"}"),
            Text.Literal($"Error: {lastError.Value?.Message ?? "None"}")
        );
    }
}

public class User
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

public class AppConfig
{
    public string Theme { get; set; } = "";
    public string Language { get; set; } = "";
}
```

## State Updates

```csharp demo-tabs
public class StateUpdatesDemo : ViewBase
{
    public override object? Build()
    {
        var count = UseState(0);
        var text = UseState("Hello");
        var items = UseState(() => new List<string> { "Item 1", "Item 2" });
        
        return Layout.Vertical(
            Text.H2("State Updates Demo"),
            
            // Direct updates
            Layout.Horizontal(
                new Button($"Count: {count.Value}", _ => count.Set(count.Value + 1)),
                new Button("Reset", _ => count.Set(0))
            ),
            
            // String updates
            Layout.Horizontal(
                text.ToTextInput("Enter text"),
                new Button("Clear", _ => text.Set("")),
                new Button("Uppercase", _ => text.Set(text.Value.ToUpper()))
            ),
            
            // Collection updates
            Layout.Horizontal(
                new Button("Add Item", _ => {
                    var newItems = new List<string>(items.Value) { $"Item {items.Value.Count + 1}" };
                    items.Set(newItems);
                }),
                new Button("Clear", _ => items.Set(new List<string>()))
            ),
            
            Text.Literal($"Text: {text.Value} | Items: {string.Join(", ", items.Value)}")
        );
    }
}
```

## State in Forms

```csharp demo-tabs
public class FormStateDemo : ViewBase
{
    public override object? Build()
    {
        var name = UseState("");
        var email = UseState("");
        var age = UseState(18);
        var isSubscribed = UseState(false);
        
        return Layout.Vertical(
            Text.H2("Form State Demo"),
            
            name.ToTextInput("Name").Placeholder("Enter your name"),
            email.ToTextInput("Email").Placeholder("Enter your email"),
            age.ToNumberInput("Age").Min(0).Max(120),
            isSubscribed.ToBoolInput("Subscribe to newsletter"),
            
            new Separator(),
            
            Layout.Horizontal(
                new Button("Submit", _ => {
                    // Form submission logic here
                }),
                new Button("Clear", _ => {
                    name.Set("");
                    email.Set("");
                    age.Set(18);
                    isSubscribed.Set(false);
                })
            ),
            
            Text.Literal($"Preview: {name.Value} ({email.Value}) - Age: {age.Value}")
        );
    }
}
```

### State with Effects

```csharp demo-tabs
public class EffectsStateDemo : ViewBase
{
    public override object? Build()
    {
        var count = UseState(0);
        var lastUpdate = UseState(DateTime.Now);
        var isEven = UseState(false);
        
        // Effect that runs when count changes
        UseEffect(() => {
            lastUpdate.Set(DateTime.Now);
            isEven.Set(count.Value % 2 == 0);
        }, [count]);
        
        return Layout.Vertical(
            Text.H2("State with Effects Demo"),
            
            Layout.Horizontal(
                new Button($"Count: {count.Value}", _ => count.Set(count.Value + 1)),
                new Button("Reset", _ => count.Set(0))
            ),
            
            new Separator(),
            
            Text.Literal($"Last Update: {lastUpdate.Value:HH:mm:ss}"),
            Text.Literal($"Is Even: {(isEven.Value ? "Yes" : "No")}")
        );
    }
}
```
