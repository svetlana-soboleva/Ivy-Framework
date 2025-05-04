# Markdown

The Markdown widget renders Markdown content as formatted HTML. It supports standard Markdown syntax including headings, lists, links, code blocks, tables, and more.

```csharp demo-tabs
public class BlogPostView : ViewBase
{
    public override object? Build()
    {
        var markdownContent = 
            """
            # The Power of Markdown
            
            Markdown makes it **easy** to create *formatted* content without dealing with HTML tags.
            
            ## Key Benefits
            
            1. Simple, easy-to-learn syntax
            2. Converts easily to HTML
            3. Readable in both plain text and rendered form
            4. Widely supported across platforms
            
            ## Code Example
            
            ```csharp
            public class Example
            {
                public void Demo()
                {
                    Console.WriteLine("Hello, Markdown!");
                }
            }
            ```
            
            ## Tables
            
            | Feature     | Support |
            |-------------|---------|
            | Headers     | ✅     |
            | Bold/Italic | ✅     |
            | Lists       | ✅     |
            | Code blocks | ✅     |
            | Tables      | ✅     |
            | Links       | ✅     |
            
            Check out [this link](https://example.com) for more information.
            
            > Markdown is a lightweight markup language that you can use to add formatting elements to plaintext text documents.
            
            ---
            
            ![Cats](https://api.images.cat/150/150)
            """;
            
        return new Markdown(markdownContent);
    }
}
```

<WidgetDocs Type="Ivy.Markdown" ExtensionTypes="Ivy.MarkdownExtensions"  SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Markdown.cs"/> 