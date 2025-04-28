# Tasks and Observables

Tasks and Observables are powerful features in Ivy for handling asynchronous operations and reactive data streams. They provide different approaches to managing asynchronous data flow in your applications.

## Tasks

Tasks in Ivy are used to handle asynchronous operations that complete once, such as API calls or file operations. They are similar to C#'s `Task` type but with additional features for UI integration.

### Basic Task Usage

```csharp
public class DataLoaderView : ViewBase
{
    public override object? Build()
    {
        var data = UseState<Data?>(null);
        var isLoading = UseState(false);
        var error = UseState<string?>(null);
        
        return Layout.Vertical(
            isLoading.Value ? new Spinner() : null,
            error.Value != null ? new Alert(error.Value) : null,
            data.Value != null ? new DataDisplay(data.Value) : null,
            new Button("Load Data", onClick: async _ => {
                isLoading.Set(true);
                error.Set(null);
                try {
                    var result = await api.GetDataAsync();
                    data.Set(result);
                } catch (Exception ex) {
                    error.Set(ex.Message);
                } finally {
                    isLoading.Set(false);
                }
            })
        );
    }
}
```

### Task with Progress

```csharp
public class FileUploadView : ViewBase
{
    public override object? Build()
    {
        var progress = UseState(0.0);
        var isUploading = UseState(false);
        
        return Layout.Vertical(
            isUploading.Value ? new ProgressBar(progress.Value) : null,
            new Button("Upload File", onClick: async _ => {
                isUploading.Set(true);
                try {
                    await api.UploadFileAsync(
                        file,
                        onProgress: p => progress.Set(p)
                    );
                } finally {
                    isUploading.Set(false);
                    progress.Set(0);
                }
            })
        );
    }
}
```

## Observables

Observables in Ivy are used for handling continuous streams of data, such as real-time updates or event streams. They are similar to Rx.NET's `IObservable<T>` but integrated with Ivy's UI system.

### Basic Observable Usage

```csharp
public class LiveDataView : ViewBase
{
    public override object? Build()
    {
        var data = UseState(new List<DataPoint>());
        
        UseEffect(() => {
            var subscription = dataService.SubscribeToUpdates(
                updates => data.Set(updates)
            );
            
            return () => subscription.Dispose();
        }, EffectTrigger.AfterInit());
        
        return new Chart(data.Value);
    }
}
```

### Observable with Operators

```csharp
public class SearchView : ViewBase
{
    public override object? Build()
    {
        var searchTerm = UseState("");
        var results = UseState(new List<Result>());
        
        UseEffect(() => {
            var subscription = searchTerm
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Where(term => !string.IsNullOrEmpty(term))
                .Select(term => api.SearchAsync(term))
                .Subscribe(results => this.results.Set(results));
            
            return () => subscription.Dispose();
        }, searchTerm);
        
        return Layout.Vertical(
            new TextInput("Search", value: searchTerm.Value, onChange: v => searchTerm.Set(v)),
            new ResultsList(results.Value)
        );
    }
}
```

## Best Practices

1. **Task Management**:
   - Always handle task exceptions
   - Show loading states during task execution
   - Clean up resources when tasks complete
   - Use appropriate task cancellation

2. **Observable Management**:
   - Always dispose of subscriptions
   - Use appropriate operators for data transformation
   - Handle errors in observable chains
   - Consider using retry logic for transient failures

3. **UI Integration**:
   - Update UI state appropriately during async operations
   - Show loading indicators for long-running tasks
   - Display error messages when operations fail
   - Consider using optimistic updates

## Examples

### Real-time Chat Application

```csharp
public class ChatView : ViewBase
{
    public override object? Build()
    {
        var messages = UseState(new List<Message>());
        var newMessage = UseState("");
        var isConnected = UseState(false);
        
        UseEffect(() => {
            var subscription = chatService
                .Connect()
                .Subscribe(
                    onNext: msg => messages.Set(msgs => [...msgs, msg]),
                    onError: ex => client.Toast($"Connection error: {ex.Message}"),
                    onCompleted: () => isConnected.Set(false)
                );
            
            isConnected.Set(true);
            return () => subscription.Dispose();
        }, EffectTrigger.AfterInit());
        
        return Layout.Vertical(
            new TextBlock($"Status: {(isConnected.Value ? "Connected" : "Disconnected")}"),
            new MessageList(messages.Value),
            new TextInput("Message", value: newMessage.Value, onChange: v => newMessage.Set(v)),
            new Button("Send", onClick: async _ => {
                if (string.IsNullOrEmpty(newMessage.Value)) return;
                
                try {
                    await chatService.SendMessageAsync(newMessage.Value);
                    newMessage.Set("");
                } catch (Exception ex) {
                    client.Toast($"Failed to send message: {ex.Message}");
                }
            })
        );
    }
}
```

### Data Synchronization

```csharp
public class SyncView : ViewBase
{
    public override object? Build()
    {
        var syncStatus = UseState<SyncStatus>(SyncStatus.Idle);
        var progress = UseState(0.0);
        
        return Layout.Vertical(
            new TextBlock($"Sync Status: {syncStatus.Value}"),
            syncStatus.Value == SyncStatus.InProgress 
                ? new ProgressBar(progress.Value) 
                : null,
            new Button("Start Sync", onClick: async _ => {
                syncStatus.Set(SyncStatus.InProgress);
                try {
                    await dataService.SyncAsync(
                        onProgress: p => progress.Set(p)
                    );
                    syncStatus.Set(SyncStatus.Completed);
                } catch (Exception ex) {
                    syncStatus.Set(SyncStatus.Failed);
                    client.Toast($"Sync failed: {ex.Message}");
                }
            })
        );
    }
}
```

## See Also

- [Effects](./Effects.md)
- [State](./State.md)
- [Signals](./Signals.md)