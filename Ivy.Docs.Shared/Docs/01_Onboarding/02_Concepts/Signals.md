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

This example demonstrates the request-response pattern where a requester sends a query and receives specific responses from providers. Unlike one-to-many broadcasting, this pattern expects specific data back from each provider.

```csharp demo-tabs
public class RequestResponseDemo : ViewBase
{
    public override object? Build()
    {
        var query = UseState("");
        var results = UseState<string[]>(() => Array.Empty<string>());
        
        void SearchData(Event<Button> _)
        {
            if (!string.IsNullOrWhiteSpace(query.Value))
            {
                // Simulate request-response: one query gets responses from multiple sources
                var allResults = new List<string>();
                
                // Response from User Database
                var userResults = new[] { "John Doe", "Jane Smith", "Bob Johnson" }
                    .Where(item => item.Contains(query.Value, StringComparison.OrdinalIgnoreCase))
                    .Select(item => "[Users] " + item);
                allResults.AddRange(userResults);
                
                // Response from Product Catalog
                var productResults = new[] { "Laptop", "Smartphone", "Tablet" }
                    .Where(item => item.Contains(query.Value, StringComparison.OrdinalIgnoreCase))
                    .Select(item => "[Products] " + item);
                allResults.AddRange(productResults);
                
                results.Set(allResults.ToArray());
                query.Set("");
            }
        }
        
        return Layout.Vertical(
            Text.Block("Try searching for: John, Jane, Laptop, Smartphone, Tablet"),
            Layout.Horizontal(
                query.ToTextInput("Search"),
                new Button("Search", SearchData)
            ),
            Text.Block($"Found {results.Value.Length} results"),
            Layout.Vertical(
                results.Value.Select(r => Text.Block(r))
            ),
            Layout.Horizontal(
                Layout.Vertical(
                    Text.Block("Users: John Doe, Jane Smith, Bob Johnson"),
                    Text.Block("Products: Laptop, Smartphone, Tablet")
                )
            )
        );
    }
}

public class DataProvider : ViewBase
{
    private readonly string _providerName;
    private readonly string[] _dataSource;
    
    public DataProvider(string providerName, string[] dataSource)
    {
        _providerName = providerName;
        _dataSource = dataSource;
    }
    
    public override object? Build()
    {
        var processedQueries = UseState(0);
        var lastQuery = UseState("");
        
        return new Card(
            Layout.Vertical(
                Text.Block(_providerName),
                Text.Block($"Data source: {string.Join(", ", _dataSource)}"),
                Text.Block($"Processed: {processedQueries.Value} queries"),
                Text.Block($"Last query: {lastQuery.Value}")
            )
        );
    }
}

public class DataRequestSignal : AbstractSignal<string, string[]> { }
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

## Examples

### Chat System with Signals

This example demonstrates how signals can be used to create a real-time chat system where multiple users can send and receive messages simultaneously.

```csharp demo-tabs
public class ChatSignal : AbstractSignal<string, Unit> { }

public class ChatDemo : ViewBase
{
    public override object? Build()
    {
        var message = UseState("");
        var sharedMessages = UseState<List<string>>(() => new List<string>());
        
        void SendMessage(Event<Button> _)
        {
            if (!string.IsNullOrEmpty(message.Value))
            {
                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                var newMessage = $"[{timestamp}] {message.Value}";
                
                sharedMessages.Set(current => {
                    var updated = new List<string>(current) { newMessage };
                    return updated.TakeLast(5).ToList();
                });
                
                message.Set("");
            }
        }
        
        return Layout.Vertical(
            Layout.Horizontal(
                message.ToTextInput("Type a message..."),
                new Button("Send", SendMessage)
            ),

            Layout.Horizontal(
                new ChatReceiver("Alice", sharedMessages.Value),
                new ChatReceiver("Bob", sharedMessages.Value),
                new ChatReceiver("Charlie", sharedMessages.Value)
            )
        );
    }
}

public class ChatReceiver : ViewBase
{
    private readonly string _userName;
    private readonly List<string> _messages;
    
    public ChatReceiver(string userName, List<string> messages)
    {
        _userName = userName;
        _messages = messages;
    }
    
    public override object? Build()
    {
        return new Card(
            Layout.Vertical(
                Text.Block(_userName),
                Text.Block($"Messages: {_messages.Count}"),
                Layout.Vertical(
                    _messages.Select(msg => Text.Block(msg))
                )
            )
        );
    }
}
```

### Multi-Component Counter

```csharp demo-tabs
public class CounterSignal : AbstractSignal<int, string> { }

public class MultiComponentCounterDemo : ViewBase
{
    public override object? Build()
    {
        var sharedCount = UseState(0);
        var lastAction = UseState("");
        
        void Increment(Event<Button> _)
        {
            sharedCount.Set(sharedCount.Value + 1);
            lastAction.Set("Incremented");
        }
        
        void Decrement(Event<Button> _)
        {
            sharedCount.Set(sharedCount.Value - 1);
            lastAction.Set("Decremented");
        }
        
        void Reset(Event<Button> _)
        {
            sharedCount.Set(0);
            lastAction.Set("Reset");
        }
        
        return Layout.Vertical(
            Layout.Horizontal(
                new Button("Increment", Increment),
                new Button("Decrement", Decrement),
                new Button("Reset", Reset)
            ),
            Text.Block($"Current Count: {sharedCount.Value}"),
            Text.Block($"Last Action: {lastAction.Value}"),
            Layout.Horizontal(
                new CounterDisplay("Display 1", sharedCount.Value, lastAction.Value),
                new CounterDisplay("Display 2", sharedCount.Value, lastAction.Value),
                new CounterDisplay("Display 3", sharedCount.Value, lastAction.Value)
            )
        );
    }
}

public class CounterDisplay : ViewBase
{
    private readonly string _name;
    private readonly int _count;
    private readonly string _lastAction;
    
    public CounterDisplay(string name, int count, string lastAction)
    {
        _name = name;
        _count = count;
        _lastAction = lastAction;
    }
    
    public override object? Build()
    {
        return new Card(
            Layout.Vertical(
                Text.Block(_name),
                Text.Block($"Count: {_count}"),
                Text.Block($"Last: {_lastAction}")
            )
        );
    }
}
```

## See Also

- [State Management](./State.md)
- [Effects](./Effects.md)
