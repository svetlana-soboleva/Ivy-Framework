using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

public record Box : WidgetBase<Box>
{
    public Box(object? content = null!) : base(content != null ? [content] : [])
    {
    }

    [Prop] public Colors? Color { get; set; } = Colors.Green;

    [Prop] public int? BorderThickness { get; set; } = 2;

    [Prop] public BorderRadius BorderRadius { get; set; } = BorderRadius.Rounded;
    
    [Prop] public BorderStyle BorderStyle { get; set; } = BorderStyle.Solid;
    
    [Prop] public int Padding { get; set; } = 2;
    
    [Prop] public Align? Align { get; set; } = Shared.Align.Center;
}

public static class BoxExtensions
{
    public static Box Color(this Box box, Colors color)
    {
        return box with { Color = color };
    }

    public static Box BorderThickness(this Box box, int thickness)
    {
        return box with { BorderThickness = thickness };
    }

    public static Box BorderRadius(this Box box, BorderRadius radius)
    {
        return box with { BorderRadius = radius };
    }

    public static Box BorderStyle(this Box box, BorderStyle style)
    {
        return box with { BorderStyle = style };
    }
    
    public static Box Padding(this Box box, int padding)
    {
        return box with { Padding = padding };
    }
    
    public static Box Content(this Box box, object? content)
    {
        return box with { Children = content != null ? [content] : [] };
    }
    
    public static Box Align(this Box box, Align? align)
    {
        return box with { Align = align };
    }
}