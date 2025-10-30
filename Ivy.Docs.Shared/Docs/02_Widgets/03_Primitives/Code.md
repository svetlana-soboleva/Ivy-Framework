---
searchHints:
  - syntax
  - highlighting
  - code-block
  - snippet
  - pre
  - programming
---

# Code

<Ingress>
Display beautifully formatted code snippets with syntax highlighting, line numbers, and copy functionality for multiple programming languages.
</Ingress>

The `Code` widget displays formatted code snippets with syntax highlighting. It supports multiple programming languages and features line numbers and copy buttons for better user experience.

```csharp demo-tabs
Layout.Vertical()
    | new Code(@"function fibonacci(n) {
  if (n <= 1) return n;
  return fibonacci(n-1) + fibonacci(n-2);
}
    
// Print first 10 Fibonacci numbers
for (let i = 0; i < 10; i++) {
  console.log(fibonacci(i));
  }
  ")
      .ShowLineNumbers()
      .ShowCopyButton()
      .Language(Languages.Javascript)
      .Width(Size.Full())
      .Height(Size.Auto())
```

<WidgetDocs Type="Ivy.Code" ExtensionTypes="Ivy.CodeExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Code.cs"/>
