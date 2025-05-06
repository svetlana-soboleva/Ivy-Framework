namespace Todos.Apps;

public record Todo(string Title, bool Done);

[App(icon: Icons.Calendar)]
public class TodosApp : ViewBase
{
    public override object? Build()
    {
        //States: 
        var newTitle = this.UseState("");
        var todos = this.UseState(ImmutableArray.Create<Todo>());

        //Service for showing toast notifications:
        var client = this.UseService<IClientProvider>();

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
                      ).Icon(Icons.Plus).Variant(ButtonVariant.Default)
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
    }
}

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