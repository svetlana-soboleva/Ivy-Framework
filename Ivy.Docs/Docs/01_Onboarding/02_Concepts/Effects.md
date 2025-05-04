# Effects

The `UseEffect` hook is a powerful feature in Ivy that allows you to perform side effects in your views. It's similar to React's useEffect hook but adapted for Ivy's architecture and patterns.

## Overview

Effects are used to handle side effects in your views, such as:
- Subscribing to data streams
- Making API calls
- Setting up event listeners
- Managing cleanup operations

## Basic Usage

The `UseEffect` hook has several overloads to accommodate different use cases:

```csharp
// For synchronous operations
void UseEffect(Action handler, params IEffectTriggerConvertible[] triggers)

// For asynchronous operations
void UseEffect(Func<Task> handler, params IEffectTriggerConvertible[] triggers)

// For operations that return a disposable for cleanup
void UseEffect(Func<IDisposable> handler, params IEffectTriggerConvertible[] triggers)
void UseEffect(Func<Task<IDisposable>> handler, params IEffectTriggerConvertible[] triggers)
```

## Effect Triggers

Effects can be triggered in different ways using the `triggers` parameter:

1. **AfterInit**: Runs once when the view is initialized
```csharp
UseEffect(() => {
    // Your effect code here
}, EffectTrigger.AfterInit());
```

2. **AfterChange**: Runs when a state value changes
```csharp
var myState = UseState("initial");
UseEffect(() => {
    // Your effect code here
}, EffectTrigger.AfterChange(myState));
```

3. **AfterRender**: Runs after the view has been rendered
```csharp
UseEffect(() => {
    // Your effect code here
}, EffectTrigger.AfterRender());
```

## Cleanup

Effects can return an `IDisposable` for cleanup operations. This is particularly useful for:
- Unsubscribing from observables
- Removing event listeners
- Canceling ongoing operations

```csharp
UseEffect(() => {
    // Setup
    var subscription = someObservable.Subscribe(/* ... */);
    
    // Cleanup
    return () => subscription.Dispose();
});
```

## Examples

### Fetching Data

```csharp
var data = UseState<Data[]>(Array.Empty<Data>());

UseEffect(async () => {
    var result = await apiService.GetDataAsync();
    data.Set(result);
}, EffectTrigger.AfterInit());
```

### Subscribing to an Observable

```csharp
var lastValue = UseState<object?>(null);

UseEffect(() => {
    return observable.Subscribe(value => lastValue.Set(value));
}, EffectTrigger.AfterInit());
```

### Debounced Search

```csharp
var searchResults = UseState(Array.Empty<Result>());
var searchTerm = UseState("");

UseEffect(async () => {
    var results = await searchService.SearchAsync(searchTerm.Value);
    searchResults.Set(results);
}, searchTerm.Throttle(TimeSpan.FromMilliseconds(250)).ToTrigger());
```

## Best Practices

1. **Keep Effects Focused**: Each effect should handle one specific side effect.

2. **Clean Up Resources**: Always return an `IDisposable` when setting up subscriptions or event listeners.

3. **Use Appropriate Triggers**: Choose the right trigger type for your use case:
   - Use `AfterInit` for one-time setup
   - Use `AfterChange` for state-dependent effects
   - Use `AfterRender` for DOM-related operations

4. **Handle Async Operations**: Use the async overloads when performing asynchronous operations.

5. **Avoid Infinite Loops**: Be careful when effects modify state that they depend on, as this can create infinite loops.

## Common Pitfalls

1. **Missing Cleanup**: Not returning an `IDisposable` when setting up subscriptions can lead to memory leaks.

2. **Incorrect Trigger Usage**: Using the wrong trigger type can cause effects to run too often or not often enough.

3. **State Updates in Effects**: Be cautious when updating state in effects, especially if the state is used as a trigger.

## See Also

- [State Management](./State.md)