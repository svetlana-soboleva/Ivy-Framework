# DateTimeInput

The DateTimeInput widget provides a comprehensive date and time picker interface with support for different variants. It allows users to select dates from a calendar, times from a time selector, or both date and time together, making it ideal for scheduling, event creation, and form inputs.

## Basic Usage

Here's a simple example of a DateTimeInput that allows users to select a date and time:

```csharp
dateState.ToDateTimeInput()
```

```csharp
dateState.ToDateTimeInput()
```

## Variants

DateTimeInput supports three variants to suit different use cases:

```csharp
Layout.Horizontal()
    | dateState.ToDateTimeInput().Variant(DateTimeInputs.Date)
    | dateState.ToDateTimeInput().Variant(DateTimeInputs.DateTime)
    | dateState.ToDateTimeInput().Variant(DateTimeInputs.Time)
```

### Date Variant

The Date variant provides a calendar picker for selecting dates only:

```csharp
dateState.ToDateTimeInput().Variant(DateTimeInputs.Date)
```

### DateTime Variant

The DateTime variant combines a calendar picker with a time input field:

```csharp
dateState.ToDateTimeInput().Variant(DateTimeInputs.DateTime)
```

### Time Variant

The Time variant provides a time picker for selecting time only:

```csharp
dateState.ToDateTimeInput().Variant(DateTimeInputs.Time)
```

## Supported State Types

DateTimeInput supports various date and time types:

- `DateTime` and `DateTime?`
- `DateTimeOffset` and `DateTimeOffset?`
- `DateOnly` and `DateOnly?`
- `TimeOnly` and `TimeOnly?`
- `string` (for ISO format)

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

### Different State Types

```csharp
var dateState = this.UseState(DateTime.Now);
var dateOnlyState = this.UseState(DateOnly.FromDateTime(DateTime.Now));
var timeOnlyState = this.UseState(TimeOnly.FromDateTime(DateTime.Now));
var nullableState = this.UseState<DateTime?>(DateTime.Now);

return Layout.Vertical(
    dateState.ToDateTimeInput().Variant(DateTimeInputs.DateTime),
    dateOnlyState.ToDateTimeInput().Variant(DateTimeInputs.Date),
    timeOnlyState.ToDateTimeInput().Variant(DateTimeInputs.Time),
    nullableState.ToDateTimeInput().Variant(DateTimeInputs.DateTime)
);
```

### Disabled DateTimeInput

```csharp
dateState.ToDateTimeInput().Disabled()
```

### Convenience Methods

```csharp
// For date-only input
dateState.ToDateInput()

// For time-only input
dateState.ToTimeInput()
```

<WidgetDocs Type="Ivy.DateTimeInput" ExtensionTypes="Ivy.DateTimeInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/DateTimeInput.cs"/> 