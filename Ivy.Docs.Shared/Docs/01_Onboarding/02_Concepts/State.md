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

## State Types and Patterns

### Primitive Types

## State Updates

## State in Forms

## Advanced Patterns

### State with Effects
