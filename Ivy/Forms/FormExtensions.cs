using Ivy.Core.Hooks;

namespace Ivy.Forms;

public static class FormExtensions
{
    public static FormBuilder<T> ToForm<T>(this IState<T> obj)
    {
        return new FormBuilder<T>(obj);
    }
}