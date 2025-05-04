---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# CodeInput

The CodeInput widget provides a specialized text input field for entering and editing code with syntax highlighting. It supports various programming languages and offers features like line numbers and code formatting.

## Basic Usage

Here's a simple example of a CodeInput widget:

```csharp
var code = """
function helloWorld() {
  console.log('Hello, world!');
}
""";

var codeState = UseState(code);
return codeState.ToCodeInput().Language(Languages.Javascript);
```

```csharp 
var codeState = UseState(code);
return codeState.ToCodeInput().Language(Languages.Javascript);
```

## Variants

The CodeInput widget can be customized with different languages for syntax highlighting:

```csharp
Layout.Horizontal()
    | codeState.ToCodeInput().Language(Languages.Javascript)
    | codeState.ToCodeInput().Language(Languages.Python)
    | codeState.ToCodeInput().Language(Languages.Sql)
```

## Event Handling

You can handle events such as changes in the CodeInput:

```csharp
var codeState = UseState(code);
var onChange = (Event<IInput<string>, string> e) => {
    Console.WriteLine("Code changed: " + e.Value);
};
return codeState.ToCodeInput().OnChange(onChange);
```

## Styling

The CodeInput widget can be styled with various options:

```csharp
codeState.ToCodeInput()
    .Width(Size.Full())
    .Height(Size.Units(30))
    .Placeholder("Enter your code here...")
```

<WidgetDocs Type="Ivy.CodeInput" ExtensionTypes="Ivy.CodeInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/CodeInput.cs"/>

## Examples

### DBML Editor

```csharp
var dbml = this.UseState(sampleDbml);
return Layout.Horizontal().RemoveParentPadding().Height(Size.Screen())
       | dbml.ToCodeInput().Width(90).Height(Size.Full()).Language(Languages.Dbml)
       | new DbmlCanvas(dbml.Value).Width(Size.Grow());
``` 