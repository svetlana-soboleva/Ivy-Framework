using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

public record Article : WidgetBase<Article>
{
    public Article(params IEnumerable<object> content) : base(content)
    {
    }

    [Prop] public bool ShowToc { get; set; } = true;
    
    [Prop] public bool ShowFooter { get; set; } = true;
    
    [Prop] public InternalLink? Previous { get; set; }
    
    [Prop] public InternalLink? Next { get; set; }
    
    [Prop] public string? DocumentSource { get; set; }
}

public static class ArticleExtensions
{
    public static Article ShowToc(this Article article, bool showToc = true) => article with { ShowToc = showToc };
    public static Article ShowFooter(this Article article, bool showFooter = true) => article with { ShowFooter = showFooter };
    public static Article Previous(this Article article, InternalLink? navigateBack) => article with { Previous = navigateBack };
    public static Article Next(this Article article, InternalLink? navigateForward) => article with { Next = navigateForward };
    public static Article DocumentSource(this Article article, string? documentSource) => article with { DocumentSource = documentSource };
}