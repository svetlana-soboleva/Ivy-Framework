# BoolInput

The `BoolInput` widget provides a checkbox, switch, or toggle for boolean (true/false) input values. It allows users to easily switch between two states in a form or configuration interface.

## Basic Usage

Here's a simple example of a `BoolInput` used as a checkbox:

```csharp
var state = this.UseState(false);
new BoolInput<bool>(state).Label("Accept Terms");
```

<WidgetDocs Type="Ivy.BoolInput" ExtensionsType="Ivy.BoolInputExtensions"/> 