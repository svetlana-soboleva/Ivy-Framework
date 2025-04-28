using Ivy.Core;
using Ivy.Core.Hooks;

namespace Ivy.Alerts;

public delegate void ShowAlertDelegate(string message, string? title = null, AlertButtonSet buttonSet = AlertButtonSet.OkCancel);

public static class UseAlertExtensions
{
    public static (IView? alertView, AlertResult alertResult, ShowAlertDelegate showAlert) UseAlert<TW>(this TW view) where TW : ViewBase =>
        view.Context.UseAlert();
    
    public static (IView? alertView, AlertResult alertResult, ShowAlertDelegate showAlert) UseAlert(this IViewContext context)
    {
        var alertResult = context.UseState(AlertResult.Undecided);
        var isOpen = context.UseState(false);
        var alertOptions = context.UseState((AlertOptions)null!);

        var view = isOpen.Value ? new AlertView(alertResult, isOpen, alertOptions.Value) : null;
         
        var showAlert = new ShowAlertDelegate((message, title, buttonSet) =>
        {
            alertOptions.Set(new AlertOptions(message, title, buttonSet));
            alertResult.Set(AlertResult.Undecided);
            isOpen.Set(true);
        });
        
        return (view, alertResult.Value, showAlert);
    }
}