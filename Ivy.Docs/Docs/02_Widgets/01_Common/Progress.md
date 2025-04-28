---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# Progress

The Progress widget is used to visually represent the completion status of a task or process. It can display a percentage of completion and can be customized with different color variants.

## Basic Usage

Here's a simple example of a progress bar initialized at 50%.

```csharp demo-below
new Progress(50)
```

<WidgetDocs Type="Ivy.Progress" ExtensionsType="Ivy.ProgressExtensions"/>