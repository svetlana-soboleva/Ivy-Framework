using Microsoft.Extensions.DependencyInjection;

namespace Ivy.Connections;

public interface IConnection
{
    public string GetContext();
    public string GetName();
    public void RegisterServices(IServiceCollection services);
}