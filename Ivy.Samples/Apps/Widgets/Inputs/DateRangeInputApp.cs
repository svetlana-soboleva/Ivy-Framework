using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.CalendarRange, path: ["Widgets", "Inputs"])]
public class DateRangeInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        // States for variants
        var dateRangeState = UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));
        var nullableDateRangeState = UseState<(DateTime?, DateTime?)>(() => (DateTime.Today.AddDays(-7), DateTime.Today));
        var invalidDateRangeState = UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));
        var disabledDateRangeState = UseState(() => (from: DateTime.Today.AddDays(-7), to: DateTime.Today));

        // States for data binding
        var dateOnlyRangeState = UseState(() => (from: DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), to: DateOnly.FromDateTime(DateTime.Today)));
        var timeOnlyRangeState = UseState(() => (from: TimeOnly.FromDateTime(DateTime.Now.AddHours(-2)), to: TimeOnly.FromDateTime(DateTime.Now)));
        var stringRangeState = UseState(() => (from: DateTime.Today.AddDays(-7).ToString("O"), to: DateTime.Today.ToString("O")));
        var nullableDateOnlyRangeState = UseState<(DateOnly?, DateOnly?)>(() => (DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today)));
        var nullableTimeOnlyRangeState = UseState<(TimeOnly?, TimeOnly?)>(() => (TimeOnly.FromDateTime(DateTime.Now.AddHours(-2)), TimeOnly.FromDateTime(DateTime.Now)));

        // Variants grid
        var variantsGrid = Layout.Grid().Columns(4)
            | null!
            | Text.InlineCode("Normal")
            | Text.InlineCode("Disabled")
            | Text.InlineCode("Invalid")

            | Text.InlineCode("DateTime")
            | dateRangeState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .TestId("daterange-input-datetime-main")
            | disabledDateRangeState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Disabled()
                .TestId("daterange-input-datetime-disabled-main")
            | invalidDateRangeState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .Invalid("Invalid date range")
                .TestId("daterange-input-datetime-invalid-main")

            | Text.InlineCode("Nullable")
            | nullableDateRangeState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .TestId("daterange-input-datetime-nullable-main")
            | nullableDateRangeState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Disabled()
                .TestId("daterange-input-datetime-nullable-disabled-main")
            | nullableDateRangeState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Invalid("Nullable invalid date range")
                .TestId("daterange-input-datetime-nullable-invalid-main");

        // Data binding grid
        var dataBindingGrid = Layout.Grid().Columns(3)
            | Text.InlineCode("Type")
            | Text.InlineCode("Input")
            | Text.InlineCode("Current Value")

            | Text.InlineCode("(DateTime, DateTime)")
            | dateRangeState.ToDateRangeInput().TestId("daterange-input-datetime-binding")
            | Text.InlineCode($"({dateRangeState.Value.from:yyyy-MM-dd}, {dateRangeState.Value.to:yyyy-MM-dd})")

            | Text.InlineCode("(DateOnly, DateOnly)")
            | dateOnlyRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-dateonly-binding")
            | Text.InlineCode($"({dateOnlyRangeState.Value.from:yyyy-MM-dd}, {dateOnlyRangeState.Value.to:yyyy-MM-dd})")

            | Text.InlineCode("(TimeOnly, TimeOnly)")
            | timeOnlyRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-timeonly-binding")
            | Text.InlineCode($"({timeOnlyRangeState.Value.from:HH:mm:ss}, {timeOnlyRangeState.Value.to:HH:mm:ss})")

            | Text.InlineCode("(string, string)")
            | stringRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-string-binding")
            | Text.InlineCode($"({stringRangeState.Value.from}, {stringRangeState.Value.to})")

            | Text.InlineCode("(DateTime?, DateTime?)")
            | nullableDateRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-datetime-nullable-binding")
            | Text.InlineCode($"({nullableDateRangeState.Value.Item1?.ToString("yyyy-MM-dd") ?? "null"}, {nullableDateRangeState.Value.Item2?.ToString("yyyy-MM-dd") ?? "null"})")

            | Text.InlineCode("(DateOnly?, DateOnly?)")
            | nullableDateOnlyRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-dateonly-nullable-binding")
            | Text.InlineCode($"({nullableDateOnlyRangeState.Value.Item1?.ToString("yyyy-MM-dd") ?? "null"}, {nullableDateOnlyRangeState.Value.Item2?.ToString("yyyy-MM-dd") ?? "null"})")

            | Text.InlineCode("(TimeOnly?, TimeOnly?)")
            | nullableTimeOnlyRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-timeonly-nullable-binding")
            | Text.InlineCode($"({nullableTimeOnlyRangeState.Value.Item1?.ToString("HH:mm:ss") ?? "null"}, {nullableTimeOnlyRangeState.Value.Item2?.ToString("HH:mm:ss") ?? "null"})");

        // Current values section
        var currentValues = Layout.Vertical()
            | Text.H3("Current Values")
            | Text.Block($"DateTime Range: {dateRangeState.Value.from:yyyy-MM-dd} to {dateRangeState.Value.to:yyyy-MM-dd}")
            | Text.Block($"Nullable DateTime Range: {nullableDateRangeState.Value.Item1?.ToString("yyyy-MM-dd") ?? "null"} to {nullableDateRangeState.Value.Item2?.ToString("yyyy-MM-dd") ?? "null"}")
            | Text.Block($"DateOnly Range: {dateOnlyRangeState.Value.from:yyyy-MM-dd} to {dateOnlyRangeState.Value.to:yyyy-MM-dd}")
            | Text.Block($"TimeOnly Range: {timeOnlyRangeState.Value.from:HH:mm:ss} to {timeOnlyRangeState.Value.to:HH:mm:ss}")
            | Text.Block($"String Range: {stringRangeState.Value.from} to {stringRangeState.Value.to}")
            | Text.Block($"Nullable DateOnly Range: {nullableDateOnlyRangeState.Value.Item1?.ToString("yyyy-MM-dd") ?? "null"} to {nullableDateOnlyRangeState.Value.Item2?.ToString("yyyy-MM-dd") ?? "null"}")
            | Text.Block($"Nullable TimeOnly Range: {nullableTimeOnlyRangeState.Value.Item1?.ToString("HH:mm:ss") ?? "null"} to {nullableTimeOnlyRangeState.Value.Item2?.ToString("HH:mm:ss") ?? "null"}");

        return Layout.Vertical()
            | Text.H1("DateRangeInput")
            | Text.H2("Variants")
            | variantsGrid
            | Text.H2("Data Binding")
            | dataBindingGrid
            | currentValues;
    }
}