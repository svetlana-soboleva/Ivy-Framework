# Article

Articles are container components for structured content with support for table of contents, navigation links, and document source references. They're perfect for documentation pages and long-form content.

Actually this page is build using an `Article` widget.

```csharp
public class GettingStartedApp : ViewBase
{
    public override object? Build()
    {
        var appDescriptor = this.UseService<AppDescriptor>();
        
        return new Article(
            Vertical()
                | H1("Getting Started with Ivy")
                | Markdown("Ivy is a powerful web framework for building data-centric applications.")
                | H2("Installation")
                | Markdown("First, install the required packages...")
                | H2("Basic Usage")
                | Markdown("Let's create a simple application...")
        )
        .Previous(appDescriptor.Previous)
        .Next(appDescriptor.Next)
        .DocumentSource(appDescriptor.DocumentSource);
    }
}
```

<WidgetDocs Type="Ivy.Article" ExtensionsType="Ivy.ArticleExtensions"/> 