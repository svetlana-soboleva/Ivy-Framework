﻿---
searchHints:
  - tutorial
  - example
  - todo
  - walkthrough
  - guide
  - step-by-step
---

# Todo Tutorial

<Ingress>
Build a complete todo application from scratch to learn essential Ivy concepts including state management, components, and event handling.
</Ingress>

## Prerequisites

Before starting this tutorial, make sure you have [installed](02_Installation.md) Ivy.

## Creating the Todo App

Let's create a new todo app step by step.

### 1. Create a new project

Using the Ivy CLI we can create a new project.

```terminal
>ivy init --namespace Todos
```

### 2. Create the Todo Model

Create a new file `TodosApp.cs` in the `Apps` folder.
Declare a record `Todo.cs` to represent our todo items:

```csharp
public record Todo(string Title, bool Done);
```

### 3. Create the Main App Class

Create a new class `TodosApp` in the file that inherits from `ViewBase`:

```csharp
[App(icon: Icons.Calendar)]
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
//State for the input field where users type new todo titles
var newTitle = this.UseState("");
//State for storing the list of todo items
var todos = this.UseState(ImmutableArray.Create<Todo>());

//Service for showing toast notifications
var client = this.UseService<IClientProvider>();
```

### 5. Build the UI

Now let's create the user interface. We'll use Ivy's layout system and components:

```csharp
return new Card().Title("Todos").Description("What do you want to get done today?")
   | (Layout.Vertical()
       | (Layout.Horizontal().Width(Size.Full())
          | newTitle.ToTextInput(placeholder: "New Task...").Width(Size.Grow())
          | new Button("Add", onClick: _ =>
              {
                  var title = newTitle.Value;
                  todos.Set(todos.Value.Add(new Todo(title, false)));
                  client.Toast($"New '{title}' todo added.", "Todos");
                  newTitle.Set("");
              }
          ).Icon(Icons.Plus).Variant(ButtonVariant.Primary)
       )
       | (Layout.Vertical()
          | todos.Value.Select(todo => new TodoItem(todo,
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
       ))
    ;
```

### 6. Create the TodoItem Component

Create a new class `TodoItem.cs` for the todo item view:

```csharp
public class TodoItem(Todo todo, Action deleteTodo, Action toggleTodo) : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical()
           | (Layout.Horizontal().Align(Align.Center).Width(Size.Full())
              | new BoolInput<bool>(todo.Done, _ =>
              {
                  toggleTodo();
              })
              | (todo.Done
                  ? Text.Muted(todo.Title).StrikeThrough().Width(Size.Grow())
                  : Text.Literal(todo.Title).Width(Size.Grow()))
              | new Button(null, _ =>
                  {
                      deleteTodo();
                  }
              ).Icon(Icons.Trash).Variant(ButtonVariant.Outline)
           )
           | new Separator()
        ;
    }
}
```

### 7. Run

Now let's run the project.

```terminal
>dotnet watch
```

Get your application's complete server description

```terminal

>dotnet run --describe

Output (YAML format):
apps:
   - name: Dashboard
     id: dashboard
     isVisible: true
   - name: Settings
     id: settings
     isVisible: true
connections:
   - name: MainDatabase
     connectionType: EntityFramework.SqlServer
     namespace: MyApp.Data
   - name: ExternalApi
     connectionType: OpenAPI
     namespace: MyApp.Services
secrets:
   - ConnectionStrings:Database
   - Auth0:ClientId
   - Stripe:ApiKey
services:
   - serviceType: IUserService
     implementationType: UserService
     lifetime: Scoped
```

You can find the source code for the app at <https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy.Samples.Shared/Apps/Demos/TodosApp.cs>.
