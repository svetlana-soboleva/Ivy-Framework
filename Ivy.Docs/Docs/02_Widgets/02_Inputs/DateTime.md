# DateTimeInput

The `DateTimeInput` widget provides a comprehensive date and time picker interface with support for different variants. It allows users to select dates from a calendar, times from a time selector, or both date and time together, making it ideal for scheduling, event creation, and form inputs.

## Basic Usage

Here's a simple example of a DateTimeInput that allows users to select a date and time:

```csharp demo-below
public class BasicDateTimeUsageDemo : ViewBase
{
    public override object? Build()
    {
        var dateState = UseState(DateTime.Today);
        var daysBetween = dateState.Value.Subtract(DateTime.Today).Days;
        return Layout.Vertical() 
                | Text.Large("When is your birthday?")
                | dateState.ToDateTimeInput()
                           .Variant(DateTimeInputs.Date)
                | Text.Html($"<i>That's <b>{daysBetween}</b> days from now!");
    }
}    
```


## Variants

`DateTimeInput` supports three variants to suit different use cases:

### Date Variant

The Date variant provides a calendar picker for selecting dates only.

```csharp
dateState.ToDateTimeInput().Variant(DateTimeInputs.Date)
```
Instead of using `DateTimeInputs.Date` the function `ToDateInput` should generally be used.

### DateTime Variant

The DateTime variant combines a calendar picker with a time input field.

```csharp
dateState.ToDateTimeInput().Variant(DateTimeInputs.DateTime)
```
Instead of using `DateTimeInputs.DateTime` the function `ToDateTimeInput` should generally be used.

### Time Variant

The Time variant provides a time picker for selecting time only.

```csharp
dateState.ToDateTimeInput().Variant(DateTimeInputs.Time)
```
Instead of using `DateTimeInputs.Time` the function `ToTimeInput` should generally be used.  

The following demo shows all of these in action. 

```csharp demo-below
public class DateTimeVariantsDemo : ViewBase
{    
    public override object? Build()
    {    
        var dateState = UseState(DateTime.Today.Date);
        var timeState = UseState(DateTime.Now);
        var dateTimeState = UseState(DateTime.Today);
        
        return Layout.Vertical()
                | (Layout.Horizontal()
                    | Text.Small("Date")
                          .Width(35)
                    | dateState.ToDateInput()
                           .Format("dd/MM/yyyy"))
                | (Layout.Horizontal()
                    | Text.Small("DateTime")
                          .Width(35)
                    | dateTimeState.ToDateTimeInput()
                           .Format("dd/MM/yyyy HH:mm:ss"))
                | (Layout.Horizontal()
                    | Text.Small("Time")
                          .Width(35)
                    | timeState.ToTimeInput());
    }    
}                
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

## Format

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