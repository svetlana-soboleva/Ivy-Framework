using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

public record Avatar : WidgetBase<Avatar>
{
    public Avatar(string fallback, string? image = null)
    {
        Fallback = fallback;
        Image = image;
    }
    
    [Prop] public string Fallback { get; set; }
    [Prop] public string? Image { get; set; }
}