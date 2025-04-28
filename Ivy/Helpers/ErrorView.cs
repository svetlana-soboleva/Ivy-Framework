using Ivy.Client;
using Ivy.Core;
using Ivy.Core.Hooks;

namespace Ivy.Helpers;

public class ErrorView(System.Exception e) : ViewBase, IStateless
{
    public override object? Build()
    {
        return new Ivy.Error(e.GetType().Name, e.Message, e.StackTrace);
    }
}