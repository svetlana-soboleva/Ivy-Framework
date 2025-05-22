# Tooltip

Tooltips provide contextual information when hovering or focusing on a widget.

## Basic Usage

```csharp
new Tooltip(new Button("Hover Me"), "Hello World!");
```

You can also add a tooltip using the extension method:

```csharp
new Button("Hover Me").WithTooltip("Hello World!");
```

<WidgetDocs Type="Ivy.Tooltip" ExtensionTypes="Ivy.TooltipExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Tooltip.cs"/>
