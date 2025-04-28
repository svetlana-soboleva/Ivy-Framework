# Todo Tutorial

In this tutorial, we'll create a simple todo application using Ivy. We'll learn about basic Ivy concepts like state management, components, and event handling.

## Prerequisites

Before starting this tutorial, make sure you have:
1. Ivy installed and set up in your development environment
2. Basic understanding of C# programming

## Creating the Todo App

Let's create a new todo application step by step.

### 1. Create a new project

Using the Ivy CLI we can create a new project.

```terminal
> ivy init -n TodoApp
```

### 2. Create the Todo Model

Then let's create a record `Todo.cs` to represent our todo items:

```csharp
public record Todo(string Title, bool Done);
```

This record has two properties:
- `Title`: The text description of the todo item
- `Done`: A boolean indicating whether the todo is completed

### 3. Create the Main App Class

Create a new class `TodosApp.cs` that inherits from `ViewBase` (or `SampleBase` if you're using samples):

```csharp
[App(icon: Icons.Calendar, path: ["Demos"])]
public class TodosApp : ViewBase
{
    public override object? Build()
    {
        // We'll add the implementation here
    }
}
```

### 4. Add State Management

Inside the `Build` method, we'll add state management for our todos and input field:

```csharp
var newTitle = UseState("");
var todos = UseState(ImmutableArray.Create<Todo>());
var client = UseService<IClientProvider>();
```

This creates:
- `newTitle`: State for the input field where users type new todo titles
- `todos`: State for storing the list of todo items
- `client`: Service for showing toast notifications

### 5. Build the UI

Now let's create the user interface. We'll use Ivy's layout system and components:

```csharp
return Layout.Vertical(
    new Card(
        Layout.Vertical(
            Layout.Horizontal(
                newTitle.ToTextInput(placeholder:"New todo...").Width(Size.Grow()),
                new Button("Add", onClick: _ =>
                    {
                        var title = newTitle.Value;
                        todos.Set(todos.Value.Add(new Todo(title, false)));
                        client.Toast($"New '{title}' todo added.", "Todos");
                        newTitle.Set("");
                    }
                ).Icon(Icons.Plus).Variant(ButtonVariant.Default)
            ).Width(Size.Full()),
            Layout.Vertical(
                todos.Value.Select(todo => new TodoItem(todo,
                    () =>
                    {
                        todos.Set(todos.Value.Remove(todo)); 
                    },
                    () =>
                    {
                        todos.Set(todos.Value.Replace(todo, todo with
                        {
                            Done = !todo.Done
                        }));
                    }
                ))
            )
        )
    ).Title("Todos").Width(1/2f)
);
```

### 6. Create the TodoItem Component

Create a new class `TodoItem.cs` for the todo item component:

```csharp
public class TodoItem(Todo todo, Action deleteTodo, Action toggleTodo) : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical(
            Layout.Horizontal(
                new BoolInput<bool>(todo.Done, _ =>
                {
                    toggleTodo();
                }),
                todo.Done ? Text.Muted(todo.Title).StrikeThrough().Width(Size.Grow()) : Text.Literal(todo.Title).Width(Size.Grow()) ,
                new Button(null, _ =>
                {
                    deleteTodo();
                }).Icon(Icons.Trash).Variant(ButtonVariant.Outline)
            ).Align(Align.Center).Width(Size.Full()),
            new Separator()
        );
    }
}
```

## Understanding the Components

Let's break down the key components and concepts used in this todo app:

### State Management
- `UseState`: Used to create reactive state variables
- `ImmutableArray`: Used for the todos list to ensure immutability
- `Set`: Method to update state values

### Layout Components
- `Layout.Vertical`: Arranges components vertically
- `Layout.Horizontal`: Arranges components horizontally
- `Card`: Container component with a title and styling
- `Separator`: Visual divider between items

### Input Components
- `TextInput`: For entering new todo titles
- `BoolInput`: Checkbox for toggling todo completion
- `Button`: For adding new todos and deleting existing ones

### Text Components
- `Text.Literal`: Regular text display
- `Text.Muted`: Styled text for completed items
- `StrikeThrough`: Text decoration for completed items

### Event Handling
- `onClick`: Handles button click events
- `toggleTodo`: Toggles todo completion status
- `deleteTodo`: Removes a todo from the list

## Features

This todo app includes the following features:
1. Add new todos
2. Mark todos as complete/incomplete
3. Delete todos
4. Visual feedback with toast notifications
5. Responsive layout
6. Clean and modern UI

## Running the App

To run the app:
1. Make sure all the code is in place
2. Build and run your Ivy application
3. Navigate to the Demos section
4. You should see your todo app with the calendar icon

## Next Steps

You can enhance this todo app by adding features like:
1. Persistence (saving todos to a database)
2. Categories or tags for todos
3. Due dates
4. Priority levels
5. Search and filtering
6. Sorting options

## Conclusion

This tutorial demonstrated how to create a functional todo application using Ivy. We covered:
- Basic state management
- Component creation
- Layout system
- Event handling
- UI components

The app serves as a good foundation for learning Ivy's core concepts and can be extended with additional features as needed.