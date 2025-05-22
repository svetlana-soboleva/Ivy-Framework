# Confetti

Confetti adds a fun celebratory effect to any widget. The confetti can be
triggered automatically, on click or when the mouse hovers the widget.

```csharp demo
var onClick = new Button("Click")
    .HandleClick(() => client.Toast("Did you see the confetti?"))
    .WithConfetti(AnimationTrigger.Click);
```

<WidgetDocs Type="Ivy.Confetti" ExtensionTypes="Ivy.ConfettiExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Effects/Confetti.cs"/>
