# TextInput

The TextInput widget provides a standard text entry field. It supports various text input types including single-line text, multi-line text, password fields, and offers features like placeholder text, validation, and text formatting.

## Basic Usage

Here's a simple example of a text input with a placeholder:

```csharp
var withoutValue = UseState((string?)null);
new TextInput<string>(withoutValue, placeholder: "Enter text here...");
```

```csharp 
new TextInput<string>(withoutValue, placeholder: "Enter text here...")
```

## Variants

TextInputs come in several variants to suit different use cases:

```csharp
Layout.Horizontal()
    | new TextInput<string>(withoutValue, placeholder: "Text")
    | new TextInput<string>(withoutValue, placeholder: "Password").Variant(TextInputs.Password)
    | new TextInput<string>(withoutValue, placeholder: "Textarea").Variant(TextInputs.Textarea)
    | new TextInput<string>(withoutValue, placeholder: "Search").Variant(TextInputs.Search)
```

## Event Handling

TextInputs can handle change and blur events:

```csharp
var onChangedState = UseState("");
new TextInput<string>(onChangedState.Value, e => onChangedState.Set(e.Value));
```

## Styling

TextInputs can be customized with various styling options:

```csharp
new TextInput<string>(withoutValue, placeholder: "Styled Input")
    .Disabled()
    .Invalid("Invalid input")
```

<WidgetDocs Type="Ivy.TextInput" ExtensionTypes="Ivy.TextInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/TextInput.cs"/>

## Examples



### Form Usage

```csharp
var username = UseState("");
new TextInput<string>(username, placeholder: "Username")
``` 