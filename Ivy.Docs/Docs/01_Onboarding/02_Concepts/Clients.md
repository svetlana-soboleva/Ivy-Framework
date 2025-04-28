# Clients

Clients in Ivy are the bridge between your server-side C# code and the client-side React components. They provide a seamless way to interact with the browser and handle client-side operations.

## Overview

The `IClientProvider` interface is the main entry point for client-side interactions. It's available through dependency injection using the `UseService` hook:

```csharp
var client = this.UseService<IClientProvider>();
```

## Common Use Cases

### Showing Toasts

```csharp
client.Toast("Operation successful!");
```

### Opening Dialogs

```csharp
client.Dialog(new DialogContent("Are you sure?", "This action cannot be undone."));
```

### Navigation

```csharp
client.Navigate("/some/path");
```

### File Operations

```csharp
// Download a file
client.DownloadFile("data.csv", csvContent);

// Upload files
client.UploadFiles(async files => {
    foreach (var file in files)
    {
        // Process uploaded file
    }
});
```

## Best Practices

1. **Dependency Injection**: Always use `UseService<IClientProvider>()` to get the client instance.
2. **Error Handling**: Wrap client operations in try-catch blocks when appropriate.
3. **Async Operations**: Use async/await for operations that might take time.
4. **State Management**: Use clients in combination with state management for reactive updates.

## Examples

### Form Submission with Toast Feedback

```csharp
public class FormView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var form = UseState(new FormData());

        return new Form(
            onSubmit: async () => {
                try {
                    await api.SubmitForm(form.Value);
                    client.Toast("Form submitted successfully!");
                } catch {
                    client.Toast("Error submitting form", ToastType.Error);
                }
            }
        );
    }
}
```

### File Upload with Progress

```csharp
public class UploadView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var progress = UseState(0);

        return new Button("Upload Files", onClick: _ => {
            client.UploadFiles(async files => {
                foreach (var file in files)
                {
                    await UploadWithProgress(file, p => progress.Set(p));
                }
                client.Toast("Upload complete!");
            });
        });
    }
}
```

## See Also

- [Forms](./Forms.md)
- [State Management](./State.md)
- [Effects](./Effects.md)