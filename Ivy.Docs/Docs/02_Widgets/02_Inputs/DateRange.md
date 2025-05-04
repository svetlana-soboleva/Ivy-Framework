# DateRangeInput

The DateRangeInput widget allows users to select a range of dates. It provides a calendar interface for both start and end date selection, making it ideal for filtering data by date ranges or scheduling events.

## Basic Usage

Here's a simple example of a DateRangeInput that allows users to select a date range:

```csharp
dateRangeState = this.UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));
new DateRangeInput<(DateTime, DateTime)>(dateRangeState)
```

```csharp
new DateRangeInput<(DateTime, DateTime)>(dateRangeState)
```

## Variants

The DateRangeInput can be disabled to prevent user interaction:

```csharp
new DateRangeInput<(DateTime, DateTime)>(dateRangeState).Disabled()
```

## Event Handling

DateRangeInput can handle changes in the selected date range using the `OnChange` event:

```csharp
var dateRangeState = this.UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));
var onChange = (Event<IInput<(DateTime, DateTime)>, (DateTime, DateTime)> e) =>
{
    // Handle the change event
};
new DateRangeInput<(DateTime, DateTime)>(dateRangeState, onChange)
```

## Styling

DateRangeInput can be customized with various styling options, such as setting a placeholder or format:

```csharp
new DateRangeInput<(DateTime, DateTime)>(dateRangeState)
    .Placeholder("Select a date range")
    .Format("MM/dd/yyyy")
```

<WidgetDocs Type="Ivy.DateRangeInput" ExtensionTypes="Ivy.DateRangeInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/DateRangeInput.cs"/>

## Examples

### Disabled State

```csharp
new DateRangeInput<(DateTime, DateTime)>(dateRangeState).Disabled()
``` 