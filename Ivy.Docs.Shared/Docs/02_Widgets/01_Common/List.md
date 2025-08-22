# List

<Ingress>
Display collections of items in organized, styled lists with customizable formatting and interactive elements.
</Ingress>

The `List` widget is a container designed to render collections of items in a vertical layout. It works seamlessly with `ListItem` components to create interactive, searchable, and customizable lists that are perfect for navigation menus, data displays, and user interfaces.

## Basic Usage

The simplest way to create a list is by passing items directly to the constructor:

```csharp demo-tabs
public class BasicListDemo : ViewBase
{
    public override object? Build()
    {
        var items = new[]
        {
            new ListItem("Apple"),
            new ListItem("Banana"),
            new ListItem("Cherry"),
            new ListItem("Date")
        };
        
        return new List(items);
    }
}
```

## ListItem Properties

### Title and Subtitle

Each `ListItem` can have a title and subtitle for hierarchical information display:

```csharp demo-tabs
public class TitleSubtitleDemo : ViewBase
{
    public override object? Build()
    {
        var users = new[]
        {
            new ListItem("John Doe", subtitle: "Software Engineer"),
            new ListItem("Jane Smith", subtitle: "Product Manager"),
            new ListItem("Bob Johnson", subtitle: "Designer"),
            new ListItem("Alice Brown", subtitle: "Developer")
        };
        
        return new List(users);
    }
}
```

### Icons

Add visual indicators with icons to make lists more intuitive:

```csharp demo-tabs
public class IconListDemo : ViewBase
{
    public override object? Build()
    {
        var menuItems = new[]
        {
            new ListItem("Dashboard", icon: Icons.House, subtitle: "Main overview"),
            new ListItem("Users", icon: Icons.Users, subtitle: "Manage users"),
            new ListItem("Settings", icon: Icons.Settings, subtitle: "Configuration"),
            new ListItem("Reports", icon: Icons.ChartBar, subtitle: "Analytics")
        };
        
        return new List(menuItems);
    }
}
```

### Badges and Tags

Use badges to show additional information like counts, status, or labels:

```csharp demo-tabs
public class BadgeListDemo : ViewBase
{
    public override object? Build()
    {
        var notifications = new[]
        {
            new ListItem("New Message", subtitle: "From John Doe", badge: "3"),
            new ListItem("System Update", subtitle: "Available now", badge: "!"),
            new ListItem("Task Reminder", subtitle: "Due tomorrow", badge: "5")
        };
        
        return new List(notifications);
    }
}
```

## Interactive Lists

### Clickable Items

Make list items interactive by adding click handlers:

```csharp demo-tabs
public class InteractiveListDemo : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        
        var onItemClick = new Action<Event<ListItem>>(e =>
        {
            var item = e.Sender;
            client.Toast($"Clicked: {item.Title}", "Item Selected");
        });
        
        var items = new[]
        {
            new ListItem("Click me!", onClick: onItemClick, icon: Icons.MousePointer),
            new ListItem("Me too!", onClick: onItemClick, icon: Icons.MousePointer)
        };

        return new List(items);
    }
}
```

### Navigation with Blades

Create navigation patterns using the blade system:

```csharp demo-tabs
public class NavigationListDemo : ViewBase
{
    public override object? Build()
    {
        return this.UseBlades(() => new NavigationListRootView(), "Navigation");
    }
}

public class NavigationListRootView : ViewBase
{
    public override object? Build()
    {
        var blades = this.UseContext<IBladeController>();
        
        var onItemClick = new Action<Event<ListItem>>(e =>
        {
            var item = e.Sender;
            var detailView = new ListDetailView(item.Title!);
            blades.Push(this, detailView, item.Title);
        });
        
        var menuItems = new[]
        {
            new ListItem("Products", onClick: onItemClick, icon: Icons.Package, subtitle: "Manage inventory"),
            new ListItem("Customers", onClick: onItemClick, icon: Icons.Users, subtitle: "Customer database"),
            new ListItem("Orders", onClick: onItemClick, icon: Icons.ShoppingCart, subtitle: "Order management"),
            new ListItem("Analytics", onClick: onItemClick, icon: Icons.ChartBar, subtitle: "Reports & insights")
        };
        
        return new List(menuItems);
    }
}

public class ListDetailView(string title) : ViewBase
{
    public override object? Build()
    {
        return new Card($"Details for {title}")
            | Text.Block($"This is the detail view for {title}")
            | Text.Block("Here you can see detailed information about the selected item.")
            | new Button("Go Back", onClick: _ => { });
    }
}
```

## Data-Driven Lists

### Dynamic Content

Create lists from dynamic data sources:

```csharp demo-tabs
public class DynamicListDemo : ViewBase
{
    public override object? Build()
    {
        var items = UseState(new[] { "Item 1", "Item 2", "Item 3" });
        
        var addItem = new Action<Event<Button>>(e =>
        {
            var newItems = items.Value.Append($"Item {items.Value.Length + 1}").ToArray();
            items.Set(newItems);
        });
        
        var removeItem = new Action<Event<Button>>(e =>
        {
            if (items.Value.Length > 0)
            {
                var newItems = items.Value.Take(items.Value.Length - 1).ToArray();
                items.Set(newItems);
            }
        });
        
        return Layout.Vertical().Gap(2)
            | (Layout.Horizontal().Gap(1)
                | new Button("Add Item", addItem).Variant(ButtonVariant.Secondary)
                | new Button("Remove Item", removeItem).Variant(ButtonVariant.Destructive))
            | new List(items.Value.Select(item => new ListItem(item)));
    }
}
```

### Search and Filter

Implement search functionality with filtered lists:

```csharp demo-tabs
public class SearchableListDemo : ViewBase
{
    public override object? Build()
    {
        var allItems = new[] { "Apple", "Banana", "Cherry", "Date", "Elderberry", "Fig", "Grape" };
        var searchTerm = UseState("");
        var filteredItems = UseState(allItems);
        
        UseEffect(() =>
        {
            var filtered = allItems.Where(item => 
                item.Contains(searchTerm.Value, StringComparison.OrdinalIgnoreCase)).ToArray();
            filteredItems.Set(filtered);
        }, [searchTerm]);
        
        var listItems = filteredItems.Value.Select(item => new ListItem(item));
        
        return Layout.Vertical().Gap(2)
            | searchTerm.ToSearchInput().Placeholder("Search fruits...")
            | new List(listItems);
    }
}
```

## Advanced Patterns

### Custom Item Rendering

Use the `items` parameter to create complex list items with custom layouts:

```csharp demo-tabs
public class CustomItemDemo : ViewBase
{
    public override object? Build()
    {
        var products = new[]
        {
            new { Name = "Laptop", Price = 999.99m, Stock = 15 },
            new { Name = "Mouse", Price = 29.99m, Stock = 50 },
            new { Name = "Keyboard", Price = 89.99m, Stock = 25 }
        };
        
        var listItems = products.Select(product => new ListItem(
            title: product.Name,
            subtitle: $"${product.Price} - {product.Stock} in stock",
            items: new object[]
            {
                Layout.Horizontal().Gap(2)
                    | Text.Block($"${product.Price}")
                    | new Badge(product.Stock.ToString()).Variant(BadgeVariant.Secondary)
            }
        ));
        
        return new List(listItems);
    }
}
```

### Integration with Other Widgets

Lists work seamlessly with other Ivy widgets:

```csharp demo-tabs
public class IntegratedListDemo : ViewBase
{
    public override object? Build()
    {
        var selectedItems = UseState(new HashSet<string>());
        
        var onItemClick = new Action<Event<ListItem>>(e =>
        {
            var item = e.Sender;
            var title = item.Title!;
            
            if (selectedItems.Value.Contains(title))
            {
                selectedItems.Set(selectedItems.Value.Where(x => x != title).ToHashSet());
            }
            else
            {
                selectedItems.Set(selectedItems.Value.Append(title).ToHashSet());
            }
        });
        
        var items = new[] { "Option 1", "Option 2", "Option 3", "Option 4" };
        
        var listItems = items.Select(item => new ListItem(
            title: item,
            onClick: onItemClick,
            icon: selectedItems.Value.Contains(item) ? Icons.CircleCheck : Icons.Circle,
            badge: selectedItems.Value.Contains(item) ? "Selected" : null
        ));
        
        return Layout.Vertical().Gap(2)
            | new Card(
                new List(listItems)
            ).Title("Multi-Select List")
            | Text.Block($"Selected: {string.Join(", ", selectedItems.Value)}");
    }
}
```

### Empty State Handling

Provide meaningful content when lists are empty:

```csharp demo-tabs
public class EmptyStateDemo : ViewBase
{
    public override object? Build()
    {
        var items = UseState(new string[0]);
        
        var addSampleItems = new Action<Event<Button>>(e =>
        {
            items.Set(new[] { "Sample Item 1", "Sample Item 2", "Sample Item 3" });
        });
        
        var clearItems = new Action<Event<Button>>(e =>
        {
            items.Set(new string[0]);
        });
        
        var listContent = items.Value.Length > 0 
            ? (object)new List(items.Value.Select(item => new ListItem(item)))
            : new Card("No items found").Width(Size.Full());
        
        return Layout.Vertical().Gap(2)
            | (Layout.Horizontal().Gap(1)
                | new Button("Add Items", addSampleItems).Variant(ButtonVariant.Secondary)
                | new Button("Clear All", clearItems).Variant(ButtonVariant.Destructive))
            | listContent;
    }
}
```

## Performance Considerations

### Virtual Scrolling

For large lists, consider using the built-in virtual scrolling capabilities:

```csharp demo-tabs
public class LargeListDemo : ViewBase
{
    public override object? Build()
    {
        var items = this.UseMemo(() => 
            Enumerable.Range(1, 10).Select(i => $"Item {i:D4}").ToArray(), 
            []
        );
        
        var listItems = items.Select(item => new ListItem(item));
        
        return new List(listItems);
    }
}
```

### Memoization

Use memoization to prevent unnecessary re-renders of list items:

```csharp demo-tabs
public class MemoizedListDemo : ViewBase
{
    public override object? Build()
    {
        var items = UseState(new[] { 1, 2 });
        
        var addItem = new Action<Event<Button>>(e =>
        {
            var newItems = items.Value.Append(items.Value.Length + 1).ToArray();
            items.Set(newItems);
        });
        
        var listItems = items.Value.Select(item => new MemoizedListItem(item));
        
        return Layout.Vertical().Gap(2)
            | new Button("Add Item", addItem)
            | new List(listItems);
    }
}

public class MemoizedListItem(int value) : ViewBase, IMemoized
{
    public object[] GetMemoValues() => [value];
    
    public override object? Build()
    {
        return new ListItem($"Item {value}", subtitle: $"Rendered at {DateTime.Now:HH:mm:ss}");
    }
}
```

<WidgetDocs Type="Ivy.List" ExtensionTypes="Ivy.WidgetBaseExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Lists/List.cs"/>
