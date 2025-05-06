# TextBlock

The TextBlock widget displays text content with customizable styling. It's a fundamental building block for creating user interfaces with text, supporting various formatting options and layout properties.

This widget is rarely used directly. Instead, we use the helper class `Ivy.Helpers.Text` which provides a more user-friendly API for creating text elements.

## Text Helper

```csharp
public class TextDemo : ViewBase
{   
    public object? Build()
    {
        return Layout.Vertical()
            | Text.Literal("Literal")
            | Text.H1("H1")
            | Text.H2("H2")
            | Text.H3("H3")
            | Text.H4("H4")
            | Text.Block("Block")
            | Text.Blockquote("Blockquote")
            | Text.InlineCode("InlineCode")
            | Text.Lead("Lead")
            | Text.Large("Large")
            | Text.Small("Small")
            | Text.Muted("Muted")
            | Text.Danger("Danger")
            | Text.Warning("Warning")
            | Text.Success("Success")
    }
}
```

## TextBuilder Modifiers

