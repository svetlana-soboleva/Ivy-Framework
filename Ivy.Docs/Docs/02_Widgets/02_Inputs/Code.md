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

The CodeInput widget can be customized with different languages for syntax highlighting:

```csharp demo-tabs
public class LanguageHighlightDemo : ViewBase
{
    public override object? Build()
    {    
        var jsCode = UseState("console.log('hello world!');");
        var csCode = UseState("Console.WriteLine(\"Hello World!\");");
        var pyCode = UseState("print('hello world!')");
        var sqlCode = UseState("select * from employees;");
        var htmlCode = UseState("<h1> Hello World! </h1>");
        var cssCode = UseState(@"<style>
                                p {
                                  text-align: center;
                                  color: red;
                                } 
                                </style>");
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
        var jsonCode = UseState(@"{ name : ""Ivy"", version : ""1.0.0""}");
        return Layout.Vertical()
            | Layout.Grid().Columns(2) 
                | Layout.Vertical()
                    | Text.H3("JavaScript")
                    | Text.Small("Enter JavaScript code below!") 
                | jsCode.ToCodeInput().Language(Languages.Javascript)
            | Layout.Grid().Columns(2)
                | Layout.Vertical()
                    | Text.H3("C#")
                    | Text.Small("Enter C# code below!")
                | csCode.ToCodeInput().Language(Languages.Csharp)
            | Layout.Grid().Columns(2)
                | Layout.Vertical()
                    | Text.H3("Python")
                    | Text.Small("Enter Python code below!")
                | pyCode.ToCodeInput().Language(Languages.Python)
            | Layout.Grid().Columns(2)
                | Layout.Vertical()
                    | Text.H3("SQL")
                    | Text.Small("Enter SQL code below!")
                | sqlCode.ToCodeInput().Language(Languages.Sql)
            | Layout.Grid().Columns(2)
                | Layout.Vertical()
                    | Text.H3("CSS")
                    | Text.Small("Enter CSS code below!")
                | cssCode.ToCodeInput().Language(Languages.Css)
            | Layout.Grid().Columns(2)
                | Layout.Vertical()
                    | Text.H3("HTML")
                    | Text.Small("Enter HTML code below!")
                | htmlCode.ToCodeInput().Language(Languages.Html)
            | Layout.Grid().Columns(2)
                | Layout.Vertical()
                    | Text.H3("JSON")
                    | Text.Small("Enter JSON code below!")
                | jsonCode.ToCodeInput().Language(Languages.Json)
            | Layout.Grid().Columns(2)
                | Layout.Vertical()
                    | Text.H3("DBML")
                    | Text.Small("Enter DBML code below!")
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