# DateTimeInput

The DateTimeInput widget provides a date and time picker interface. It allows users to select both a date from a calendar and a time from a time selector, useful for scheduling and event creation forms.

## Basic Usage

Here's a simple example of a DateTimeInput that allows users to select a date and time:

```csharp
dateState.ToDateTimeInput()
```

```csharp 
dateState.ToDateTimeInput()
```

## Variants

DateTimeInput supports several variants to suit different use cases:

```csharp 
Layout.Horizontal()
    | dateState.ToDateTimeInput().Variant(DateTimeInputs.Date)
    | dateState.ToDateTimeInput().Variant(DateTimeInputs.DateTime)
    | dateState.ToDateTimeInput().Variant(DateTimeInputs.Time)
```

## Event Handling

DateTimeInput can handle change and blur events:

```csharp
var dateState = this.UseState(DateTime.Now);
var onChangeHandler = (Event<IInput<DateTime>, DateTime> e) =>
{
    dateState.Set(e.Value);
};
return dateState.ToDateTimeInput().OnChange(onChangeHandler);
```

## Styling

DateTimeInput can be customized with various styling options:

```csharp
dateState.ToDateTimeInput()
    .Placeholder("Select a date")
    .Format("MM/dd/yyyy")
```

## Examples

### Disabled DateTimeInput

```csharp
dateState.ToDateTimeInput().Disabled()
```

<WidgetDocs Type="Ivy.DateTimeInput" ExtensionsType="Ivy.DateTimeInputExtensions"/> 