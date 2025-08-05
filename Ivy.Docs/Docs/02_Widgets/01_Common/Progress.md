---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# Progress

The `Progress` widget is used to visually represent the completion status of a task or process. It displays a visual progress bar that can be customized with different color variants and can be bound to state for dynamic updates.

## Basic Usage

Here's a simple example of a progress bar initialized at 50%.

```csharp demo-below
new Progress(50)
```

## State-Bound Progress

The `Progress` widget can be bound to state for dynamic updates:

```csharp demo-below
var progress = this.UseState(25);

Layout.Vertical(
    new Progress(progress.Value),
    Layout.Horizontal(
        new Button("0%", _ => progress.Set(0)),
        new Button("25%", _ => progress.Set(25)),
        new Button("50%", _ => progress.Set(50)),
        new Button("75%", _ => progress.Set(75)),
        new Button("100%", _ => progress.Set(100))
    )
)
```

## Color Variants

The `Progress` widget supports different color variants to match your application's design:

### Primary (Default)
```csharp demo-below
new Progress(65)
```

### Emerald Gradient
```csharp demo-below
new Progress(65).ColorVariant(Progress.ColorVariants.EmeraldGradient)
```

## Goal Display

You can add a goal text to provide context about what the progress represents:

```csharp demo-below
Layout.Vertical(
    new Progress(75).Goal("Task completion"),
    new Progress(40).Goal("Upload progress").ColorVariant(Progress.ColorVariants.EmeraldGradient),
    new Progress(90).Goal("Loading...")
)
```

## Interactive Example

Here's a comprehensive interactive example that demonstrates progress tracking with increment/decrement controls:

```csharp demo-below
var taskProgress = this.UseState(0);
var uploadProgress = this.UseState(33);

Layout.Vertical()
    | Text.H3("Task Progress")
    | new Progress(taskProgress.Value).Goal($"Tasks completed: {taskProgress.Value}%")
    | Layout.Horizontal(
        new Button("Reset", _ => taskProgress.Set(0)).Secondary(),
        new Button("-10", _ => taskProgress.Set(Math.Max(0, taskProgress.Value - 10))),
        new Button("+10", _ => taskProgress.Set(Math.Min(100, taskProgress.Value + 10))),
        new Button("Complete", _ => taskProgress.Set(100))
    )
    | Spacer.Vertical(Size.Medium)
    | Text.H3("Upload Progress") 
    | new Progress(uploadProgress.Value)
        .Goal("Uploading files...")
        .ColorVariant(Progress.ColorVariants.EmeraldGradient)
    | Layout.Horizontal(
        new Button("Simulate Upload", _ => {
            uploadProgress.Set(0);
            // Simulate progress updates
            Task.Run(async () => {
                for(int i = 0; i <= 100; i += 10) {
                    uploadProgress.Set(i);
                    await Task.Delay(200);
                }
            });
        })
    )
```

## Common Use Cases

### File Upload Progress
```csharp demo-below
var fileProgress = this.UseState(67);
Layout.Vertical(
    new Progress(fileProgress.Value)
        .Goal("Uploading document.pdf")
        .ColorVariant(Progress.ColorVariants.EmeraldGradient),
    Text.Small($"{fileProgress.Value}% complete")
)
```

### Task Completion
```csharp demo-below  
var completedTasks = this.UseState(4);
var totalTasks = 7;
var percentage = (int)((completedTasks.Value / (double)totalTasks) * 100);

Layout.Vertical(
    new Progress(percentage).Goal($"Tasks: {completedTasks.Value}/{totalTasks}"),
    Layout.Horizontal(
        new Button("Complete Task", _ => completedTasks.Set(Math.Min(totalTasks, completedTasks.Value + 1))),
        new Button("Reset", _ => completedTasks.Set(0)).Secondary()
    )
)
```

### Loading States
```csharp demo-below
var loadingProgress = this.UseState(0);

Layout.Vertical(
    new Progress(loadingProgress.Value).Goal("Loading application..."),
    new Button("Start Loading", _ => {
        Task.Run(async () => {
            var steps = new[] { "Initializing...", "Loading modules...", "Connecting...", "Ready!" };
            for(int i = 0; i < steps.Length; i++) {
                loadingProgress.Set((i + 1) * 25);
                await Task.Delay(500);
            }
        });
    })
)
```

## Properties

The `Progress` widget supports the following properties:

- **Value**: `int?` - The progress value (0-100). Defaults to 0.
- **Goal**: `string?` - Optional text to display alongside the progress bar.
- **ColorVariant**: `Progress.ColorVariants` - The color theme. Options are `Primary` (default) and `EmeraldGradient`.

## Extension Methods

- **`.Value(IState<int>)`**: Binds the progress to a state variable.
- **`.Goal(string?)`**: Sets the goal text.
- **`.ColorVariant(Progress.ColorVariants)`**: Sets the color variant.

<WidgetDocs Type="Ivy.Progress" ExtensionTypes="Ivy.ProgressExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Progress.cs"/>