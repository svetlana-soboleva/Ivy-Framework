# State

State management is a fundamental concept in Ivy that allows you to handle and update data within your views. Ivy provides several mechanisms for managing state, each suited for different use cases.

## Overview

Ivy offers multiple ways to manage state:

1. **UseState**: For simple state management within a view
2. **UseSignal**: For reactive state management with computed values
3. **UseStore**: For global state management across multiple views

## UseState

`UseState` is the most basic form of state management in Ivy, similar to React's `useState`:

```csharp
public class CounterView : ViewBase
{
    public override object? Build()
    {
        var count = UseState(0);
        
        return Layout.Horizontal(
            new Button("-", onClick: _ => count.Set(count.Value - 1)),
            new TextBlock($"Count: {count.Value}"),
            new Button("+", onClick: _ => count.Set(count.Value + 1))
        );
    }
}
```

## UseSignal

`UseSignal` provides reactive state management with computed values:

```csharp
public class FormView : ViewBase
{
    public override object? Build()
    {
        var firstName = UseSignal("");
        var lastName = UseSignal("");
        
        // Computed signal
        var fullName = UseSignal(() => 
            $"{firstName.Value} {lastName.Value}".Trim());
        
        // Computed signal with validation
        var isValid = UseSignal(() => 
            !string.IsNullOrEmpty(fullName.Value));
        
        return Layout.Vertical(
            new TextInput("First Name", value: firstName.Value, onChange: v => firstName.Set(v)),
            new TextInput("Last Name", value: lastName.Value, onChange: v => lastName.Set(v)),
            new TextBlock($"Full Name: {fullName.Value}"),
            new Button("Submit", disabled: !isValid.Value)
        );
    }
}
```

## UseStore

`UseStore` is used for global state management:

```csharp
public class UserStore
{
    public string? Name { get; set; }
    public bool IsAuthenticated { get; set; }
}

public class AppView : ViewBase
{
    public override object? Build()
    {
        var userStore = UseStore(new UserStore());
        
        return Layout.Vertical(
            userStore.Value.IsAuthenticated
                ? new TextBlock($"Welcome, {userStore.Value.Name}!")
                : new LoginForm()
        );
    }
}
```

## State Updates

### Synchronous Updates

```csharp
var count = UseState(0);
count.Set(count.Value + 1); // Direct update
```

### Asynchronous Updates

```csharp
var data = UseState<Data?>(null);
var isLoading = UseState(false);

UseEffect(async () => {
    isLoading.Set(true);
    try {
        var result = await api.GetData();
        data.Set(result);
    } finally {
        isLoading.Set(false);
    }
}, EffectTrigger.AfterInit());
```

## Best Practices

1. **State Location**: Keep state as close as possible to where it's used
2. **State Updates**: Use appropriate update methods based on your needs
3. **State Initialization**: Provide meaningful initial values
4. **State Dependencies**: Be mindful of state dependencies and their effects
5. **State Cleanup**: Clean up state when components are disposed

## Examples

### Form State Management

```csharp
public class RegistrationForm : ViewBase
{
    public override object? Build()
    {
        var formData = UseState(new RegistrationData());
        var errors = UseState(new Dictionary<string, string>());
        var isSubmitting = UseState(false);
        
        return Layout.Vertical(
            new TextInput("Email", value: formData.Value.Email, onChange: v => formData.Set(v)),
            new TextInput("Password", value: formData.Value.Password, onChange: v => formData.Set(v)),
            errors.Value.Any() ? new Alert(errors.Value.Values.First()) : null,
            new Button("Register", disabled: isSubmitting.Value)
        );
    }
}
```

### Real-time Data Updates

```csharp
public class LiveDataView : ViewBase
{
    public override object? Build()
    {
        var data = UseState(new List<DataPoint>());
        var lastUpdate = UseState(DateTime.Now);
        
        UseEffect(() => {
            var subscription = dataService.SubscribeToUpdates(
                updates => {
                    data.Set(updates);
                    lastUpdate.Set(DateTime.Now);
                }
            );
            
            return () => subscription.Dispose();
        }, EffectTrigger.AfterInit());
        
        return Layout.Vertical(
            new TextBlock($"Last Update: {lastUpdate.Value:HH:mm:ss}"),
            new Chart(data.Value)
        );
    }
}
```

## See Also

- [Effects](./Effects.md)
- [Signals](./Signals.md)
- [Memoization](./Memoization.md)

------------------------------------------------

State in Ivy is represented by the generic `IState<T>` interface.  Views obtain state via the `UseState` hook:

```csharp
var count = UseState(0);
```

The hook returns an `IState<int>` which exposes the current `Value` and a `Set(...)` method to update it.  When `Set` is called Ivy schedules a re-render of the view that owns the state.

State is scoped to the view instance.  Re-instantiating a view (for example by navigating away and back) will create fresh state values unless they are explicitly passed in as props.

You can react to changes with the `UseEffect` hook.  The following example taken from `OrderDetailsBlade.cs` loads data when the view is first created or when a refresh token changes:

```csharp
UseEffect(async () =>
{
    var db = factory.CreateDbContext();
    order.Set(await db.Orders.Include(e => e.Customer)
        .SingleOrDefaultAsync(e => e.Id == orderId));
}, [EffectTrigger.AfterInit(), refreshToken]);
```

Effects run after the view has been rendered.  Ivy automatically tracks the dependencies supplied in the array and only reruns the effect when one of them has changed.
