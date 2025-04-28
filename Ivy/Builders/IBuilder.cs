namespace Ivy.Builders;

public interface IBuilder<in TModel>
{
    public object? Build(object? value, TModel record);
}