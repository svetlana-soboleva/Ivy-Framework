using Microsoft.EntityFrameworkCore;

namespace Ivy.Database.Generator.Toolkit.Databases;

public class PostgresDatabaseProvider() : IDatabaseProvider
{
    public T GetDbContext<T>(string connectionString) where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseNpgsql(connectionString)
            .Options;
        return (T)Activator.CreateInstance(typeof(T), options)!;
    }
    
    public string? GetDefaultConnectionString(string? projectDirectory)
    {
        return null;
    }
}