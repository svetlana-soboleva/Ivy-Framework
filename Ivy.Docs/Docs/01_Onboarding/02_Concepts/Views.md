# Views

Views are the fundamental building blocks of Ivy applications. They are similar to React components, providing a way to encapsulate UI logic and state management in a reusable way.

## Overview

A View in Ivy is a class that inherits from `ViewBase` and implements the `Build` method. The `Build` method returns the UI representation of the view, which can be any object that Ivy knows how to render.

## Basic View Structure

```csharp
[App(icon: Icons.Box)]
public class SimpleView : ViewBase
{
    public override object? Build()
    {
        return new TextBlock("Hello, World!");
    }
}
```

## View with State

Views can manage state using the `UseState` hook:

```csharp
public class CounterView : ViewBase
{
    public override object? Build()
    {
        var count = UseState(0);
        
        return Layout.Horizontal(
            new Button("Increment", onClick: _ => count.Set(count.Value + 1)),
            new TextBlock($"Count: {count.Value}")
        );
    }
}
```

## View Composition

Views can be composed to build complex UIs:

```csharp
public class DashboardView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical(
            new HeaderView(),
            new StatsView(),
            new ChartView(),
            new TableView()
        );
    }
}
```

## View Props

Views can accept props through their constructor:

```csharp
public class UserCardView : ViewBase
{
    private readonly User _user;
    
    public UserCardView(User user)
    {
        _user = user;
    }
    
    public override object? Build()
    {
        return Layout.Vertical(
            new TextBlock(_user.Name),
            new TextBlock(_user.Email),
            new Button("View Profile", onClick: _ => Navigate($"/users/{_user.Id}"))
        );
    }
}
```

## View Lifecycle

Views support various lifecycle hooks:

```csharp
public class LifecycleView : ViewBase
{
    public override void OnInit()
    {
        // Called when the view is first created
    }
    
    public override void OnDispose()
    {
        // Called when the view is being disposed
    }
    
    public override object? Build()
    {
        return new TextBlock("Lifecycle Example");
    }
}
```

## View Effects

Views can use effects to handle side effects:

```csharp
public class DataView : ViewBase
{
    public override object? Build()
    {
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
        
        return isLoading.Value 
            ? new Spinner()
            : data.Value != null
                ? new DataDisplay(data.Value)
                : new TextBlock("No data available");
    }
}
```

## Best Practices

1. **Single Responsibility**: Each view should have a single, well-defined purpose.
2. **State Management**: Use appropriate state management hooks (`UseState`, `UseSignal`).
3. **Props**: Pass data through props rather than accessing global state.
4. **Composition**: Break down complex views into smaller, reusable components.
5. **Effects**: Use effects for side effects and cleanup.
6. **Performance**: Avoid unnecessary re-renders by using appropriate state management.

## Examples

### Form View with Validation

```csharp
public class RegistrationView : ViewBase
{
    public override object? Build()
    {
        var formData = UseState(new RegistrationData());
        var errors = UseState(new Dictionary<string, string>());
        
        return new Form(
            onSubmit: async () => {
                if (!ValidateForm(formData.Value, out var validationErrors))
                {
                    errors.Set(validationErrors);
                    return;
                }
                await api.Register(formData.Value);
            }
        )
        | new TextInput("Email", value: formData.Value.Email, onChange: v => formData.Set(v))
        | new PasswordInput("Password", value: formData.Value.Password, onChange: v => formData.Set(v))
        | errors
        | new Button("Register");
    }
}
```

### Dashboard with Real-time Updates

```csharp
public class DashboardView : ViewBase
{
    public override object? Build()
    {
        var metrics = UseState(new Metrics());
        var lastUpdate = UseState(DateTime.Now);
        
        UseEffect(async () => {
            var timer = new Timer(async _ => {
                var newMetrics = await api.GetMetrics();
                metrics.Set(newMetrics);
                lastUpdate.Set(DateTime.Now);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            
            return () => timer.Dispose();
        }, EffectTrigger.AfterInit());
        
        return Layout.Vertical(
            new TextBlock($"Last Updated: {lastUpdate.Value}"),
            new MetricsDisplay(metrics.Value),
            new ChartView(metrics.Value)
        );
    }
}
```

## See Also

- [State Management](./State.md)
- [Effects](./Effects.md)
- [Signals](./Signals.md)
- [Widgets](./Widgets.md)

----------------------------------------------------------------

# Views

A view is the fundamental unit of an Ivy application.  Every view derives from `ViewBase` and overrides the `Build()` method.  The `Build()` method returns either another view or a widget.  Widgets are provided by the `Ivy.Widgets` library and cover common UI elements such as buttons, forms and layout containers.

## A minimal example

```csharp
public class OrdersApp : ViewBase
{
    public override object? Build()
    {
        return this.UseBlades(() => new OrderListBlade(), "Search");
    }
}
```

The framework instantiates `OrdersApp` and calls `Build()` to obtain the UI to display.  Hooks like `UseBlades`, `UseRefreshToken` and `UseService` can be called inside `Build()` to access shared services, open dialogs or manage navigation.

Views can be nested.  For example `OrderListBlade` uses `FilteredListView` to show search results and opens other blades when the user selects an item.

## Returning arbitrary objects

If `Build()` returns a plain C# object (for example a `List<T>`), Ivy will attempt to choose a suitable widget automatically.  Returning a list of records renders a table with the properties as columns.

## Layout

Complex interfaces are composed using layout widgets.  The database generator application demonstrates a typical pattern:

```csharp
return Layout.Horizontal().Align(Align.Center).Height(Size.Screen()).RemoveParentPadding()
       | (Layout.Vertical().Width(150)
          | new IvyLogo()
          | ...);
```

The pipe (`|`) operator combines widgets into containers and results in concise, readable view code.
