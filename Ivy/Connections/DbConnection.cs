using Microsoft.Extensions.DependencyInjection;

namespace Ivy.Connections;

public abstract class DbConnection : IConnection
{
    public string GetContext()
    {
        throw new NotImplementedException();
    }

    public abstract void RegisterServices(IServiceCollection services);
}