# AsyncSelectInput

<Ingress>
Create dropdown selectors that load options asynchronously from APIs or databases, perfect for large datasets and on-demand loading.
</Ingress>

The `AsyncSelectInput` widget provides a select dropdown that loads options asynchronously. It's useful for scenarios where options need to be fetched from an API or when the list of options is large and should be loaded on-demand.

## Basic Usage

Here's a simple example of an `AsyncSelectInput` that fetches categories from a database:

```csharp demo-tabs
public class AsyncSelectBasicDemo : ViewBase
{
    public override object? Build()
    {
        var guidState = this.UseState<Guid?>(default(Guid?));

        async Task<Option<Guid?>[]> QueryCategories(string query)
        {
            // Simulate database query with delay
            await Task.Delay(100);
            
            // Simulate database results
            var categories = new[]
            {
                new { Id = Guid.NewGuid(), Name = "Electronics" },
                new { Id = Guid.NewGuid(), Name = "Clothing" },
                new { Id = Guid.NewGuid(), Name = "Books" },
                new { Id = Guid.NewGuid(), Name = "Home & Garden" },
                new { Id = Guid.NewGuid(), Name = "Sports" }
            };
            
            return categories
                .Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(e => new Option<Guid?>(e.Name, e.Id))
                .ToArray();
        }

        async Task<Option<Guid?>?> LookupCategory(Guid? id)
        {
            if (id == null) return null;
            await Task.Delay(50);
            
            // Simulate database lookup
            var categories = new[]
            {
                new { Id = Guid.NewGuid(), Name = "Electronics" },
                new { Id = Guid.NewGuid(), Name = "Clothing" },
                new { Id = Guid.NewGuid(), Name = "Books" }
            };
            
            var category = categories.FirstOrDefault(c => c.Id == id);
            return category != null ? new Option<Guid?>(category.Name, category.Id) : null;
        }

        return Layout.Vertical()
            | Text.Label("Select a category:")
            | guidState.ToAsyncSelectInput(QueryCategories, LookupCategory, placeholder: "Select Category")
            | Text.Small($"Selected ID: {guidState.Value?.ToString() ?? "None"}");
    }
}
```

## Data Types

AsyncSelectInput supports various data types. Here are examples for different scenarios:

### String-based AsyncSelect

```csharp demo-tabs
public class StringAsyncSelectDemo : ViewBase
{
    public override object? Build()
    {
        var selectedCountry = this.UseState<string?>(default(string));

        async Task<Option<string>[]> QueryCountries(string query)
        {
            // Simulate API call with delay
            await Task.Delay(100);
            
            var countries = new[] { "Germany", "France", "Japan", "China", "USA", "Canada", "Australia", "Brazil" };
            return countries
                .Where(c => c.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(c => new Option<string>(c))
                .ToArray();
        }

        async Task<Option<string>?> LookupCountry(string country)
        {
            if (string.IsNullOrEmpty(country)) return null;
            await Task.Delay(50);
            return new Option<string>(country);
        }

        return Layout.Vertical()
            | Text.Label("Select a country:")
            | selectedCountry.ToAsyncSelectInput(QueryCountries, LookupCountry, placeholder: "Search countries...")
            | Text.Small($"Selected: {selectedCountry.Value ?? "None"}");
    }
}
```

### Integer-based AsyncSelect

```csharp demo-tabs
public class IntegerAsyncSelectDemo : ViewBase
{
    public override object? Build()
    {
        var selectedYear = this.UseState<int?>(default(int));

        async Task<Option<int>[]> QueryYears(string query)
        {
            await Task.Delay(100);
            
            var currentYear = DateTime.Now.Year;
            var years = Enumerable.Range(currentYear - 100, 101).ToArray();
            
            if (string.IsNullOrEmpty(query))
                return years.Take(20).Select(y => new Option<int>(y.ToString(), y)).ToArray();
            
            if (int.TryParse(query, out var yearQuery))
            {
                return years
                    .Where(y => y >= yearQuery && y <= yearQuery + 10)
                    .Take(20)
                    .Select(y => new Option<int>(y.ToString(), y))
                    .ToArray();
            }
            
            return years
                .Where(y => y.ToString().Contains(query))
                .Take(20)
                .Select(y => new Option<int>(y.ToString(), y))
                .ToArray();
        }

        async Task<Option<int>?> LookupYear(int year)
        {
            await Task.Delay(50);
            return new Option<int>(year.ToString(), year);
        }

        return Layout.Vertical()
            | Text.Label("Select a year:")
            | selectedYear.ToAsyncSelectInput(QueryYears, LookupYear, placeholder: "Search years...")
            | Text.Small($"Selected: {selectedYear.Value?.ToString() ?? "None"}");
    }
}
```

### Enum-based AsyncSelect

```csharp demo-tabs
public class EnumAsyncSelectDemo : ViewBase
{
    private enum ProgrammingLanguage
    {
        CSharp,
        Java,
        Python,
        JavaScript,
        Go,
        Rust,
        FSharp,
        Kotlin,
        Swift,
        TypeScript
    }

    public override object? Build()
    {
        var selectedLanguage = this.UseState<ProgrammingLanguage?>(default(ProgrammingLanguage));

        async Task<Option<ProgrammingLanguage>[]> QueryLanguages(string query)
        {
            await Task.Delay(100);
            
            var languages = Enum.GetValues<ProgrammingLanguage>();
            
            if (string.IsNullOrEmpty(query))
                return languages.Select(l => new Option<ProgrammingLanguage>(l.ToString(), l)).ToArray();
            
            return languages
                .Where(l => l.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(l => new Option<ProgrammingLanguage>(l.ToString(), l))
                .ToArray();
        }

        async Task<Option<ProgrammingLanguage>?> LookupLanguage(ProgrammingLanguage language)
        {
            await Task.Delay(50);
            return new Option<ProgrammingLanguage>(language.ToString(), language);
        }

        return Layout.Vertical()
            | Text.Label("Select a programming language:")
            | selectedLanguage.ToAsyncSelectInput(QueryLanguages, LookupLanguage, placeholder: "Search languages...")
            | Text.Small($"Selected: {selectedLanguage.Value?.ToString() ?? "None"}");
    }
}
```

## Advanced Patterns

### Custom Query Logic

Implement complex search logic with multiple criteria:

```csharp demo-tabs
public class AdvancedQueryDemo : ViewBase
{
    public record User
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = "";
        public string Email { get; init; } = "";
        public string Department { get; init; } = "";
        public bool IsActive { get; init; }
    }

    public override object? Build()
    {
        var selectedUser = this.UseState<Guid?>(default(Guid));

        async Task<Option<Guid>[]> QueryUsers(string query)
        {
            await Task.Delay(200); // Simulate API delay
            
            // Simulate user database
            var users = new[]
            {
                new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = false },
                new User { Id = Guid.NewGuid(), Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true }
            };

            if (string.IsNullOrEmpty(query))
                return users.Where(u => u.IsActive).Take(5).Select(u => new Option<Guid>($"{u.Name} ({u.Department})", u.Id)).ToArray();

            var queryLower = query.ToLowerInvariant();
            return users
                .Where(u => u.IsActive && 
                           (u.Name.ToLowerInvariant().Contains(queryLower) || 
                            u.Email.ToLowerInvariant().Contains(queryLower) ||
                            u.Department.ToLowerInvariant().Contains(queryLower)))
                .Take(10)
                .Select(u => new Option<Guid>($"{u.Name} ({u.Department})", u.Id))
                .ToArray();
        }

        async Task<Option<Guid>?> LookupUser(Guid id)
        {
            await Task.Delay(100);
            // In real app, fetch from database
            var users = new[]
            {
                new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true }
            };
            
            var user = users.FirstOrDefault(u => u.Id == id);
            return user != null ? new Option<Guid>($"{user.Name} ({user.Department})", user.Id) : null;
        }

        return Layout.Vertical()
            | Text.Label("Search and select a user:")
            | selectedUser.ToAsyncSelectInput(QueryUsers, LookupUser, placeholder: "Search by name, email, or department...")
            | Text.Small($"Selected User ID: {selectedUser.Value?.ToString() ?? "None"}");
    }
}
```

### Error Handling and Loading States

Handle errors gracefully and show loading states:

```csharp demo-tabs
public class ErrorHandlingDemo : ViewBase
{
    public override object? Build()
    {
        var selectedItem = this.UseState<string?>(default(string));
        var errorMessage = this.UseState<string?>(default(string));

        async Task<Option<string>[]> QueryWithErrors(string query)
        {
            try
            {
                await Task.Delay(300); // Simulate network delay
                
                // Simulate random errors
                if (Random.Shared.Next(10) == 0)
                    throw new Exception("Simulated network error");
                
                var items = new[] { "Apple", "Banana", "Cherry", "Date", "Elderberry" };
                return items
                    .Where(item => item.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Select(item => new Option<string>(item))
                    .ToArray();
            }
            catch (Exception ex)
            {
                errorMessage.Set(ex.Message);
                return Array.Empty<Option<string>>();
            }
        }

        async Task<Option<string>?> LookupWithErrors(string item)
        {
            try
            {
                await Task.Delay(100);
                errorMessage.Set(default(string)); // Clear previous errors
                return new Option<string>(item);
            }
            catch (Exception ex)
            {
                errorMessage.Set($"Lookup failed: {ex.Message}");
                return null;
            }
        }

        return Layout.Vertical()
            | Text.Label("AsyncSelect with error handling:")
            | selectedItem.ToAsyncSelectInput(QueryWithErrors, LookupWithErrors, placeholder: "Search items...")
            | (errorMessage.Value != null ? Text.Block(errorMessage.Value) : null)
            | Text.Small($"Selected: {selectedItem.Value ?? "None"}");
    }
}
```

## Styling and States

Customize the `AsyncSelectInput` with various styling options:

```csharp demo-tabs
public class StylingDemo : ViewBase
{
    public override object? Build()
    {
        var normalSelect = this.UseState<string?>(default(string));
        var invalidSelect = this.UseState<string?>(default(string));
        var disabledSelect = this.UseState<string?>(default(string));

        async Task<Option<string>[]> QueryOptions(string query)
        {
            await Task.Delay(100);
            var options = new[] { "Option 1", "Option 2", "Option 3" };
            return options
                .Where(opt => opt.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(opt => new Option<string>(opt))
                .ToArray();
        }

        async Task<Option<string>?> LookupOption(string option)
        {
            await Task.Delay(50);
            return new Option<string>(option);
        }

        return Layout.Vertical()
            | Text.Label("Normal AsyncSelectInput:")
            | normalSelect.ToAsyncSelectInput(QueryOptions, LookupOption, placeholder: "Choose an option...")
            
            | Text.Label("Invalid AsyncSelectInput:")
            | invalidSelect.ToAsyncSelectInput(QueryOptions, LookupOption, placeholder: "This has an error...")
                .Invalid("This field is required")
            
            | Text.Label("Disabled AsyncSelectInput:")
            | disabledSelect.ToAsyncSelectInput(QueryOptions, LookupOption, placeholder: "This is disabled...")
                .Disabled(true);
    }
}
```

## Performance Considerations

### Debouncing Queries

Implement debouncing to avoid excessive API calls:

```csharp demo-tabs
public class DebouncedQueryDemo : ViewBase
{
    public override object? Build()
    {
        var selectedItem = this.UseState<string?>(default(string));
        var queryCount = this.UseState(0);

        async Task<Option<string>[]> DebouncedQuery(string query)
        {
            var currentCount = queryCount.Value;
            queryCount.Set(currentCount + 1);
            await Task.Delay(500); // Simulate API call
            
            var items = new[] { "Apple", "Banana", "Cherry", "Date", "Elderberry", "Fig", "Grape" };
            return items
                .Where(item => item.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(item => new Option<string>(item))
                .ToArray();
        }

        async Task<Option<string>?> LookupItem(string item)
        {
            await Task.Delay(100);
            return new Option<string>(item);
        }

        return Layout.Vertical()
            | Text.Label("AsyncSelect with query counting:")
            | selectedItem.ToAsyncSelectInput(DebouncedQuery, LookupItem, placeholder: "Type to search...")
            | Text.Small($"Selected: {selectedItem.Value ?? "None"}")
            | Text.Small($"Total queries made: {queryCount.Value}");
    }
}
```

<Callout Type="tip">
AsyncSelectInput automatically handles loading states and provides a smooth user experience. The query function is called as the user types, and the lookup function is called when displaying the selected value.
</Callout>

## API Reference

<WidgetDocs Type="Ivy.AsyncSelectInput" ExtensionTypes="Ivy.AsyncSelectInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/AsyncSelectInput.cs"/>

## Examples

### Real-world Search Scenario

A comprehensive example showing AsyncSelectInput for user search functionality:

```csharp demo-tabs
public class UserSearchDemo : ViewBase
{
    public record User
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = "";
        public string Email { get; init; } = "";
        public string Department { get; init; } = "";
        public bool IsActive { get; init; }
    }

    public override object? Build()
    {
        var selectedUser = this.UseState<Guid?>(default(Guid));

        // Category query and lookup
        async Task<Option<Guid>[]> QueryUsers(string query)
        {
            await Task.Delay(200); // Simulate API delay
            
            // Simulate user database
            var users = new[]
            {
                new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true }
            };
            
            if (string.IsNullOrEmpty(query))
                return users.Take(5).Select(u => new Option<Guid>($"{u.Name} ({u.Department})", u.Id)).ToArray();
            
            var queryLower = query.ToLowerInvariant();
            return users
                .Where(u => u.IsActive && 
                           (u.Name.ToLowerInvariant().Contains(queryLower) || 
                            u.Email.ToLowerInvariant().Contains(queryLower) ||
                            u.Department.ToLowerInvariant().Contains(queryLower)))
                .Take(10)
                .Select(u => new Option<Guid>($"{u.Name} ({u.Department})", u.Id))
                .ToArray();
        }

        async Task<Option<Guid>?> LookupUser(Guid id)
        {
            await Task.Delay(100);
            // Simulate database lookup
            var users = new[]
            {
                new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true }
            };
            
            var user = users.FirstOrDefault(u => u.Id == id);
            return user != null ? new Option<Guid>($"{user.Name} ({user.Department})", user.Id) : null;
        }

        return Layout.Vertical()
            | Text.H2("User Search")
            | selectedUser.ToAsyncSelectInput(QueryUsers, LookupUser, placeholder: "Search users by name, email, or department...")
            | Text.Small($"Selected User ID: {selectedUser.Value?.ToString() ?? "None"}");
    }
}
```

<Callout Type="tip">
Use AsyncSelectInput for foreign key relationships, large datasets, and when you need to provide search functionality. It's perfect for scenarios where the full list of options would be too large to load upfront.
</Callout>
