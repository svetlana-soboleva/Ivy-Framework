# Animation

The Animation widget wraps any child widget and animates it using the
`AnimationType` you specify. Animations can be triggered automatically, on click
or on hover.

```csharp
Icons.LoaderCircle.ToIcon()
    .WithAnimation(AnimationType.Rotate)
    .Duration(1);
```

<WidgetDocs Type="Ivy.Animation" ExtensionTypes="Ivy.AnimationExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Effects/Animation.cs"/>
