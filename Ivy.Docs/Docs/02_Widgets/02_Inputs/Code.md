---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# CodeInput

The CodeInput widget provides a specialized text input field for entering and editing code with syntax highlighting. 
It supports various programming languages and offers features like line numbers and code formatting.

## Basic Usage

Here's a simple example of a CodeInput widget:

```csharp
public class JSCodeDemo: ViewBase
{
    public override object? Build()
    {
        var code = """
                function helloWorld() {
                    console.log('Hello, world!');
                }
                """;

        var codeState = UseState(code);
        return Layout.Horizontal() 
                | codeState.ToCodeInput().Language(Languages.Javascript);
    }
}        
```


## Variants

The `CodeInput` widget can be customized with different languages for syntax highlighting:

### Languages.Csharp variant 

The following code shows how to use this variant to make the syntax highlighting work 
for C#. 

```csharp demo-below
public class CSharpLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
        var csCode = UseState("Console.WriteLine(\"Hello World!\");");
        return Layout.Vertical()
                    | Text.H3("C#")
                    | Text.Html("<i>Enter C# code below!</i>") 
                    | csCode.ToCodeInput().Language(Languages.Csharp);
    }
}
```

### Languages.JavaScript variant

The following code shows how to use this variant to make the syntax highlighting work
for Javascript.

```csharp demo-below
public class JavaScriptLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
        var jsCode = UseState("console.log('hello world!');");
        return Layout.Vertical()
                    | Text.H3("JavaScript")
                    | Text.Html("<i>Enter JavaScript code below!</i>") 
                    | jsCode.ToCodeInput().Language(Languages.Javascript);
    }
}
```

### Languages.Python variant

The following code shows how to use this variant to make the syntax highlighting work
for Python.

```csharp demo-below
public class PythonLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
        var pyCode = UseState("print('hello world!')");
        return Layout.Vertical()
                    | Text.H3("Python")
                    | Text.Html("<i>Enter Python code below!</i>") 
                    | pyCode.ToCodeInput().Language(Languages.Python);
    }
}
```

### Languages.Sql variant

The following code shows how to use this variant to make the syntax highlighting work
for SQL.

```csharp demo-below
public class SqlLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
        var sqlCode = UseState("select * from employees;");
        return Layout.Vertical()
                    | Text.H3("Sql")
                    | Text.Html("<i>Enter SQL code below!</i>") 
                    | sqlCode.ToCodeInput().Language(Languages.Sql);
    }
}
```

### Languages.Html variant

The following code shows how to use this variant to make the syntax highlighting work
for HTML.

```csharp demo-below
public class HtmlLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
        var htmlCode = UseState("<html><body><h1>Hello World!</h1></body><html>");
        return Layout.Vertical()
                    | Text.H3("HTML")
                    | Text.Html("<i>Enter HTML code below!</i>") 
                    | htmlCode.ToCodeInput().Language(Languages.Html);
    }
}
```

### Languages.Json variant

The following code shows how to use this variant to make the syntax highlighting work
for JSON.

```csharp demo-tabs
public class JsonLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
        var jsonCode = UseState(@"{ ""name"" : ""Ivy"", ""version"" : ""1.0.0""}");
        return Layout.Vertical()
                    | Text.H3("JSON")
                    | Text.Html("<i>Enter JSON below!</i>") 
                    | jsonCode.ToCodeInput().Language(Languages.Json);
    }
}
```


### Languages.Css variant

The following code shows how to use this variant to make the syntax highlighting work
for CSS.

```csharp demo-tabs
public class CssLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
        var cssCode = UseState(@"<style>
                                p {
                                  text-align: center;
                                  color: red;
                                } 
                                </style>");
        return Layout.Vertical()
                    | Text.H3("CSS")
                    | Text.Html("<i>Enter CSS below!</i>") 
                    | cssCode.ToCodeInput().Language(Languages.Css);
    }
}
```

### Languages.Dbml variant

The following code shows how to use this variant to make the syntax highlighting work
for DBML.

```csharp demo-tabs
public class DbmlLanguageHighlightDemo : ViewBase 
{
    public override object? Build()
    {    
         var dbmlCode = UseState(
            @"Table users {
                    id integer [primary key]
                    username varchar
                    role varchar
                    created_at timestamp
                }
              Table posts {
                id integer [primary key]
                title varchar
                body text
                user_id integer
                created_at timestamp
            }");
         return Layout.Vertical()
                    | Text.H3("DBML")
                    | Text.Html("<i>Enter DBML below!</i>") 
                    | dbmlCode.ToCodeInput().Language(Languages.Dbml);
    }
}
```


## Styling 
There are several styles that can be applied to these code inputs. 

### Invalid 
To mark a `CodeInput` as invalid because of the content, this style should be used. 
The following code shows how to use it. 

```csharp demo-below
public class InvalidCodeDemo: ViewBase
{
    public override object? Build()
    {
        var jsCode = UseState("console.log('hello world!';");
        return  Layout.Grid().Columns(2) 
                | Text.H4("Incomplete JavaScript code") 
                | jsCode.ToCodeInput().Language(Languages.Javascript)
                .Invalid("Missing closing (')') brace!");
    }
}
```

### Disabled 
To mark a `CodeInput` as disabled because of the content or for any other reason, this style
should be used. 

```csharp demo-below
public class DisabledCodeDemo : ViewBase
{
    public override object? Build()
    {
        var disabledCode = UseState("print('hello world!')");
        return Layout.Grid().Columns(2)
            | Text.H4("Disabled Python code")
            | disabledCode.ToCodeInput().Disabled();
    }
}
```


## Event Handling

You can handle events such as changes in the `CodeInput`: 

```csharp demo-below
public class CodeInputWithButton : ViewBase 
{
    public override object? Build()
    {        
        var codeState = UseState("");
        
        return Layout.Vertical()
            | Text.Label("Enter Code:")
            | codeState.ToCodeInput()
                    .Width(200)
                    .Height(100)
                    .Placeholder("Enter your code here...")
            
            | new Button("Execute Code")
                .Disabled(string.IsNullOrWhiteSpace(codeState.Value))
            | Text.Small(string.IsNullOrWhiteSpace(codeState.Value) 
                ? "Enter code to enable the button" 
                : "Ready to execute!");
    }
}

```


<WidgetDocs Type="Ivy.CodeInput" ExtensionTypes="Ivy.CodeInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/CodeInput.cs"/>

## Examples

### DBML Editor

```csharp demo-below
public class DBMLEditorDemo : ViewBase
{
    public override object? Build()
    {
        var sampleDbml = @"Table users {
                            id integer [primary key]
                            username varchar
                            role varchar
                            created_at timestamp
                    }";
        var dbml = this.UseState(sampleDbml);
        return Layout.Horizontal().RemoveParentPadding().Height(Size.Screen())
                | dbml.ToCodeInput().Width(90).Height(Size.Full()).Language(Languages.Dbml)
                | new DbmlCanvas(dbml.Value).Width(Size.Grow());
   }
}

``` 