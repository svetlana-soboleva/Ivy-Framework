# Confetti

<Ingress Text="Add celebratory confetti effects to any widget with customizable triggers for automatic, click, or hover activation." />

`Confetti` adds a fun celebratory effect to any widget by wrapping it and triggering confetti animations. The confetti can be triggered automatically, on click, or when the mouse hovers over the widget. Perfect for celebrating user achievements, form submissions, or adding delightful interactions to your interface.

## Basic Usage

Wrap any widget with confetti using the `WithConfetti()` extension method:

```csharp demo-tabs
new Button("Celebrate!")
    .WithConfetti(AnimationTrigger.Click)
```

### Auto Trigger

Confetti fires automatically when the widget is first rendered:

```csharp demo-tabs
Text.Block("Welcome!")
    .WithConfetti(AnimationTrigger.Auto)
```

## Animation Triggers

Confetti supports three different trigger modes that control when the effect activates:

### Click Trigger

Confetti fires when the user clicks on the wrapped widget:

```csharp demo-tabs
new Button("Click Me!")
    .WithConfetti(AnimationTrigger.Click)
```

### Hover Trigger

Confetti activates when the mouse hovers over the widget:

```csharp demo-tabs
new Card("Hover over me")
    .WithConfetti(AnimationTrigger.Hover)
```

## Common Use Cases

### Task Completion Celebration

```csharp demo-tabs
Layout.Vertical().Gap(20)
    | new Card("Task Manager")
    | new Button("Complete Task").WithConfetti(AnimationTrigger.Click)
    | Text.Block("Task completed! 🎉").WithConfetti(AnimationTrigger.Auto)
```

### Achievement Unlocking

```csharp demo-tabs
Layout.Vertical().Gap(20)
    | new Button("Unlock Achievement").WithConfetti(AnimationTrigger.Click)
    | new List(new[] { "First Steps", "Quick Learner", "Power User" }
        .Select(achievement => new ListItem(achievement)))
```

### Interactive Elements

```csharp demo-tabs
Layout.Vertical().Gap(20)
    | Text.Block("Interactive elements with confetti:")
    | new Button("Click me").WithConfetti(AnimationTrigger.Click)
    | new Card("Hover me").WithConfetti(AnimationTrigger.Hover)
    | new Badge("Auto confetti").WithConfetti(AnimationTrigger.Auto)
```

## Advanced Patterns

### Conditional Confetti

Show confetti only under certain conditions:

```csharp demo-tabs
Layout.Vertical().Gap(20)
    | new Button("Special Action").WithConfetti(AnimationTrigger.Click)
    | new Button("Auto Confetti").WithConfetti(AnimationTrigger.Auto)
```

### Confetti with State Changes

```csharp demo-tabs
Layout.Vertical().Gap(20)
    | new Button("Complete Task").WithConfetti(AnimationTrigger.Click)
    | Text.Block("Task completed! 🎉").WithConfetti(AnimationTrigger.Auto)
```

<WidgetDocs Type="Ivy.Confetti" ExtensionTypes="Ivy.ConfettiExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Effects/Confetti.cs"/>

## Advanced Examples

## Integration with Other Widgets

Confetti works seamlessly with all Ivy widgets:

```csharp demo-tabs
Layout.Vertical().Gap(10)
    | new Button("Action").WithConfetti(AnimationTrigger.Click)
    | new Card("Content").WithConfetti(AnimationTrigger.Hover)
    | new ListItem("Item").WithConfetti(AnimationTrigger.Click)
    | Text.Block("Message").WithConfetti(AnimationTrigger.Auto)
    | new Badge("Success").WithConfetti(AnimationTrigger.Auto)
```
