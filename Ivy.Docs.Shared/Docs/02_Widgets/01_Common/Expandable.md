# Expandable

<Ingress>
Create collapsible content sections that users can expand and collapse to maintain clean, organized layouts.
</Ingress>

The `Expandable` widget allows you to hide and show content interactively, providing a clean and organized way to present information. It's perfect for organizing content into collapsible sections, FAQs, or any scenario where you want to reduce visual clutter.

## Basic Usage

Here's a simple example of an expandable widget.

```csharp demo-below 
new Expandable("Click to expand", "This is the hidden content that appears when you expand the widget.")
```

## Content Types

### Text Content

Expandable widgets can contain various types of content, starting with simple text.

```csharp demo-tabs
new Expandable("Simple Text", "This is a simple text content that gets hidden and shown.")
```

### Rich Content

You can include more complex content like formatted text, icons, and other widgets.

```csharp demo-tabs
new Expandable(
    "Rich Content Example", 
    Layout.Vertical().Gap(2)
        | Text.H3("Welcome to Ivy Framework")
        | Text.Muted("This expandable contains multiple elements")
        | new Badge("New").Secondary()
        | Text.Small("Click to collapse this content")
)
```

### Nested Expandables

Create hierarchical structures by nesting expandable widgets.

```csharp demo-tabs
new Expandable("Main Section", 
    Layout.Vertical().Gap(2)
        | Text.H4("Overview")
        | Text.Muted("This is the main content")
        | new Expandable("Subsection 1", "Details about subsection 1")
        | new Expandable("Subsection 2", "Details about subsection 2")
        | new Expandable("Subsection 3", "Details about subsection 3")
)
```

## Layout Integration

### In Vertical Layouts

Expandables work seamlessly with vertical layouts for organized content presentation.

```csharp demo-tabs
Layout.Vertical().Gap(2)
    | new Expandable("Getting Started", "Follow these steps to get started with Ivy Framework...")
    | new Expandable("Configuration", "Configure your application with these settings...")
    | new Expandable("Deployment", "Learn how to deploy your Ivy application...")
    | new Expandable("Troubleshooting", "Common issues and their solutions...")
```

### In Grid Layouts

Use expandables in grid layouts for organized information display.

```csharp demo-tabs
Layout.Grid().Columns(2).Gap(2)
    | new Expandable("Features", "Discover all the amazing features of Ivy Framework")
    | new Expandable("Performance", "Learn about performance optimizations and best practices")
    | new Expandable("Security", "Security considerations and best practices")
    | new Expandable("Scalability", "How to scale your Ivy applications")
```

### In Cards

Combine expandables with cards for enhanced visual presentation.

```csharp demo-tabs
Layout.Grid().Columns(2).Gap(2)
    | new Card(new Expandable("Documentation", "Access comprehensive documentation and guides"))
    | new Card(new Expandable("Examples", "Browse through practical examples and use cases"))
    | new Card(new Expandable("API Reference", "Detailed API documentation and examples"))
    | new Card(new Expandable("Community", "Join our community and get support"))
```

## Advanced Patterns

### Conditional Content

Show different content based on conditions or state.

```csharp demo-tabs
Layout.Vertical().Gap(2)
    | new Expandable("User Profile", 
        Layout.Vertical().Gap(2)
            | Text.H4("John Doe")
            | Text.Muted("john.doe@example.com")
            | new Badge("Premium").Secondary()
            | new Expandable("Preferences", "User preferences and settings")
            | new Expandable("History", "User activity and history")
    )
    | new Expandable("System Status", 
        Layout.Horizontal().Gap(2)
            | new Badge("Online").Secondary()
            | Text.Muted("All systems operational")
    )
```

### Interactive Content

Include interactive elements within expandable content.

```csharp demo-tabs
new Expandable("Interactive Demo", 
    Layout.Vertical().Gap(2)
        | Text.H4("Try it out!")
        | Layout.Horizontal().Gap(2)
            | new Button("Click me!").Primary()
            | new Button("Reset").Secondary()
        | new Progress(75)
        | Text.Small("Progress: 75%")
)
```

### Form Sections

Organize forms into logical, collapsible sections.

```csharp demo-tabs
Layout.Vertical().Gap(2)
    | new Expandable("Personal Information", 
        Layout.Vertical().Gap(2)
            | Text.H4("Basic Details")
            | Layout.Grid().Columns(2).Gap(2)
                | Text.Small("First Name")
                | Text.Small("Last Name")
                | Text.Small("Email")
                | Text.Small("Phone")
    )
    | new Expandable("Address", 
        Layout.Vertical().Gap(2)
            | Text.H4("Location Details")
            | Layout.Grid().Columns(2).Gap(2)
                | Text.Small("Street")
                | Text.Small("City")
                | Text.Small("State")
                | Text.Small("ZIP Code")
    )
    | new Expandable("Preferences", 
        Layout.Vertical().Gap(2)
            | Text.H4("User Preferences")
            | Layout.Grid().Columns(2).Gap(2)
                | Text.Small("Language")
                | Text.Small("Theme")
                | Text.Small("Notifications")
                | Text.Small("Privacy")
    )
```

## Properties

The `Expandable` widget supports the following properties:

- **Disabled**: Set to `true` to disable the expandable functionality

```csharp demo-tabs
Layout.Vertical().Gap(2)
    | new Expandable("Normal", "This expandable works normally")
    | new Expandable("Disabled", "This expandable is disabled").Disabled(true)
```

## Best Practices

1. **Clear Headers**: Use descriptive, concise headers that clearly indicate the content
2. **Logical Grouping**: Group related information into expandable sections
3. **Consistent Structure**: Maintain consistent patterns across your application
4. **Performance**: Avoid nesting too many expandables deeply
5. **Accessibility**: Ensure headers are descriptive for screen readers

## Common Use Cases

- **FAQs**: Organize frequently asked questions
- **Documentation**: Structure documentation into logical sections
- **Forms**: Group form fields into collapsible sections
- **Settings**: Organize application settings and preferences
- **Data Display**: Show detailed information on demand

<WidgetDocs Type="Ivy.Expandable" ExtensionTypes="Ivy.ExpandableExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Expandable.cs"/>