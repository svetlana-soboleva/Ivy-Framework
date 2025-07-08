using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.Calendar, path: ["Widgets", "Inputs"])]
public class DateTimeInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        // States for variants
        var dateState = UseState(DateTime.Now);
        var dateTimeState = UseState(DateTime.Now);
        var timeState = UseState(TimeOnly.FromDateTime(DateTime.Now));
        var nullableDateState = UseState<DateTime?>(() => null);
        var invalidDateState = UseState(DateTime.Now);
        var disabledDateState = UseState(DateTime.Now);

        // States for data binding
        var dateOnlyState = UseState(DateOnly.FromDateTime(DateTime.Now));
        var timeOnlyState = UseState(TimeOnly.FromDateTime(DateTime.Now));
        var stringState = UseState(DateTime.Now.ToString("O"));
        var nullableTimeState = UseState<TimeOnly?>(() => null);
        var nullableDateOnlyState = UseState<DateOnly?>(() => null);

        // Variants grid
        var variantsGrid = Layout.Grid().Columns(5)
            | null!
            | Text.Block("Normal")
            | Text.Block("Disabled")
            | Text.Block("Invalid")
            | Text.Block("Nullable")

            | Text.InlineCode("Date")
            | dateState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Date)
                .Placeholder("Pick a date")
                .Format("yyyy-MM-dd")
            | disabledDateState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Date)
                .Placeholder("Pick a date")
                .Disabled()
            | invalidDateState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Date)
                .Placeholder("Pick a date")
                .Format("yyyy-MM-dd")
                .Invalid("Invalid date")
            | nullableDateState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Date)
                .Placeholder("Pick a date")

            | Text.InlineCode("DateTime")
            | dateTimeState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.DateTime)
                .Placeholder("Pick date & time")
            | disabledDateState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.DateTime)
                .Placeholder("Pick date & time")
                .Disabled()
            | invalidDateState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.DateTime)
                .Placeholder("Pick date & time")
                .Invalid("Invalid datetime")
            | nullableDateState.ToDateTimeInput()
                .Variant(DateTimeInputs.DateTime)
                .Placeholder("Pick date & time")

            | Text.InlineCode("Time")
            | timeState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Time)
                .Placeholder("Pick a time")
            | timeState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Time)
                .Placeholder("Pick a time")
                .Disabled()
            | timeState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Time)
                .Placeholder("Pick a time")
                .Invalid("Invalid time")
            | nullableTimeState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Time)
                .Placeholder("Pick a time")
            ;

        // Data binding grid
        var dataBindingGrid = Layout.Grid().Columns(4)
            | Text.Block("Type")
            | Text.Block("Input")
            | Text.Block("Current Value")
            | Text.Block("Nullable?")

            | Text.InlineCode("DateTime")
            | dateState.ToDateTimeInput().Variant(DateTimeInputs.DateTime)
            | Text.InlineCode($"{dateState.Value:yyyy-MM-dd HH:mm:ss}")
            | Text.Block("No")

            | Text.InlineCode("DateOnly")
            | dateOnlyState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Date)
            | Text.InlineCode($"{dateOnlyState.Value:yyyy-MM-dd}")
            | Text.Block("No")

            | Text.InlineCode("TimeOnly")
            | timeOnlyState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Time)
            | Text.InlineCode($"{timeOnlyState.Value:HH:mm:ss}")
            | Text.Block("No")

            | Text.InlineCode("string (ISO)")
            | stringState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.DateTime)
            | Text.InlineCode(stringState.Value)
            | Text.Block("No")

            | Text.InlineCode("DateTime?")
            | nullableDateState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.DateTime)
            | Text.InlineCode(nullableDateState.Value?.ToString("yyyy-MM-dd HH:mm:ss") ?? "null")
            | Text.Block("Yes")

            | Text.InlineCode("DateOnly?")
            | nullableDateOnlyState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Date)
            | Text.InlineCode(nullableDateOnlyState.Value?.ToString("yyyy-MM-dd") ?? "null")
            | Text.Block("Yes")
            
            | Text.InlineCode("TimeOnly?")
            | nullableTimeState
                .ToDateTimeInput()
                .Variant(DateTimeInputs.Time)
            | Text.InlineCode(nullableTimeState.Value?.ToString("HH:mm:ss") ?? "null")
            | Text.Block("Yes")
            ;

        // Current values section
        var currentValues = Layout.Vertical()
            | Text.H3("Current Values")
            | Text.Block($"Date: {dateState.Value:yyyy-MM-dd}")
            | Text.Block($"DateTime: {dateTimeState.Value:yyyy-MM-dd HH:mm:ss}")
            | Text.Block($"Time: {timeState.Value:HH:mm:ss}")
            | Text.Block($"Nullable DateTime: {nullableDateState.Value?.ToString("yyyy-MM-dd HH:mm:ss") ?? "null"}")
            | Text.Block($"DateOnly: {dateOnlyState.Value:yyyy-MM-dd}")
            | Text.Block($"TimeOnly: {timeOnlyState.Value:HH:mm:ss}")
            | Text.Block($"string: {stringState.Value}")
            | Text.Block($"Nullable DateOnly: {nullableDateOnlyState.Value?.ToString("yyyy-MM-dd") ?? "null"}")
            | Text.Block($"Nullable TimeOnly: {nullableTimeState.Value?.ToString("HH:mm:ss") ?? "null"}");

        return Layout.Vertical()
            | Text.H1("DateTimeInput")
            | Text.H2("Variants")
            | variantsGrid
            | Text.H2("Data Binding")
            | dataBindingGrid
            | currentValues;
    }
}