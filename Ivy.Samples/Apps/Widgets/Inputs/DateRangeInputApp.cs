using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.CalendarRange, path: ["Widgets", "Inputs"])]
public class DateRangeInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        // States for variants
        var dateOnlyState = UseState(() => (from: DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), to: DateOnly.FromDateTime(DateTime.Today)));
        var nullableDateOnlyState = UseState<(DateOnly?, DateOnly?)>(() => (DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today)));
        var invalidDateOnlyState = UseState(() => (from: DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), to: DateOnly.FromDateTime(DateTime.Today)));
        var invalidNullableDateOnlyState = UseState<(DateOnly?, DateOnly?)>(() => (DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today)));
        var disabledDateOnlyState = UseState(() => (from: DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), to: DateOnly.FromDateTime(DateTime.Today)));
        var disabledNullableDateOnlyState = UseState<(DateOnly?, DateOnly?)>(() => (DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today)));

        // States for data binding
        var dateOnlyRangeState = UseState(() => (from: DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), to: DateOnly.FromDateTime(DateTime.Today)));
        var nullableDateOnlyRangeState = UseState<(DateOnly?, DateOnly?)>(() => (DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today)));

        // Variants grid
        var variantsGrid = Layout.Grid().Columns(5)
            | null!
            | Text.InlineCode("Type")
            | Text.InlineCode("Normal")
            | Text.InlineCode("Disabled")
            | Text.InlineCode("Invalid")
            | Text.InlineCode("Invalid + Nullable")

            | Text.InlineCode("(DateOnly, DateOnly)")
            | dateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .TestId("daterange-input-dateonly-main")
            | disabledDateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Disabled()
                .TestId("daterange-input-dateonly-disabled-main")
            | invalidDateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .Invalid("Invalid date range")
                .TestId("daterange-input-dateonly-invalid-main")
            | invalidDateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .Invalid("Invalid date range")
                .TestId("daterange-input-dateonly-invalid-nullable-main")

            | Text.InlineCode("(DateOnly?, DateOnly?)")
            | nullableDateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .TestId("daterange-input-dateonly-nullable-main")
            | disabledNullableDateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Disabled()
                .Format("yyyy-MM-dd")
                .TestId("daterange-input-dateonly-nullable-disabled-main")
            | invalidNullableDateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .Invalid("Invalid date range")
                .TestId("daterange-input-dateonly-nullable-invalid-main")
            | invalidNullableDateOnlyState
                .ToDateRangeInput()
                .Placeholder("Select date range")
                .Format("yyyy-MM-dd")
                .Invalid("Invalid date range")
                .TestId("daterange-input-dateonly-nullable-invalid-nullable-main");

        // Data binding grid
        var dataBindingGrid = Layout.Grid().Columns(3)
            | Text.InlineCode("Type")
            | Text.InlineCode("Input")
            | Text.InlineCode("Current Value")

            | Text.InlineCode("(DateOnly, DateOnly)")
            | dateOnlyRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-dateonly-binding")
            | Text.InlineCode($"({dateOnlyRangeState.Value.from:yyyy-MM-dd}, {dateOnlyRangeState.Value.to:yyyy-MM-dd})")

            | Text.InlineCode("(DateOnly?, DateOnly?)")
            | nullableDateOnlyRangeState
                .ToDateRangeInput()
                .TestId("daterange-input-dateonly-nullable-binding")
            | Text.InlineCode($"({nullableDateOnlyRangeState.Value.Item1?.ToString("yyyy-MM-dd") ?? "null"}, {nullableDateOnlyRangeState.Value.Item2?.ToString("yyyy-MM-dd") ?? "null"})");

        // Current values section
        var currentValues = Layout.Vertical()
            | Text.H3("Current Values")
            | Text.Block($"DateOnly Range: {dateOnlyRangeState.Value.from:yyyy-MM-dd} to {dateOnlyRangeState.Value.to:yyyy-MM-dd}")
            | Text.Block($"Nullable DateOnly Range: {nullableDateOnlyRangeState.Value.Item1?.ToString("yyyy-MM-dd") ?? "null"} to {nullableDateOnlyRangeState.Value.Item2?.ToString("yyyy-MM-dd") ?? "null"}");

        return Layout.Vertical()
            | Text.H1("DateRangeInput")
            | Text.H2("Variants")
            | variantsGrid
            | Text.H2("Data Binding")
            | dataBindingGrid
            | currentValues;
    }
}