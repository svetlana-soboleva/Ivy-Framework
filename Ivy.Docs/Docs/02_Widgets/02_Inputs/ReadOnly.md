# ReadOnlyInput

The ReadOnlyInput widget displays data in an input-like format that cannot be edited by the user. It's useful for showing form values in a consistent style with other inputs, while preventing modification.

## Basic Usage

Here's a simple example of a ReadOnlyInput displaying a value:

```csharp
double value = 123.45;
var readOnlyInput = new ReadOnlyInput<double>(value);
```

## Examples

### Displaying a Value

```csharp
double value = 123.45;
var readOnlyInput = new ReadOnlyInput<double>(value);
```

### Handling Events

```csharp
var state = UseState(123.45);
var readOnlyInput = new ReadOnlyInput<double>(state)
    .OnBlur(e => Console.WriteLine("Input blurred"));
```

### Using ShowCopyButton

```csharp
var readOnlyInput = new ReadOnlyInput<string>("ReadOnly Text")
    .ShowCopyButton(true);
```

<WidgetDocs Type="Ivy.ReadOnlyInput" ExtensionsType="Ivy.ReadOnlyInputExtensions"/> 