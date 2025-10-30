using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>Kanban card widget representing an individual item within a kanban column.</summary>
public record KanbanCard : WidgetBase<KanbanCard>
{
    /// <summary>Initializes KanbanCard with specified content.</summary>
    /// <param name="content">Content to display within kanban card.</param>
    public KanbanCard(object? content) : base(content != null ? [content] : [])
    {
    }

    /// <summary>Optional ID for the kanban card.</summary>
    [Prop] public object? CardId { get; set; }

    /// <summary>Optional priority for the kanban card.</summary>
    [Prop] public object? Priority { get; set; }

    /// <summary>Event handler called when the card is clicked.</summary>
    [Event] public Func<Event<KanbanCard, object?>, ValueTask>? OnClick { get; set; }
}
