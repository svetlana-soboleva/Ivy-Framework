using System.Collections.Concurrent;
using System.Reactive.Disposables;

namespace Ivy.Core.Hooks;

public struct Unit {}

public interface ISignalSender<TInput, TOutput>
{
    public Task<TOutput[]> Send(TInput input);
}

public interface ISignalReceiver<out TInput, in TOutput>
{
    public IDisposable Receive(Func<TInput, TOutput> callback);
}

public abstract class AbstractSignal<TInput, TOutput>() : ISignalSender<TInput, TOutput>
{
    private readonly ConcurrentDictionary<Guid, Func<TInput, TOutput>> _subscribers = new();
    
    public async Task<TOutput[]> Send(TInput input)
    {
        var tasks = _subscribers.Values.Select(async callback => {
            try {
                return await Task.Run(() => callback(input));
            }
            catch (Exception) {
                return default;
            }
        });
        return (await Task.WhenAll(tasks))!;
    }
    
    public IDisposable Receive(Guid receiverId, Func<TInput, TOutput> callback)
    {
        _subscribers.TryRemove(receiverId, out _);
        _subscribers.TryAdd(receiverId, callback);
        return Disposable.Create(() =>
        {
            _subscribers.TryRemove(receiverId, out _);
        });
    }
}  

public class SignalReceiver<TInput, TOutput>(Guid receiverId, AbstractSignal<TInput, TOutput> signal) : ISignalReceiver<TInput, TOutput>
{
    public IDisposable Receive(Func<TInput, TOutput> callback)
    {
        return signal.Receive(receiverId, callback);
    }
}

public static class UseSignalExtensions
{
    public static ISignalSender<TInput,TOutput> CreateSignal<T,TInput,TOutput>(this IViewContext context) where T : AbstractSignal<TInput, TOutput>
    {
        return context.CreateContext(Activator.CreateInstance<T>);
    }
    
    public static ISignalReceiver<TInput,TOutput> UseSignal<T,TInput,TOutput>(this IViewContext view) where T : AbstractSignal<TInput, TOutput>
    {
        var receiverId = view.UseState(Guid.NewGuid, buildOnChange:false);
        var signal =  view.UseContext<T>();
        return new SignalReceiver<TInput, TOutput>(receiverId.Value, signal);
    }
}
