using Ivy.Core;
using Ivy.Core.Hooks;

namespace Ivy.Views;

/// <summary>
/// Represents an error view that displays comprehensive error information
/// including the exception type, message, and stack trace.
/// </summary>
public class ErrorView(System.Exception e) : ViewBase, IStateless
{
    /// <summary>
    /// Builds the error view layout, displaying comprehensive error information
    /// including the exception type name, message, and stack trace for debugging.
    /// </summary>
    /// <returns>An Error widget containing the exception type, message, and stack trace.</returns>
    public override object? Build()
    {
        if (e is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
        {
            e = aggregateException.InnerExceptions[0];
        }

        return new Error(e.GetType().Name, e.Message, e.StackTrace);
    }
}