# ResizeablePanelGroup

<Ingress>
Create flexible, resizable layouts with draggable handles that allow users to dynamically adjust panel sizes in your applications.
</Ingress>

The `ResizeablePanelGroup` widget creates layouts with multiple panels separated by draggable handles, allowing users to resize sections interactively. Panels can be arranged horizontally or vertically and support nesting for complex layouts.

## Basic Usage

The simplest resizable panel group consists of two or more panels arranged horizontally:

```csharp demo-tabs 
public class BasicResizeablePanelView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(30, new Card("Left Panel")),
            new ResizeablePanel(70, new Card("Right Panel"))
        );
    }
}
```

## Orientation

### Horizontal Layout (Default)

Panels are arranged side by side with vertical drag handles:

```csharp demo-tabs 
public class HorizontalResizeableView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(25, new Card("Sidebar")),
            new ResizeablePanel(50, new Card("Main Content")),
            new ResizeablePanel(25, new Card("Info Panel"))
        ).Horizontal();
    }
}
```

### Vertical Layout

Panels are stacked vertically with horizontal drag handles:

```csharp demo-tabs 
public class VerticalResizeableView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(30, new Card("Header")),
            new ResizeablePanel(50, new Card("Content")),
            new ResizeablePanel(20, new Card("Footer"))
        ).Vertical();
    }
}
```

## Panel Sizing

### Default Sizes

Each panel can have a default size specified as a percentage of the total space:

```csharp demo-tabs 
public class DefaultSizesView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(20, new Card("20% width")),
            new ResizeablePanel(60, new Card("60% width")),
            new ResizeablePanel(20, new Card("20% width"))
        );
    }
}
```

### Auto-Sizing

Panels without specified sizes will automatically distribute the remaining space:

```csharp demo-tabs 
public class AutoSizingView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(25, new Card("Fixed 25%")),
            new ResizeablePanel(null, new Card("Auto size")),
            new ResizeablePanel(null, new Card("Auto size"))
        );
    }
}
```

## Handle Visibility

### Show/Hide Resize Handles

Control whether the drag handles are visible and functional:

```csharp demo-tabs 
public class HandleVisibilityView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical()
            | Text.Block("With Handles (Default)")
            | new ResizeablePanelGroup(
                new ResizeablePanel(50, new Card("Panel A")),
                new ResizeablePanel(50, new Card("Panel B"))
            ).ShowHandle(true)
            | Text.Block("Without Handles")
            | new ResizeablePanelGroup(
                new ResizeablePanel(50, new Card("Panel A")),
                new ResizeablePanel(50, new Card("Panel B"))
            ).ShowHandle(false);
    }
}
```

## Nested Layouts

Create complex layouts by nesting ResizeablePanelGroups:

```csharp demo-tabs 
public class NestedLayoutView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(25, new Card("Sidebar")),
            new ResizeablePanel(75,
                new ResizeablePanelGroup(
                    new ResizeablePanel(60, new Card("Main Content")),
                    new ResizeablePanel(40,
                        new ResizeablePanelGroup(
                            new ResizeablePanel(50, new Card("Top Right")),
                            new ResizeablePanel(50, new Card("Bottom Right"))
                        ).Vertical())
                ).Horizontal())
        ).Horizontal();
    }
}
```

## Full-Height Layouts

For layouts that should fill the entire viewport height:

```csharp demo-tabs 
public class FullHeightView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(20, new Card("Navigation")),
            new ResizeablePanel(60, new Card("Main")),
            new ResizeablePanel(20, new Card("Sidebar"))
        ).Height(Size.Screen());
    }
}
```

## Advanced Examples

### IDE-Style Layout

Create a layout similar to an integrated development environment:

```csharp demo-tabs 
public class IDELayoutView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            // Left sidebar
            new ResizeablePanel(20, 
                new Card(
                    Layout.Vertical()
                        | Text.H3("Explorer")
                        | Text.Block("• src/")
                        | Text.Block("• components/")
                        | Text.Block("• pages/")
                )),
            // Main area
            new ResizeablePanel(60,
                new ResizeablePanelGroup(
                    // Code editor
                    new ResizeablePanel(70, 
                        new Card(
                            Layout.Vertical()
                                | Text.H3("Editor")
                                | Text.Code("function hello() {\n  console.log('Hello!');\n}")
                        )),
                    // Bottom panel
                    new ResizeablePanel(30,
                        new Card(
                            Layout.Vertical()
                                | Text.H3("Terminal")
                                | Text.Block("$ npm start")
                                | Text.Block("Server running on port 3000")
                        ))
                ).Vertical()),
            // Right sidebar
            new ResizeablePanel(20,
                new Card(
                    Layout.Vertical()
                        | Text.H3("Properties")
                        | Text.Block("Component props")
                        | Text.Block("State variables")
                ))
        ).Height(Size.Screen());
    }
}
```

### Dashboard with Resizable Widgets

Create a dashboard where users can resize different sections:

```csharp demo-tabs 
public class DashboardView : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(30,
                new ResizeablePanelGroup(
                    new ResizeablePanel(50, 
                        new Card(
                            Layout.Vertical()
                                | Text.H3("Metrics")
                                | Text.Block("Revenue: $45,231")
                                | Text.Block("Users: 1,234")
                        )),
                    new ResizeablePanel(50,
                        new Card(
                            Layout.Vertical()
                                | Text.H3("Alerts")
                                | Text.Block("• System healthy")
                                | Text.Block("• 2 warnings")
                        ))
                ).Vertical()),
            new ResizeablePanel(70,
                new ResizeablePanelGroup(
                    new ResizeablePanel(60,
                        new Card(
                            Layout.Vertical()
                                | Text.H3("Analytics Chart")
                                | Text.Block("[Chart would go here]")
                        )),
                    new ResizeablePanel(40,
                        new Card(
                            Layout.Vertical()
                                | Text.H3("Recent Activity")
                                | Text.Block("• User logged in")
                                | Text.Block("• Order processed")
                                | Text.Block("• Payment received")
                        ))
                ).Vertical())
        );
    }
}
```

<WidgetDocs Type="Ivy.ResizeablePanelGroup" ExtensionTypes="Ivy.ResizeablePanelsExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Layouts/ResizeablePanelGroup.cs"/>
