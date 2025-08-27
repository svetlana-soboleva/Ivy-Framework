# Signals

<Ingress>
Signals enable inter-component communication in Ivy applications, allowing components to send and receive messages across the component tree.
</Ingress>

## Overview

Signals are communication primitives that allow different parts of your application to communicate with each other. They follow a publisher-subscriber pattern where components can send messages through signals and other components can listen for and respond to those messages.

## Basic Usage

First, define a signal by creating a class that inherits from `AbstractSignal<TInput, TOutput>`:

```csharp
public class CounterSignal : AbstractSignal<int, string> { }
```

Then use the signal in your application:

```csharp demo-below
public class SignalExample : ViewBase
{
    public override object? Build()
    {
        var signal = Context.CreateSignal<CounterSignal, int, string>();
        var output = UseState("");

        async void OnClick(Event<Button> _)
        {
            var results = await signal.Send(1);
            output.Set(string.Join(", ", results));
        }

        return Layout.Vertical(
            new Button("Send Signal", OnClick),
            new ChildReceiver(),
            output
        );
    }
}

public class ChildReceiver : ViewBase
{
    public override object? Build()
    {
        var signal = Context.UseSignal<CounterSignal, int, string>();
        var counter = UseState(0);

        UseEffect(() => signal.Receive(input =>
        {
            counter.Set(counter.Value + input);
            return $"Child received: {input}, total: {counter.Value}";
        }));

        return new Card($"Counter: {counter.Value}");
    }
}
```

## Signal Communication Patterns

### One-to-Many Communication

This example demonstrates the one-to-many pattern where one sender broadcasts a message that multiple receivers all receive simultaneously.

```csharp demo-tabs
public class BroadcastSignal : AbstractSignal<string, Unit> { }

public class OneToManyDemo : ViewBase
{
    public override object? Build()
    {
        var signal = Context.CreateSignal<BroadcastSignal, string, Unit>();
        var message = UseState("");
        var receiver1Message = UseState("");
        var receiver2Message = UseState("");
        var receiver3Message = UseState("");
        
        async void BroadcastMessage(Event<Button> _)
        {
            if (!string.IsNullOrWhiteSpace(message.Value))
            {
                await signal.Send(message.Value);
                message.Set("");
            }
        }
        
        // Set up receivers
        UseEffect(() => 
        {
            var receiver = Context.UseSignal<BroadcastSignal, string, Unit>();
            var subscription = receiver.Receive(message =>
            {
                receiver1Message.Set(message);
                receiver2Message.Set(message);
                receiver3Message.Set(message);
                return new Unit();
            });
            return subscription;
        });
        
        return Layout.Vertical(
            Layout.Horizontal(
                message.ToTextInput("Broadcast Message"),
                new Button("Send to ALL", BroadcastMessage)
            ),
            Layout.Horizontal(
                Layout.Vertical(
                    new Card(Text.Block(receiver1Message.Value))
                ),
                Layout.Vertical(
                    new Card(Text.Block(receiver2Message.Value))
                ),
                Layout.Vertical(
                    new Card(Text.Block(receiver3Message.Value))
                )
            )
        );
    }
}
```

### Request-Response Pattern

```csharp demo-tabs
public class DataRequestSignal : AbstractSignal<string, string[]> { }

public class DataRequester : ViewBase
{
    public override object? Build()
    {
        var signal = Context.CreateSignal<DataRequestSignal, string, string[]>();
        var query = UseState("");
        var results = UseState<string[]>(() => Array.Empty<string>());
        async void SearchData(Event<Button> _)
        {
            var responses = await signal.Send(query.Value);
            var allResults = responses.SelectMany(r => r).ToArray();
            results.Set(allResults);
        }
        
        return Layout.Vertical(
            query.ToTextInput("Search Query"),
            new Button("Search", SearchData),
            Layout.Vertical(results.Value.Select(r => Text.Literal(r)))
        );
    }
}

public class DataProvider : ViewBase
{
    public override object? Build()
    {
        var signal = Context.UseSignal<DataRequestSignal, string, string[]>();
        var processedQueries = UseState(0);
        
        UseEffect(() => signal.Receive(query =>
        {
            processedQueries.Set(processedQueries.Value + 1);
            // Simulate data processing
            return new[] { $"Result 1 for '{query}'", $"Result 2 for '{query}'" };
        }));
        
        return new Card($"Processed {processedQueries.Value} queries");
    }
}
```

## Broadcast Types

Signals can be configured to broadcast across different scopes:

### App-Level Broadcasting

```csharp
[Signal(BroadcastType.App)]
public class AppNotificationSignal : AbstractSignal<string, Unit> { }
```

### Server-Level Broadcasting

```csharp
[Signal(BroadcastType.Server)]
public class ServerEventSignal : AbstractSignal<ServerEvent, Unit> { }
```

### Machine-Level Broadcasting

```csharp
[Signal(BroadcastType.Machine)]
public class SystemSignal : AbstractSignal<SystemEvent, Unit> { }
```

## Best Practices

1. **Single Purpose**: Each signal should handle one type of communication.
2. **Type Safety**: Use strongly typed input and output parameters.
3. **Error Handling**: Signal receivers should handle exceptions gracefully.
4. **Cleanup**: Dispose of signal subscriptions in UseEffect cleanup.
5. **Broadcasting**: Use appropriate broadcast types for your use case.

## Examples

### Chat System with Signals

```csharp demo-tabs
public class ChatSignal : AbstractSignal<string, Unit> { }

public class ChatSender : ViewBase
{
    public override object? Build()
    {
        var signal = Context.CreateSignal<ChatSignal, string, Unit>();
        var message = UseState("");
        
        async void SendMessage(Event<Button> _)
        {
            if (!string.IsNullOrEmpty(message.Value))
            {
                await signal.Send(message.Value);
                message.Set("");
            }
        }
        
        return Layout.Horizontal(
            message.ToTextInput("Type a message..."),
            new Button("Send", SendMessage)
        );
    }
}

public class ChatReceiver : ViewBase
{
    public override object? Build()
    {
        var signal = Context.UseSignal<ChatSignal, string, Unit>();
        var messages = UseState<List<string>>(() => new List<string>());
        
        UseEffect(() => signal.Receive(message =>
        {
            messages.Set(current => {
                var updated = new List<string>(current) { message };
                return updated.TakeLast(10).ToList(); // Keep last 10 messages
            });
            return new Unit();
        }));
        
        return new Card(
            Layout.Vertical(
                messages.Value.Select(msg => Text.Literal(msg))
            )
        ).Title("Messages");
    }
}
```

### Multi-Component Counter

```csharp demo-tabs
public class CounterSignal : AbstractSignal<int, string> { }

public class CounterController : ViewBase
{
    public override object? Build()
    {
        var signal = Context.CreateSignal<CounterSignal, int, string>();
        var responses = UseState<string[]>(() => Array.Empty<string>());
        
        async void Increment(Event<Button> _)
        {
            var results = await signal.Send(1);
            responses.Set(results);
        }
        
        async void Decrement(Event<Button> _)
        {
            var results = await signal.Send(-1);
            responses.Set(results);
        }
        
        return Layout.Vertical(
            Layout.Horizontal(
                new Button("Increment", Increment),
                new Button("Decrement", Decrement)
            ),
            Layout.Vertical(responses.Value.Select(r => Text.Literal(r)))
        );
    }
}

public class CounterDisplay : ViewBase
{
    public override object? Build()
    {
        var signal = Context.UseSignal<CounterSignal, int, string>();
        var count = UseState(0);
        
        UseEffect(() => signal.Receive(increment =>
        {
            count.Set(count.Value + increment);
            return $"Counter: {count.Value}";
        }));
        
        return new Card($"Count: {count.Value}");
    }
}
```

## See Also

- [State Management](./State.md)
- [Effects](./Effects.md)
