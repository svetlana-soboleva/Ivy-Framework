# Html

The Html widget allows you to render raw HTML content in your Ivy application. This is useful when you need to include content from external sources or when you want direct control over the markup.

```csharp demo-tabs
public class HtmlContentView : ViewBase
{
    public override object? Build()
    {
        var htmlContent = 
            """
            <div style="background-color: #f5f5f5; padding: 20px; border-radius: 8px;">
              <h2 style="color: #3498db;">Custom HTML Content</h2>
              <p>This is <em>rendered</em> as <strong>raw HTML</strong> content.</p>
              <ul>
                <li>You can include complex formatting</li>
                <li>Add custom styling</li>
                <li>Include tables, lists, and more</li>
              </ul>
              <table border="1" style="width: 100%; border-collapse: collapse;">
                <tr>
                  <th style="padding: 8px; text-align: left;">Feature</th>
                  <th style="padding: 8px; text-align: left;">Status</th>
                </tr>
                <tr>
                  <td style="padding: 8px;">Custom HTML</td>
                  <td style="padding: 8px; color: green;">Supported</td>
                </tr>
                <tr>
                  <td style="padding: 8px;">Complex Tables</td>
                  <td style="padding: 8px; color: green;">Supported</td>
                </tr>
              </table>
            </div>
            """;
        
        return Layout.Vertical().Gap(4)
            | Text.H1("HTML Widget Example")
            | new Html(htmlContent);
    }
}
```

<WidgetDocs Type="Ivy.Html" ExtensionsType="Ivy.HtmlExtensions"/> 