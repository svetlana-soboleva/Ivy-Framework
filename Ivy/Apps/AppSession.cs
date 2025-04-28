using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Services;

namespace Ivy.Apps;

public class AppSession : IDisposable
{
    private readonly Disposables _disposables = new();
    
    public void TrackDisposable(IDisposable disposable)
    {
        _disposables.Add(disposable);
    }
    
    public required string ConnectionId { get; set; }
    
    public required string AppId { get; set; }
    
    public required IWidgetTree WidgetTree { get; set; }
    
    public required AppDescriptor AppDescriptor { get; set; }
    
    public required ViewBase App { get; set; }
    
    public required IContentBuilder ContentBuilder { get; set; }
    
    public required IServiceProvider AppServices { get; set; }
    
    public void Dispose()
    {
        _disposables.Dispose();
        WidgetTree.Dispose();
    }
}