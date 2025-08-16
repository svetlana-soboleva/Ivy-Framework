# HeaderLayout

<Ingress>
HeaderLayout provides a fixed header area above scrollable content, perfect for toolbars, navigation, and persistent controls that should remain visible while users scroll through content.
</Ingress>

The `HeaderLayout` widget creates a layout with a fixed header section at the top and a scrollable content area below. This pattern is commonly used for applications that need persistent navigation, toolbars, or status information while allowing the main content to scroll independently.

## Basic Usage

The simplest HeaderLayout takes a header and content as parameters:

```csharp
new HeaderLayout(
    header: Text.H2("Page Title"),
    content: Text.P("This content will scroll underneath the fixed header.")
)
```

```csharp demo
new HeaderLayout(
    header: new Card("Fixed Header Content")
        .Title("Header Area"),
    content: Layout.Vertical().Gap(2)
        | Text.P("This is scrollable content area.")
        | Text.P("The header above will remain fixed while this content scrolls.")
        | Text.P("You can add as much content as needed here.")
)
```

## Common Use Cases

### Toolbar with Actions

HeaderLayout is perfect for creating toolbars with action buttons that remain accessible:

```csharp demo-tabs
public class ToolbarExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        var searchText = UseState("");
        
        var toolbar = Layout.Horizontal().Gap(2).Padding(2)
            | searchText.ToTextInput()
                .Placeholder("Search items...")
                .Variant(TextInputs.Search)
            | new Button("Add Item")
                .Icon(Icons.Plus)
                .Variant(ButtonVariant.Primary)
                .HandleClick(_ => client.Toast("Add item clicked"))
            | new Button("Filter")
                .Icon(Icons.Filter)
                .Variant(ButtonVariant.Outline)
                .HandleClick(_ => client.Toast("Filter clicked"))
            | new Button("Export")
                .Icon(Icons.Download)
                .Variant(ButtonVariant.Ghost)
                .HandleClick(_ => client.Toast("Export clicked"));

        var content = Layout.Vertical().Gap(3)
            | new Card("Item 1 - This is some sample content")
            | new Card("Item 2 - More content that will scroll")
            | new Card("Item 3 - The toolbar above stays fixed")
            | new Card("Item 4 - While this content area scrolls")
            | new Card("Item 5 - Perfect for data management interfaces")
            | new Card("Item 6 - Search, filter, and action buttons")
            | new Card("Item 7 - Remain easily accessible")
            | new Card("Item 8 - Even with lots of content");

        return new HeaderLayout(toolbar, content);
    }
}
```

### Dashboard Header

Use HeaderLayout for dashboard-style interfaces with status indicators and quick actions:

```csharp demo-tabs
public class DashboardHeaderExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        
        var dashboardHeader = Layout.Horizontal().Gap(4).Padding(3)
            | Layout.Vertical().Gap(1)
                | Text.H3("Analytics Dashboard")
                | Text.Small("Last updated: 2 minutes ago").Color(Colors.Gray)
            | new Spacer()
            | Layout.Horizontal().Gap(2)
                | new Badge("Live")
                | new Button("Refresh")
                    .Icon(Icons.RefreshCw)
                    .Variant(ButtonVariant.Outline)
                    .HandleClick(_ => client.Toast("Refreshing data..."))
                | new Button("Settings")
                    .Icon(Icons.Settings)
                    .Variant(ButtonVariant.Ghost)
                    .HandleClick(_ => client.Toast("Opening settings..."));

        var dashboardContent = Layout.Vertical().Gap(4)
            | Layout.Horizontal().Gap(4)
                | new Card(
                    Layout.Vertical().Gap(2)
                        | Text.H4("Total Users")
                        | Text.H2("12,345").Color(Colors.Primary)
                        | Text.Small("↑ 12% from last month").Color(Colors.Green)
                )
                | new Card(
                    Layout.Vertical().Gap(2)
                        | Text.H4("Revenue")
                        | Text.H2("$54,321").Color(Colors.Primary)
                        | Text.Small("↑ 8% from last month").Color(Colors.Green)
                )
                | new Card(
                    Layout.Vertical().Gap(2)
                        | Text.H4("Conversion Rate")
                        | Text.H2("3.2%").Color(Colors.Primary)
                        | Text.Small("↓ 2% from last month").Color(Colors.Red)
                )
            | new Card("Chart Area - Main dashboard content goes here")
                .Height(Size.Px(300))
            | new Card("Additional metrics and reports")
                .Height(Size.Px(200));

        return new HeaderLayout(dashboardHeader, dashboardContent);
    }
}
```

### Navigation Header

Create navigation headers for content-heavy pages:

```csharp demo-tabs
public class NavigationHeaderExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        var currentSection = UseState("introduction");
        
        var navHeader = Layout.Horizontal().Gap(1).Padding(2)
            | new Button("Introduction")
                .Variant(currentSection.Value == "introduction" ? ButtonVariant.Primary : ButtonVariant.Ghost)
                .HandleClick(_ => {
                    currentSection.Value = "introduction";
                    client.Toast("Navigated to Introduction");
                })
            | new Button("Getting Started")
                .Variant(currentSection.Value == "getting-started" ? ButtonVariant.Primary : ButtonVariant.Ghost)
                .HandleClick(_ => {
                    currentSection.Value = "getting-started";
                    client.Toast("Navigated to Getting Started");
                })
            | new Button("Advanced")
                .Variant(currentSection.Value == "advanced" ? ButtonVariant.Primary : ButtonVariant.Ghost)
                .HandleClick(_ => {
                    currentSection.Value = "advanced";
                    client.Toast("Navigated to Advanced");
                })
            | new Spacer()
            | new Button("Download PDF")
                .Icon(Icons.Download)
                .Variant(ButtonVariant.Outline)
                .HandleClick(_ => client.Toast("Downloading PDF..."));

        object GetSectionContent()
        {
            return currentSection.Value switch
            {
                "introduction" => Layout.Vertical().Gap(3)
                    | Text.H2("Introduction")
                    | Text.P("Welcome to our comprehensive guide. This section covers the basic concepts you need to understand.")
                    | Text.P("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.")
                    | Text.P("Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.")
                    | new Card("Key concepts are highlighted in cards like this one.")
                    | Text.P("Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."),
                
                "getting-started" => Layout.Vertical().Gap(3)
                    | Text.H2("Getting Started")
                    | Text.P("Follow these steps to get up and running quickly.")
                    | Text.H3("Step 1: Installation")
                    | Text.P("First, install the required dependencies...")
                    | new Card("npm install ivy-framework")
                    | Text.H3("Step 2: Configuration")
                    | Text.P("Configure your application settings...")
                    | Text.P("Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."),
                
                "advanced" => Layout.Vertical().Gap(3)
                    | Text.H2("Advanced Topics")
                    | Text.P("This section covers advanced usage patterns and optimization techniques.")
                    | Text.H3("Performance Optimization")
                    | Text.P("Learn how to optimize your application for better performance.")
                    | Text.H3("Custom Extensions")
                    | Text.P("Create custom widgets and extensions for specialized use cases.")
                    | new Card("Advanced features require careful consideration of your application's architecture."),
                
                _ => Text.P("Section not found")
            };
        }

        return new HeaderLayout(navHeader, GetSectionContent());
    }
}
```

### Form with Header Actions

HeaderLayout works well for forms with header-level actions:

```csharp demo-tabs
public class FormHeaderExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        var name = UseState("John Doe");
        var email = UseState("john@example.com");
        var bio = UseState("Software developer with 5 years of experience...");

        var formHeader = Layout.Horizontal().Gap(2).Padding(2)
            | Layout.Vertical().Gap(1)
                | Text.H3("Edit Profile")
                | Text.Small("Ready to save changes").Color(Colors.Green)
            | new Spacer()
            | new Button("Cancel")
                .Variant(ButtonVariant.Ghost)
                .HandleClick(_ => client.Toast("Changes discarded"))
            | new Button("Save Changes")
                .Variant(ButtonVariant.Primary)
                .HandleClick(_ => client.Toast("Profile saved successfully!"));

        var formContent = Layout.Vertical().Gap(4)
            | new Card(
                Layout.Vertical().Gap(3)
                    | Text.H4("Personal Information")
                    | Text.Label("Full Name")
                    | name.ToTextInput()
                    | Text.Label("Email Address")
                    | email.ToTextInput()
            )
            | new Card(
                Layout.Vertical().Gap(3)
                    | Text.H4("About")
                    | Text.Label("Bio")
                    | bio.ToTextInput()
                        .Variant(TextInputs.Textarea)
            )
            | new Card(
                Layout.Vertical().Gap(3)
                    | Text.H4("Preferences")
                    | new BoolInput<bool>(UseState(true))
                        .Label("Email notifications")
                    | new BoolInput<bool>(UseState(false))
                        .Label("SMS notifications")
            );

        return new HeaderLayout(formHeader, formContent);
    }
}
```

<Callout Type="tip">
HeaderLayout automatically handles height calculations and scrolling behavior, making it perfect for full-height application layouts.
</Callout>

<WidgetDocs Type="Ivy.HeaderLayout" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Layouts/HeaderLayout.cs"/>
