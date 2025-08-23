using Ivy.Core;
using Ivy.Shared;

namespace Ivy.Views.Dashboards;

/// <summary>
/// Represents the data structure for a dashboard metric containing the formatted value, trend information, and goal progress.
/// </summary>
/// <param name="MetricFormatted">The formatted string representation of the metric value (e.g., "$1,234.56", "98.5%").</param>
/// <param name="TrendComparedToPreviousPeriod">Optional percentage change compared to the previous period as a decimal (e.g., 0.15 for 15% increase, -0.05 for 5% decrease).</param>
/// <param name="GoalAchieved">Optional progress toward goal as a decimal from 0 to 1 (e.g., 0.75 for 75% of goal achieved).</param>
/// <param name="GoalFormatted">Optional formatted string representation of the goal target (e.g., "Target: $10,000").</param>
public record MetricRecord(
    string MetricFormatted,
    double? TrendComparedToPreviousPeriod,
    double? GoalAchieved,
    string? GoalFormatted);

/// <summary>
/// A dashboard view component that displays a key performance indicator (KPI) with asynchronous data loading, trend visualization, and goal progress tracking.
/// </summary>
/// <remarks>
/// Provides a standardized way to display business metrics in dashboard layouts with automatic loading states,
/// error handling, trend indicators, and progress visualization. The component fetches data asynchronously
/// and displays appropriate loading, error, or success states with consistent styling and layout.
/// </remarks>
public class MetricView(
    string title,
    Icons? icon,
    Func<Task<MetricRecord>> metricData
) : ViewBase
{
    /// <summary>
    /// The fixed height in units for all metric view cards to ensure consistent dashboard layout.
    /// </summary>
    private const int Height = 50;

    /// <summary>
    /// Builds the metric view component with asynchronous data loading, error handling, and responsive state management.
    /// </summary>
    /// <returns>
    /// A Card widget containing the metric display with one of the following states:
    /// - Loading: Skeleton placeholder while data is being fetched
    /// - Error: Error teaser view if data loading fails
    /// - Success: Formatted metric with optional trend indicators and goal progress
    /// </returns>
    public override object? Build()
    {
        var data = UseState<MetricRecord?>(() => null);
        var failed = UseState<Exception?>(() => null);

        UseEffect(async () =>
        {
            try
            {
                data.Set(await metricData());
            }
            catch (Exception ex)
            {
                failed.Set(ex);
            }
        }, []);

        if (failed.Value is not null)
        {
            return new Card().Title(title).Icon(icon).Height(Size.Units(Height)) | new ErrorTeaserView(failed.Value);
        }

        if (data.Value is null)
        {
            return new Card(
                new Skeleton()
            ).Title(title).Icon(icon).Height(Size.Units(Height));
        }

        var x = data.Value;

        return new Card(
                Layout.Horizontal().Align(Align.Left).Gap(2)
                    | Text.H3(x.MetricFormatted).NoWrap().Overflow(Overflow.Clip)
                    | (x.TrendComparedToPreviousPeriod != null
                        ? x.TrendComparedToPreviousPeriod >= 0
                            ? Icons.TrendingUp.ToIcon().Color(Colors.Primary)
                            : Icons.TrendingDown.ToIcon().Color(Colors.Red)
                        : null)
                    | (x.TrendComparedToPreviousPeriod != null
                        ? x.TrendComparedToPreviousPeriod >= 0
                            ? Text.Small(x.TrendComparedToPreviousPeriod.Value.ToString("P1")).Color(Colors.Primary)
                            : Text.Small(x.TrendComparedToPreviousPeriod.Value.ToString("P1")).Color(Colors.Red)
                        : null),
                x.GoalAchieved != null ? new Progress((int)Math.Round(x.GoalAchieved.Value * 100.0)).ColorVariant(Progress.ColorVariants.EmeraldGradient).Goal(x.GoalFormatted) : null
            ).Title(title).Icon(icon).Height(Size.Units(Height))
            ;
    }
}
