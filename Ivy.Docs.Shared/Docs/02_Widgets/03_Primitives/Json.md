# Json

The `Json` widget displays JSON data in a formatted, syntax-highlighted view. It's useful for debugging, data visualization, and displaying API responses.

```csharp demo-tabs ivy-bg
public class ApiResponseViewer : ViewBase
{
    public override object? Build()
    {
        var apiResponse = UseState<string?>();
        var isLoading = UseState(false);
        
              async Task FetchRandomData()
        {
            try
            {
                isLoading.Set(true);
                
                // Generate sample data - in a real app, this would be from an API
                await Task.Delay(500); // Simulate network delay
                
                var sampleData = new
                {
                    id = Guid.NewGuid().ToString(),
                    timestamp = DateTime.Now.ToString("o"),
                    user = new
                    {
                        id = 1234,
                        name = "Jane Doe",
                        email = "jane.doe@example.com",
                        roles = new[] { "admin", "user" }
                    },
                    stats = new
                    {
                        visits = Random.Shared.Next(100, 1000),
                        actions = Random.Shared.Next(10, 100),
                        conversion = Math.Round(Random.Shared.NextDouble(), 2)
                    },
                    settings = new Dictionary<string, object>
                    {
                        ["darkMode"] = Random.Shared.Next(2) == 0,
                        ["notifications"] = true,
                        ["displayDensity"] = "compact"
                    }
                };
                
                apiResponse.Set(System.Text.Json.JsonSerializer.Serialize(sampleData));
            }
            finally
            {
                isLoading.Set(false);
            }
        }
        
        // Fetch data initially
        UseEffect(() => {
            FetchRandomData();
        }, []);
        
        return Layout.Vertical().Gap(4)
            | Text.H1("API Response Viewer")
            | new Button("Fetch New Data", onClick: _ => FetchRandomData()).Disabled(isLoading.Value)
            | (isLoading.Value
                ? "Loading..."
                : apiResponse.Value != null
                    ? new Json(apiResponse.Value)
                    : Text.Muted("No data available"));
    }
}
```

<WidgetDocs Type="Ivy.Json" ExtensionTypes="Ivy.JsonExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Json.cs"/>
