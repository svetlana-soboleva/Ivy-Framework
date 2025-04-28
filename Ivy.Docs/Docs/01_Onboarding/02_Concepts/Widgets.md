# Widgets

Widgets are the fundamental building blocks of the Ivy framework. They represent the smallest unit of UI and are used to construct Views. Inspired by React's component model, Widgets provide a declarative way to build user interfaces.

## What are Widgets?

A Widget is a C# record that inherits from `WidgetBase<T>` and implements the `IWidget` interface. Each Widget represents a specific UI element or container that can be composed with other Widgets to create complex user interfaces.

## Core Widget Properties

All Widgets inherit these basic properties from `WidgetBase<T>`:

- `Width`: Controls the width of the widget
- `Height`: Controls the height of the widget
- `Visible`: Controls the visibility of the widget

## Widget Categories

### Primitives
Basic UI elements that serve as building blocks:

- `TextBlock`: For displaying text
- `Markdown`: For rendering markdown content
- `Html`: For rendering HTML content
- `Json`: For displaying JSON data
- `Code`: For code snippets
- `Image`: For displaying images
- `Icon`: For icons
- `Box`: A basic container
- `Article`: For article-style content
- `Avatar`: For user avatars
- And more...

### Layout Widgets
Widgets that control the arrangement of other widgets:

- `StackLayout`: Arranges widgets in a vertical or horizontal stack
- `WrapLayout`: Wraps widgets to the next line when they exceed container width
- `GridLayout`: Arranges widgets in a grid
- `TabsLayout`: Creates tabbed interfaces
- `SidebarLayout`: Creates sidebar-based layouts
- `ResizeablePanelGroup`: Creates resizable panel layouts
- `FloatingPanel`: Creates floating/overlay panels

### Input Widgets
Widgets for user input:

- `TextInput`: For text input
- `BoolInput`: For boolean values
- `DateTimeInput`: For date and time selection
- `NumberInput`: For numeric input
- `SelectInput`: For dropdown selections
- `CodeInput`: For code editing
- `FileInput`: For file uploads
- And more...

### Form Widgets
Widgets for building forms:

- `Form`: Container for form elements
- `FormField`: Individual form field wrapper

### Dialog Widgets
Widgets for modal dialogs:

- `Dialog`: Modal dialog container
- `DialogHeader`: Dialog header section
- `DialogBody`: Dialog content section
- `DialogFooter`: Dialog footer section

### Table Widgets
Widgets for displaying tabular data:

- `Table`: Table container
- `TableRow`: Table row
- `TableCell`: Table cell

### List Widgets
Widgets for displaying lists:

- `List`: List container
- `ListItem`: Individual list item

### Chart Widgets
Widgets for data visualization:

- `LineChart`: Line chart visualization
- `PieChart`: Pie chart visualization
- `AreaChart`: Area chart visualization
- `BarChart`: Bar chart visualization

### Effect Widgets
Widgets for visual effects:

- `Confetti`: Confetti animation effect
- `Animation`: General animation container

## Using Widgets

Widgets can be used in two ways:

1. As direct UI elements:
```csharp
return new TextBlock("Hello, World!")
    .Width(Size.Units(200))
    .Height(Size.Auto());
```

2. As containers for other widgets:
```csharp
return new StackLayout(
    new TextBlock("Header"),
    new TextBlock("Content"),
    new TextBlock("Footer")
).Width(Size.Full());
```

## Widget Composition

Widgets can be composed using the `|` operator or by passing them as children in the constructor:

```csharp
// Using the | operator
var widget = new StackLayout()
    | new TextBlock("First")
    | new TextBlock("Second");

// Using constructor
var widget = new StackLayout(
    new TextBlock("First"),
    new TextBlock("Second")
);
```

## Styling Widgets

Widgets can be styled using various extension methods:

```csharp
new TextBlock("Styled Text")
    .Width(Size.Units(200))
    .Height(Size.Auto())
    .Visible(true);
```

## Best Practices

1. Keep widgets focused and single-purpose
2. Use composition to build complex UIs
3. Leverage the built-in layout widgets for responsive designs
4. Use appropriate input widgets for different data types
5. Consider using form widgets for data entry scenarios
6. Use dialog widgets for modal interactions
7. Utilize chart widgets for data visualization