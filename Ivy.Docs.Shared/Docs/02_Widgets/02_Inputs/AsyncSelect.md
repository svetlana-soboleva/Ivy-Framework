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
    // Use consistent GUIDs for demo purposes
    private static readonly Guid ElectronicsId = Guid.NewGuid();
    private static readonly Guid ClothingId = Guid.NewGuid();
    private static readonly Guid BooksId = Guid.NewGuid();
    private static readonly Guid HomeGardenId = Guid.NewGuid();
    private static readonly Guid SportsId = Guid.NewGuid();

    public override object? Build()
    {
        var guidState = this.UseState<Guid?>(default(Guid?));
        var selectedCategoryName = this.UseState<string>("No category selected");

        async Task<Option<Guid?>[]> QueryCategories(string query)
        {
            
            // Simulate database results with consistent IDs
            var categories = new[]
            {
                new { Id = ElectronicsId, Name = "Electronics" },
                new { Id = ClothingId, Name = "Clothing" },
                new { Id = BooksId, Name = "Books" },
                new { Id = HomeGardenId, Name = "Home & Garden" },
                new { Id = SportsId, Name = "Sports" }
            };
            
            return categories
                .Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(e => new Option<Guid?>(e.Name, e.Id))
                .ToArray();
        }

        async Task<Option<Guid?>?> LookupCategory(Guid? id)
        {
            if (id == null) return null;
            
            // Simulate database lookup with consistent IDs
            var categories = new[]
            {
                new { Id = ElectronicsId, Name = "Electronics" },
                new { Id = ClothingId, Name = "Clothing" },
                new { Id = BooksId, Name = "Books" },
                new { Id = HomeGardenId, Name = "Home & Garden" },
                new { Id = SportsId, Name = "Sports" }
            };
            
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                selectedCategoryName.Set(category.Name);
            }
            return category != null ? new Option<Guid?>(category.Name, category.Id) : null;
        }

        return Layout.Vertical()
            | Text.Label("Select a category:")
            | guidState.ToAsyncSelectInput(QueryCategories, LookupCategory, placeholder: "Select Category")
            | Text.Small($"Selected: {selectedCategoryName.Value}");
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
            
            var countries = new[] { "Germany", "France", "Japan", "China", "USA", "Canada", "Australia", "Brazil" };
            return countries
                .Where(c => c.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(c => new Option<string>(c))
                .ToArray();
        }

        async Task<Option<string>?> LookupCountry(string country)
        {
            if (string.IsNullOrEmpty(country)) return null;
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
        var selectedLanguage = this.UseState(ProgrammingLanguage.CSharp);

        async Task<Option<ProgrammingLanguage>[]> QueryLanguages(string query)
        {
            
            // Create a static array of languages to avoid runtime issues
            var languages = new[] 
            { 
                ProgrammingLanguage.CSharp, 
                ProgrammingLanguage.Java, 
                ProgrammingLanguage.Python, 
                ProgrammingLanguage.JavaScript, 
                ProgrammingLanguage.Go, 
                ProgrammingLanguage.Rust, 
                ProgrammingLanguage.FSharp, 
                ProgrammingLanguage.Kotlin, 
                ProgrammingLanguage.Swift, 
                ProgrammingLanguage.TypeScript 
            };
            
            if (string.IsNullOrEmpty(query))
                return languages.Select(l => new Option<ProgrammingLanguage>(l.ToString(), l)).ToArray();
            
            return languages
                .Where(l => l.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(l => new Option<ProgrammingLanguage>(l.ToString(), l))
                .ToArray();
        }

        async Task<Option<ProgrammingLanguage>?> LookupLanguage(ProgrammingLanguage language)
        {
            return new Option<ProgrammingLanguage>(language.ToString(), language);
        }

        return Layout.Vertical()
            | Text.Label("Select a programming language:")
            | selectedLanguage.ToAsyncSelectInput(QueryLanguages, LookupLanguage, placeholder: "Search languages...")
            | Text.Small($"Selected: {selectedLanguage.Value.ToString()}");
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

    // Use consistent GUIDs for demo purposes
    private static readonly Guid JohnId = Guid.NewGuid();
    private static readonly Guid JaneId = Guid.NewGuid();
    private static readonly Guid BobId = Guid.NewGuid();
    private static readonly Guid AliceId = Guid.NewGuid();
    private static readonly Guid CharlieId = Guid.NewGuid();

    // Create a static lookup dictionary for quick access
    private static readonly Dictionary<Guid, User> UserLookup = new()
    {
        { JohnId, new User { Id = JohnId, Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true } },
        { JaneId, new User { Id = JaneId, Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true } },
        { BobId, new User { Id = BobId, Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = false } },
        { AliceId, new User { Id = AliceId, Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true } },
        { CharlieId, new User { Id = CharlieId, Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true } }
    };

    public override object? Build()
    {
        var selectedUser = this.UseState<Guid>(default(Guid));
        var selectedUserInfo = this.UseState<string>("No user selected");

        // Update display when selection changes
        this.UseEffect(() =>
        {
            if (selectedUser.Value != default(Guid) && UserLookup.ContainsKey(selectedUser.Value))
            {
                var user = UserLookup[selectedUser.Value];
                selectedUserInfo.Set($"{user.Name} - {user.Email} ({user.Department})");
            }
            else
            {
                selectedUserInfo.Set("No user selected");
            }
        }, [selectedUser]);

        async Task<Option<Guid>[]> QueryUsers(string query)
        {
            
            // Simulate user database with consistent IDs
            var users = new[]
            {
                new User { Id = JohnId, Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = JaneId, Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true },
                new User { Id = BobId, Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = false },
                new User { Id = AliceId, Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true },
                new User { Id = CharlieId, Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true }
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
            // In real app, fetch from database
            var users = new[]
            {
                new User { Id = JohnId, Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = JaneId, Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true },
                new User { Id = BobId, Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = false },
                new User { Id = AliceId, Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true },
                new User { Id = CharlieId, Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true }
            };
            
            var user = users.FirstOrDefault(u => u.Id == id);
            return user != null ? new Option<Guid>($"{user.Name} ({user.Department})", user.Id) : null;
        }

        // Create a custom AsyncSelectInput that handles state updates
        var customAsyncSelect = new AsyncSelectInputView<Guid>(
            selectedUser.Value,
            e => 
            {
                // This is called when a selection is made
                selectedUser.Set(e.Value);
                Console.WriteLine($"Selection changed to: {e.Value}");
            },
            QueryUsers,
            LookupUser,
            placeholder: "Search by name, email, or department..."
        );

        return Layout.Vertical()
            | Text.Label("Search and select a user:")
            | customAsyncSelect
            | Text.Small($"Selected: {selectedUserInfo.Value}")
            | Text.Small($"Debug - Raw value: {selectedUser.Value.ToString()}");
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
            var options = new[] { "Option 1", "Option 2", "Option 3" };
            return options
                .Where(opt => opt.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(opt => new Option<string>(opt))
                .ToArray();
        }

        async Task<Option<string>?> LookupOption(string option)
        {
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
            
            var items = new[] { "Apple", "Banana", "Cherry", "Date", "Elderberry", "Fig", "Grape" };
            return items
                .Where(item => item.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(item => new Option<string>(item))
                .ToArray();
        }

        async Task<Option<string>?> LookupItem(string item)
        {
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

    // Use consistent GUIDs for demo purposes
    private static readonly Guid JohnId = Guid.NewGuid();
    private static readonly Guid JaneId = Guid.NewGuid();
    private static readonly Guid BobId = Guid.NewGuid();
    private static readonly Guid AliceId = Guid.NewGuid();
    private static readonly Guid CharlieId = Guid.NewGuid();

    // Create a static lookup dictionary for quick access
    private static readonly Dictionary<Guid, User> UserLookup = new()
    {
        { JohnId, new User { Id = JohnId, Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true } },
        { JaneId, new User { Id = JaneId, Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true } },
        { BobId, new User { Id = BobId, Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = true } },
        { AliceId, new User { Id = AliceId, Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true } },
        { CharlieId, new User { Id = CharlieId, Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true } }
    };

    public override object? Build()
    {
        var selectedUser = this.UseState<Guid>(default(Guid));
        var selectedUserDetails = this.UseState<string>("No user selected");

        // Create a static lookup dictionary for quick access
        var userLookup = new Dictionary<Guid, User>
        {
            { JohnId, new User { Id = JohnId, Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true } },
            { JaneId, new User { Id = JaneId, Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true } },
            { BobId, new User { Id = BobId, Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = true } },
            { AliceId, new User { Id = AliceId, Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true } },
            { CharlieId, new User { Id = CharlieId, Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true } }
        };

        // Update display when selection changes
        this.UseEffect(() =>
        {
            if (selectedUser.Value != default(Guid) && userLookup.ContainsKey(selectedUser.Value))
            {
                var user = userLookup[selectedUser.Value];
                selectedUserDetails.Set($"{user.Name} - {user.Email} - {user.Department}");
            }
            else
            {
                selectedUserDetails.Set("No user selected");
            }
        }, [selectedUser]);

        // Category query and lookup
        async Task<Option<Guid>[]> QueryUsers(string query)
        {
            
            // Simulate user database with consistent IDs
            var users = new[]
            {
                new User { Id = JohnId, Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = JaneId, Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true },
                new User { Id = BobId, Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = true },
                new User { Id = AliceId, Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true },
                new User { Id = CharlieId, Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true }
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
            var users = new[]
            {
                new User { Id = JohnId, Name = "John Doe", Email = "john@example.com", Department = "Engineering", IsActive = true },
                new User { Id = JaneId, Name = "Jane Smith", Email = "jane@example.com", Department = "Design", IsActive = true },
                new User { Id = BobId, Name = "Bob Johnson", Email = "bob@example.com", Department = "Marketing", IsActive = true },
                new User { Id = AliceId, Name = "Alice Brown", Email = "alice@example.com", Department = "Engineering", IsActive = true },
                new User { Id = CharlieId, Name = "Charlie Wilson", Email = "charlie@example.com", Department = "Sales", IsActive = true }
            };
            
            var user = users.FirstOrDefault(u => u.Id == id);
            return user != null ? new Option<Guid>($"{user.Name} ({user.Department})", user.Id) : null;
        }

        // Create a custom AsyncSelectInput that handles state updates
        var customAsyncSelect = new AsyncSelectInputView<Guid>(
            selectedUser.Value,
            e => 
            {
                // This is called when a selection is made
                selectedUser.Set(e.Value);
                Console.WriteLine($"Selection changed to: {e.Value}");
            },
            QueryUsers,
            LookupUser,
            placeholder: "Search users by name, email, or department..."
        );

        return Layout.Vertical()
            | Text.H2("User Search")
            | customAsyncSelect
            | Text.Small($"Selected: {selectedUserDetails.Value}")
            | Text.Small($"Debug - Raw value: {selectedUser.Value.ToString()}");
    }
}
```

<Callout Type="tip">
Use AsyncSelectInput for foreign key relationships, large datasets, and when you need to provide search functionality. It's perfect for scenarios where the full list of options would be too large to load upfront.
</Callout>
