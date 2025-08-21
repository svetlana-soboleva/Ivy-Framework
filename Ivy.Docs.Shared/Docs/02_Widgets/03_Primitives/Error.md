# Error

<Ingress>
Display error states consistently with standardized messaging, optional details, and recovery options for better user experience.
</Ingress>

The `Error` widget provides a standardized way to display error states in your app. It's designed to communicate that something went wrong and optionally provide details and recovery options.

## Basic Usage

The simplest way to create an Error widget is by passing content directly to the constructor:

```csharp demo-tabs
public class BasicErrorView : ViewBase
{
    public override object? Build()
    {
        return new Error("Connection Failed", "Unable to connect to the server");
    }
}
```

<Callout Type="tip">
Error widgets come with sensible defaults: no title, no message, and no stack trace. You can set these properties individually using the fluent extension methods.
</Callout>

### Title

Set a descriptive title for the error:

```csharp demo-tabs
public class ErrorTitleView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Error().Title("Authentication Failed")
            | new Error().Title("Network Error")
            | new Error().Title("Validation Error");
    }
}
```

### Message

Provide a user-friendly error message:

```csharp demo-tabs
public class ErrorMessageView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Error()
                .Title("Login Failed")
                .Message("Invalid username or password. Please try again.")
            | new Error()
                .Title("File Upload Error")
                .Message("The file size exceeds the maximum allowed limit of 10MB.")
            | new Error()
                .Title("Permission Denied")
                .Message("You don't have sufficient privileges to access this resource.");
    }
}
```

### Stack Trace

Include technical details for debugging (useful for developers):

```csharp demo-tabs
public class ErrorStackTraceView : ViewBase
{
    public override object? Build()
    {
        var stackTrace = @"at System.Net.Http.HttpClient.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
at MyApp.Services.ApiService.GetDataAsync(String endpoint) in /src/Services/ApiService.cs:line 45
at MyApp.Views.DataView.LoadDataAsync() in /src/Views/DataView.cs:line 23";

        return new Error()
            .Title("API Connection Error")
            .Message("Failed to connect to the external service")
            .StackTrace(stackTrace);
    }
}
```

<WidgetDocs Type="Ivy.Error" ExtensionTypes="Ivy.ErrorExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Error.cs"/>

## Examples

### Data Loading with Error Handling

Handle errors gracefully in data loading scenarios:

```csharp demo-tabs
public class DataLoadingView : ViewBase
{
    public override object? Build()
    {
        var isLoading = UseState(false);
        var hasError = UseState(false);
        var data = UseState<List<string>?>();
        
        async Task LoadData()
        {
            try {
                isLoading.Set(true);
                hasError.Set(false);
                
                // Simulate API call with random chance of failure
                await Task.Delay(1000);
                if (Random.Shared.Next(2) == 0)
                    throw new Exception("API connection timeout");
                    
                data.Set(new List<string> { "Item 1", "Item 2", "Item 3" });
            }
            catch {
                hasError.Set(true);
            }
            finally {
                isLoading.Set(false);
            }
        }

        // Initial load
        UseEffect(() => {
            _ = LoadData();
        }, []);
        
        return Layout.Vertical().Gap(4)
            | Layout.Horizontal().Gap(2)
                | new Button("Reload Data", variant: ButtonVariant.Primary).HandleClick(_ => LoadData())
            | (isLoading.Value 
                ? "Loading..." 
                : hasError.Value 
                    ? new Error()
                        .Title("Failed to load data")
                        .Message("There was a problem connecting to the server")
                    : data.Value != null 
                        ? Layout.Vertical() | Text.H3("Data Items") | string.Join(", ", data.Value)
                        : null);
    }
}
```

### Form Validation Errors

Display validation errors in forms:

```csharp demo-tabs
public class FormValidationView : ViewBase
{
    public override object? Build()
    {
        var email = UseState("");
        var password = UseState("");
        var errors = UseState<List<string>>(() => new List<string>());
        
        void ValidateForm()
        {
            var newErrors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(email.Value))
                newErrors.Add("Email is required");
            else if (!email.Value.Contains("@"))
                newErrors.Add("Email must be a valid email address");
                
            if (string.IsNullOrWhiteSpace(password.Value))
                newErrors.Add("Password is required");
            else if (password.Value.Length < 8)
                newErrors.Add("Password must be at least 8 characters long");
                
            errors.Set(newErrors);
        }
        
        return Layout.Vertical().Gap(4)
            | Text.H3("User Registration")
            | Layout.Vertical().Gap(2)
                | Text.Label("Email")
                | email.ToTextInput().Placeholder("Enter your email")
                | Text.Label("Password")
                | password.ToPasswordInput().Placeholder("Enter your password")
            | new Button("Validate", variant: ButtonVariant.Primary).HandleClick(ValidateForm)
            | (errors.Value.Count > 0 
                ? Layout.Vertical().Gap(2) | errors.Value.Select(e => new Error().Message(e))
                : null);
    }
}
```

### Exception Handling

Use the Error widget to display caught exceptions:

```csharp demo-tabs
public class ExceptionHandlingView : ViewBase
{
    public override object? Build()
    {
        var exception = UseState<Exception?>();
        var showDetails = UseState(false);
        
        void SimulateError()
        {
            try
            {
                // Simulate an error
                throw new InvalidOperationException("This is a simulated error for demonstration purposes");
            }
            catch (Exception ex)
            {
                exception.Set(ex);
            }
        }
        
        return Layout.Vertical().Gap(4)
            | new Button("Simulate Error", variant: ButtonVariant.Destructive).HandleClick(SimulateError)
            | (exception.Value != null 
                ? Layout.Vertical().Gap(4)
                    | new Error()
                        .Title(exception.Value.GetType().Name)
                        .Message(exception.Value.Message)
                        .StackTrace(showDetails.Value ? exception.Value.StackTrace : null)
                    | new Button(showDetails.Value ? "Hide Details" : "Show Details")
                        .Variant(ButtonVariant.Outline)
                        .HandleClick(() => showDetails.Set(!showDetails.Value))
                : Text.Muted("Click the button above to simulate an error"));
    }
}
```
