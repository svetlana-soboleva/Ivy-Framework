# DateRangeInput

The DateRangeInput widget allows users to select a range of dates. It provides a calendar interface for both start and end date selection, making it ideal for filtering data by date ranges or scheduling events.

## Basic Usage

Here's a simple example of a DateRangeInput that allows users to select a date range:

```csharp
var dateRangeState = this.UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));
dateRangeState.ToDateRangeInput()
```

## Supported Types

DateRangeInput supports various datetime tuple types:

- `(DateTime, DateTime)` - Standard datetime range
- `(DateTime?, DateTime?)` - Nullable datetime range
- `(DateOnly, DateOnly)` - Date-only range
- `(DateOnly?, DateOnly?)` - Nullable date-only range
- `(TimeOnly, TimeOnly)` - Time-only range
- `(TimeOnly?, TimeOnly?)` - Nullable time-only range
- `(DateTimeOffset, DateTimeOffset)` - DateTimeOffset range
- `(DateTimeOffset?, DateTimeOffset?)` - Nullable DateTimeOffset range

## Variants

The DateRangeInput can be customized with various states:

### Disabled State

```csharp
dateRangeState.ToDateRangeInput().Disabled()
```

### Invalid State

```csharp
dateRangeState.ToDateRangeInput().Invalid("Invalid date range")
```

### Nullable State

```csharp
var nullableRange = this.UseState<(DateTime?, DateTime?)>(() => (DateTime.Today.AddDays(-7), DateTime.Today));
nullableRange.ToDateRangeInput()
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

DateRangeInput can be customized with various styling options:

### Placeholder

```csharp
dateRangeState.ToDateRangeInput().Placeholder("Select a date range")
```

### Format

```csharp
dateRangeState.ToDateRangeInput().Format("yyyy-MM-dd")
```

### Combined Styling

```csharp
dateRangeState.ToDateRangeInput()
    .Placeholder("Select a date range")
    .Format("MM/dd/yyyy")
    .Invalid("Please select a valid date range")
```

## Data Binding Examples

### DateTime Range

```csharp
var dateTimeRange = this.UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));
dateTimeRange.ToDateRangeInput()
```

### DateOnly Range

```csharp
var dateOnlyRange = this.UseState(() => (from: DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), to: DateOnly.FromDateTime(DateTime.Today)));
dateOnlyRange.ToDateRangeInput()
```

### TimeOnly Range

```csharp
var timeOnlyRange = this.UseState(() => (from: TimeOnly.FromDateTime(DateTime.Now.AddHours(-2)), to: TimeOnly.FromDateTime(DateTime.Now)));
timeOnlyRange.ToDateRangeInput()
```

### Nullable Ranges

```csharp
var nullableDateTimeRange = this.UseState<(DateTime?, DateTime?)>(() => (DateTime.Today.AddDays(-7), DateTime.Today));
nullableDateTimeRange.ToDateRangeInput()

var nullableDateOnlyRange = this.UseState<(DateOnly?, DateOnly?)>(() => (DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today)));
nullableDateOnlyRange.ToDateRangeInput()
```

<WidgetDocs Type="Ivy.DateRangeInput" ExtensionTypes="Ivy.DateRangeInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/DateRangeInput.cs"/>

## Examples

### Complete Example with All Features

```csharp
var dateRangeState = this.UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));
var nullableRangeState = this.UseState<(DateTime?, DateTime?)>(() => (DateTime.Today.AddDays(-7), DateTime.Today));

return Layout.Vertical(
    dateRangeState.ToDateRangeInput().Placeholder("Select date range"),
    dateRangeState.ToDateRangeInput().Disabled(),
    dateRangeState.ToDateRangeInput().Invalid("Invalid range"),
    nullableRangeState.ToDateRangeInput().Placeholder("Nullable range")
);
``` 