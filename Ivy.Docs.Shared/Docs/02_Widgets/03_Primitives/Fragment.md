# Fragment

<Ingress>
Group multiple elements without adding extra DOM markup, similar to React Fragments, for clean component composition.
</Ingress>

The `Fragment` widget is a container component that doesn't produce any HTML elements itself. It's useful for grouping multiple elements without adding extra markup to the DOM, similar to React Fragments. This makes it perfect for conditional rendering, returning multiple widgets from a view, and creating clean component compositions.

## Basic Usage

The simplest use case is grouping multiple widgets together without adding extra markup:

```csharp demo-tabs
public class BasicFragmentView : ViewBase
{
    public override object? Build()
    {
        return new Fragment(
            Text.P("This is a simple example of using Fragment to group multiple elements.")
        );
    }
}
```

## Conditional Rendering

Fragment is excellent for conditional rendering, allowing you to show or hide content based on state:

```csharp demo-tabs
public class ConditionalRenderingView : ViewBase
{
    public override object? Build()
    {
        var showAdminControls = UseState(false);
        var showUserProfile = UseState(true);
        
        return Layout.Vertical().Gap(4)
            | Text.H2("User Dashboard")
            | new Fragment(
                showAdminControls.Value
                    ? new Fragment(
                        Text.H3("Admin Controls"),
                        new Button("Reset System", variant: ButtonVariant.Destructive),
                        new Button("View Logs"),
                        new Button("Manage Users")
                      )
                    : Text.Muted("Admin controls are hidden")
              )
            | new Fragment(
                showUserProfile.Value
                    ? new Fragment(
                        Text.H3("User Profile"),
                        new Button("Edit Profile"),
                        new Button("Change Password")
                      )
                    : null
              );
    }
}
```

## Multiple Return Elements

Fragment allows you to return multiple widgets from a single view method:

```csharp demo-tabs
public class MultipleElementsView : ViewBase
{
    public override object? Build()
    {
        var selectedTab = UseState(0);
        var tabs = new[] { "Overview", "Details", "Settings" };
        
        return new Fragment(
            // Header section
            Layout.Horizontal().Gap(4)
                | Text.H1("Application")
                | new Button("Save", _ => {})
                | new Button("Cancel", _ => {})
                | new Spacer()
                | new Button("Help", _ => {}),
            
            // Tab navigation
            Layout.Horizontal().Gap(2)
                | tabs.Select((tab, index) => 
                    new Button(tab, _ => selectedTab.Set(index))
                        .Variant(selectedTab.Value == index ? ButtonVariant.Primary : ButtonVariant.Secondary)
                ),
            
            // Content area
            new Card(
                selectedTab.Value == 0 ? Text.P("Overview content here...") :
                selectedTab.Value == 1 ? Text.P("Details content here...") :
                Text.P("Settings content here...")
            ).Title("Content")
        );
    }
}
```

## Clean Component Composition

Fragment helps create clean, readable component compositions in complex layouts:

```csharp demo-tabs
public class ComponentCompositionView : ViewBase
{
    public override object? Build()
    {
        var isExpanded = UseState(false);
        
        return Layout.Vertical().Gap(4)
            | new Fragment(
                // Always visible header
                Layout.Horizontal().Gap(2)
                    | Text.H2("Expandable Section")
                    | new Spacer()
                    | new Button(
                        isExpanded.Value ? "Collapse" : "Expand",
                        _ => isExpanded.Set(!isExpanded.Value)
                    ).Icon(isExpanded.Value ? Icons.ChevronUp : Icons.ChevronDown),
                
                // Conditional content
                isExpanded.Value ? new Fragment(
                    new Separator(),
                    Text.P("This content is only visible when expanded."),
                    Layout.Horizontal().Gap(2)
                        | new Button("Action 1", _ => {})
                        | new Button("Action 2", _ => {})
                ) : null
              );
    }
}
```

## Advanced: Dynamic Content Generation

Fragment can be used to dynamically generate content based on data:

```csharp demo-tabs
public class DynamicContentView : ViewBase
{
    public override object? Build()
    {
        var items = UseState(() => new string[] { "Item 1", "Item 2", "Item 3", "Item 4" });
        var showDetails = UseState(false);
        
        return Layout.Vertical().Gap(4)
            | Text.H2("Dynamic Content")
            | new Fragment(
                // Static controls
                Layout.Horizontal().Gap(2)
                    | new Button("Add Item", _ => {
                        var newItems = items.Value.Append($"Item {items.Value.Length + 1}").ToArray();
                        items.Set(newItems);
                      })
                    | new Button("Toggle Details", _ => showDetails.Set(!showDetails.Value)),
                
                // Dynamic list
                items.Value.Select(item => new Fragment(
                    new Card(item).Title("List Item"),
                    showDetails.Value ? Text.Small($"Details for {item}") : null
                ))
              );
    }
}
```

<WidgetDocs Type="Ivy.Fragment" ExtensionTypes="Ivy.FragmentExtensions"  SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Fragment.cs"/>
