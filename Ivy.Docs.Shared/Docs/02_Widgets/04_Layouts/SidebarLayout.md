# SidebarLayout

<Ingress Text="SidebarLayout provides a flexible sidebar navigation layout with a main content area and collapsible sidebar. It supports header and footer sections, responsive behavior, and can be configured as the main application sidebar with toggle functionality." />

## Basic Usage

The `SidebarLayout` creates a layout with a sidebar and main content area. The sidebar can optionally include header and footer sections:

```csharp demo-tabs
public class BasicSidebarExample : ViewBase
{
    public override object? Build()
    {
        return new SidebarLayout(
            mainContent: new Card(
                "This is the main content area. It takes up the remaining space after the sidebar."
            ).Title("Main Content"),
            sidebarContent: new Card(
                "This is the sidebar content where you can put navigation, menus, or other controls."
            ).Title("Sidebar")
        );
    }
}
```

### With Header and Footer

You can add optional header and footer sections to the sidebar:

```csharp demo-tabs
public class SidebarWithHeaderFooterExample : ViewBase
{
    public override object? Build()
    {
        return new SidebarLayout(
            mainContent: new Card(
                Layout.Vertical().Gap(2)
                    | Text.H2("Welcome to the App")
                    | Text.P("This is the main content area with a sidebar navigation.")
                    | new Button("Action Button").Variant(ButtonVariant.Primary)
            ).Title("Dashboard"),
            sidebarContent: Layout.Vertical().Gap(2)
                | new Button("Home").Variant(ButtonVariant.Ghost)
                | new Button("Profile").Variant(ButtonVariant.Ghost)
                | new Button("Settings").Variant(ButtonVariant.Ghost)
                | new Button("Help").Variant(ButtonVariant.Ghost),
            sidebarHeader: Layout.Vertical().Gap(1)
                | Text.H3("Navigation")
                | new Separator(),
            sidebarFooter: Layout.Vertical().Gap(1)
                | new Separator()
                | Text.Small("Version 1.0.0").Color(Colors.Gray)
        );
    }
}
```

## SidebarMenu Integration

The `SidebarMenu` widget is designed to work seamlessly with `SidebarLayout` for navigation:

```csharp demo-tabs
public class SidebarMenuExample : ViewBase
{
    public override object? Build()
    {
        var selectedPage = UseState("home");
        var client = UseService<IClientProvider>();
        
        MenuItem[] menuItems = new[]
        {
            MenuItem.Default("Dashboard").Icon(Icons.House).Tag("home"),
            MenuItem.Default("Analytics").Icon(Icons.ChartArea).Tag("analytics"),
            MenuItem.Default("Settings").Icon(Icons.Settings).Children(
                MenuItem.Default("Profile").Icon(Icons.User).Tag("profile"),
                MenuItem.Default("Account").Icon(Icons.UserCog).Tag("account"),
                MenuItem.Default("Preferences").Icon(Icons.Settings).Tag("preferences")
            ),
            MenuItem.Default("Help").Icon(Icons.CircleHelp).Tag("help")
        };

        var sidebarMenu = new SidebarMenu(
            onSelect: evt => {
                selectedPage.Value = evt.Value.ToString()!;
                client.Toast($"Navigated to {evt.Value}");
            },
            items: menuItems
        );

        object RenderContent()
        {
            return selectedPage.Value switch
            {
                "home" => new Card("Welcome to the Dashboard!").Title("Dashboard"),
                "analytics" => new Card("Analytics and reports go here.").Title("Analytics"),
                "profile" => new Card("User profile settings.").Title("Profile"),
                "account" => new Card("Account management.").Title("Account"),
                "preferences" => new Card("User preferences.").Title("Preferences"),
                "help" => new Card("Help and documentation.").Title("Help & Support"),
                _ => new Card("Page not found.").Title("404")
            };
        }

        return new SidebarLayout(
            mainContent: RenderContent(),
            sidebarContent: sidebarMenu,
            sidebarHeader: Layout.Vertical().Gap(2)
                | Text.H3("My App")
                | new TextInput(placeholder: "Search...", variant: TextInputs.Search),
            sidebarFooter: Layout.Horizontal().Gap(2)
                | new Avatar("JD").Size(32)
                | Layout.Vertical()
                    | Text.Small("John Doe").NoWrap()
                    | Text.Small("john@example.com").Color(Colors.Gray)
        );
    }
}
```

## Main App Sidebar

When used as the main application sidebar, the layout includes a toggle button and responsive behavior:

```csharp demo-tabs
public class MainAppSidebarExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        
        return new SidebarLayout(
            mainContent: Layout.Vertical().Gap(4)
                | new Card(
                    Layout.Vertical().Gap(2)
                        | Text.H1("Main Application")
                        | Text.P("This sidebar is configured as the main app sidebar with toggle functionality.")
                        | new Button("Test Action").HandleClick(_ => client.Toast("Action performed!"))
                ).Title("Welcome")
                | new Card(
                    "Additional content can be placed here. The sidebar will automatically collapse on smaller screens."
                ).Title("Content Area"),
            sidebarContent: Layout.Vertical().Gap(1)
                | new Button("Dashboard").Variant(ButtonVariant.Ghost).HandleClick(_ => client.Toast("Dashboard"))
                | new Button("Projects").Variant(ButtonVariant.Ghost).HandleClick(_ => client.Toast("Projects"))
                | new Button("Team").Variant(ButtonVariant.Ghost).HandleClick(_ => client.Toast("Team"))
                | new Button("Calendar").Variant(ButtonVariant.Ghost).HandleClick(_ => client.Toast("Calendar")),
            sidebarHeader: Layout.Vertical().Gap(2)
                | Text.H3("Workspace")
                | new TextInput(placeholder: "Search...", variant: TextInputs.Search)
        ).MainAppSidebar(); // Enable main app sidebar behavior
    }
}
```

## Custom Padding

You can control the padding of the main content area:

```csharp demo-tabs
public class CustomPaddingExample : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Text.H2("Padding Examples")
            | Layout.Horizontal().Gap(2)
                | new Button("No Padding").WithSheet(
                    () => new SidebarLayout(
                        mainContent: new Card("Content with no padding").Title("No Padding"),
                        sidebarContent: "Sidebar"
                    ).Padding(0),
                    title: "No Padding Example"
                )
                | new Button("Default Padding").WithSheet(
                    () => new SidebarLayout(
                        mainContent: new Card("Content with default padding").Title("Default Padding"),
                        sidebarContent: "Sidebar"
                    ),
                    title: "Default Padding Example"
                )
                | new Button("Large Padding").WithSheet(
                    () => new SidebarLayout(
                        mainContent: new Card("Content with large padding").Title("Large Padding"),
                        sidebarContent: "Sidebar"
                    ).Padding(6),
                    title: "Large Padding Example"
                );
    }
}
```

### Responsive Sidebar with Search

This example demonstrates a responsive sidebar with search functionality:

```csharp demo-tabs
public class ResponsiveSidebarExample : ViewBase
{
    public override object? Build()
    {
        var searchQuery = UseState("");
        var client = UseService<IClientProvider>();
        
        MenuItem[] allItems = new[]
        {
            MenuItem.Default("Dashboard").Icon(Icons.House).Tag("dashboard"),
            MenuItem.Default("Analytics").Icon(Icons.ChartArea).Tag("analytics"),
            MenuItem.Default("Users").Icon(Icons.Users).Tag("users"),
            MenuItem.Default("Products").Icon(Icons.Package).Tag("products"),
            MenuItem.Default("Orders").Icon(Icons.ShoppingCart).Tag("orders"),
            MenuItem.Default("Inventory").Icon(Icons.Warehouse).Tag("inventory"),
            MenuItem.Default("Reports").Icon(Icons.FileText).Tag("reports"),
            MenuItem.Default("Settings").Icon(Icons.Settings).Tag("settings"),
            MenuItem.Default("Help").Icon(Icons.CircleHelp).Tag("help")
        };

        var filteredItems = this.UseMemo(() => {
            if (string.IsNullOrWhiteSpace(searchQuery.Value))
                return allItems;
            
            return allItems.Where(item => 
                item.Label?.Contains(searchQuery.Value, StringComparison.OrdinalIgnoreCase) == true
            ).ToArray();
        }, [searchQuery]);

        var menu = new SidebarMenu(
            onSelect: evt => client.Toast($"Selected: {evt.Value}"),
            items: filteredItems
        ) { SearchActive = !string.IsNullOrWhiteSpace(searchQuery.Value) };

        return new SidebarLayout(
            mainContent: Layout.Vertical().Gap(4)
                | new Card(
                    Layout.Vertical().Gap(2)
                        | Text.H1("Responsive Dashboard")
                        | Text.P("This sidebar adapts to screen size and includes search functionality.")
                        | Text.Small($"Search query: '{searchQuery.Value}'").Color(Colors.Gray)
                ).Title("Main Content")
                | new Card(
                    "Try searching for items in the sidebar or resize your browser window to see the responsive behavior."
                ).Title("Instructions"),
            sidebarContent: menu,
            sidebarHeader: Layout.Vertical().Gap(2)
                | Text.H3("Navigation")
                | searchQuery.ToSearchInput()
                    .Placeholder("Search navigation..."),
            sidebarFooter: Layout.Vertical().Gap(1)
                | new Separator()
                | Text.Small($"Found {filteredItems.Length} items").Color(Colors.Gray)
        ).MainAppSidebar();
    }
}
```

<WidgetDocs Type="Ivy.SidebarLayout" ExtensionTypes="Ivy.SidebarLayoutExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Layouts/SidebarLayout.cs"/>

## Advanced Examples

### SidebarMenu Widget

The `SidebarMenu` widget is specifically designed for sidebar navigation and provides advanced features like search highlighting and keyboard navigation:

```csharp demo-tabs
public class SidebarMenuAdvancedExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        var selectedItem = UseState("");
        
        MenuItem[] menuItems = new[]
        {
            MenuItem.Default("Getting Started").Icon(Icons.Play).Children(
                MenuItem.Default("Installation").Icon(Icons.Download).Tag("install"),
                MenuItem.Default("Quick Start").Icon(Icons.Zap).Tag("quickstart"),
                MenuItem.Default("Examples").Icon(Icons.Code).Tag("examples")
            ),
            MenuItem.Default("Components").Icon(Icons.Package).Children(
                MenuItem.Default("Buttons").Icon(Icons.MousePointer).Tag("buttons"),
                MenuItem.Default("Forms").Icon(Icons.FileText).Tag("forms"),
                MenuItem.Default("Charts").Icon(Icons.ChartBar).Tag("charts"),
                MenuItem.Default("Tables").Icon(Icons.Table).Tag("tables")
            ),
            MenuItem.Default("Advanced").Icon(Icons.Cpu).Children(
                MenuItem.Default("Hooks").Icon(Icons.Link).Tag("hooks"),
                MenuItem.Default("State Management").Icon(Icons.Database).Tag("state"),
                MenuItem.Default("Performance").Icon(Icons.Gauge).Tag("performance")
            )
        };

        var menu = new SidebarMenu(
            onSelect: evt => {
                selectedItem.Value = evt.Value.ToString();
                client.Toast($"Selected: {evt.Value}");
            },
            items: menuItems
        ) {
            OnCtrlRightClickSelect = evt => {
                client.Toast($"Right-clicked: {evt.Value} (Ctrl+Right-click for special actions)");
            }
        };

        return new SidebarLayout(
            mainContent: new Card(
                Layout.Vertical().Gap(2)
                    | Text.H2("Documentation")
                    | Text.P($"Currently viewing: {(string.IsNullOrEmpty(selectedItem.Value) ? "None" : selectedItem.Value)}")
                    | Text.Small("Use Ctrl+Right-click on menu items for additional actions.")
            ).Title("Content Area"),
            sidebarContent: menu,
            sidebarHeader: Text.H3("Documentation Menu")
        );
    }
}
```

### Navigation with State Management

This example shows a more complex navigation setup with state management and dynamic content:

```csharp demo-tabs
public class AdvancedNavigationExample : ViewBase
{
    public override object? Build()
    {
        var currentSection = UseState("overview");
        var notifications = UseState(3);
        var client = UseService<IClientProvider>();
        
        MenuItem[] navigationItems = new[]
        {
            MenuItem.Default("Overview").Icon(Icons.House).Tag("overview"),
            MenuItem.Default("Projects").Icon(Icons.FolderOpen).Tag("projects"),
            MenuItem.Default("Tasks").Icon(Icons.SquareCheck).Tag("tasks"),
            MenuItem.Default("Team").Icon(Icons.Users).Tag("team"),
            MenuItem.Separator(),
            MenuItem.Default("Reports").Icon(Icons.ChartBar).Children(
                MenuItem.Default("Analytics").Icon(Icons.TrendingUp).Tag("analytics"),
                MenuItem.Default("Performance").Icon(Icons.Gauge).Tag("performance"),
                MenuItem.Default("Usage").Icon(Icons.Activity).Tag("usage")
            ),
            MenuItem.Default("Settings").Icon(Icons.Settings).Children(
                MenuItem.Default("General").Icon(Icons.Settings).Tag("general"),
                MenuItem.Default("Security").Icon(Icons.Shield).Tag("security"),
                MenuItem.Default("Integrations").Icon(Icons.Plug).Tag("integrations")
            )
        };

        var menu = new SidebarMenu(
            onSelect: evt => {
                currentSection.Value = evt.Value.ToString()!;
                client.Toast($"Switched to {evt.Value}");
            },
            items: navigationItems
        );

        object RenderMainContent()
        {
            return currentSection.Value switch
            {
                "overview" => Layout.Vertical().Gap(4)
                    | new Card(
                        Layout.Grid().Columns(3)
                            | new Card("42").Title("Total Projects")
                            | new Card("128").Title("Active Tasks") 
                            | new Card("8").Title("Team Members")
                    ).Title("Dashboard Overview"),
                    
                "projects" => new Card(
                    Layout.Vertical().Gap(2)
                        | Text.P("Manage your projects and track progress.")
                        | new Button("New Project").Variant(ButtonVariant.Primary)
                ).Title("Projects"),
                
                "tasks" => new Card(
                    Layout.Vertical().Gap(2)
                        | Text.P("View and manage your tasks.")
                        | Layout.Horizontal().Gap(2)
                            | new Button("Add Task").Variant(ButtonVariant.Primary)
                            | new Button("Filter").Variant(ButtonVariant.Outline)
                ).Title("Task Management"),
                
                "team" => new Card(
                    Layout.Vertical().Gap(2)
                        | Text.P("Team collaboration and member management.")
                        | new Button("Invite Member").Variant(ButtonVariant.Primary)
                ).Title("Team"),
                
                "analytics" => new Card("Advanced analytics and insights.").Title("Analytics"),
                "performance" => new Card("Performance metrics and optimization.").Title("Performance"),
                "usage" => new Card("Usage statistics and trends.").Title("Usage Statistics"),
                "general" => new Card("General application settings.").Title("General Settings"),
                "security" => new Card("Security and privacy settings.").Title("Security"),
                "integrations" => new Card("Third-party integrations.").Title("Integrations"),
                
                _ => new Card("Content not found.").Title("404")
            };
        }

        return new SidebarLayout(
            mainContent: RenderMainContent(),
            sidebarContent: menu,
            sidebarHeader: Layout.Vertical().Gap(3)
                | Layout.Horizontal().Gap(2)
                    | new Avatar("AC").Size(40)
                    | Layout.Vertical()
                        | Text.Large("Acme Corp")
                        | Text.Small("Workspace").Color(Colors.Gray)
                | new TextInput(placeholder: "Search workspace...", variant: TextInputs.Search),
            sidebarFooter: Layout.Vertical().Gap(2)
                | new Separator()
                | Layout.Horizontal().Gap(2)
                    | new Avatar("JD").Size(32)
                    | Layout.Vertical()
                        | Text.Small("John Doe").NoWrap()
                        | Text.Small("Administrator").Color(Colors.Gray)
                    | Layout.Horizontal().Gap(1)
                        | new Badge($"{notifications.Value}").Variant(BadgeVariant.Destructive)
                        | new Button(icon: Icons.Bell).Variant(ButtonVariant.Ghost)
        ).MainAppSidebar();
    }
}
```