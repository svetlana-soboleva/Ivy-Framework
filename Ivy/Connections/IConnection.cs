using Microsoft.Extensions.DependencyInjection;

namespace Ivy.Connections;

public interface IConnection
{
    public string GetContext();
    public string GetName();
    public ConnectionEntity[] GetEntities();
    public void RegisterServices(IServiceCollection services);
}

public record ConnectionEntity(string Name);