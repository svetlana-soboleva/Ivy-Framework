---
title: PostgreSQL
---

# PostgreSQL Database Provider

<Ingress>
Connect your Ivy application to PostgreSQL with automatic Entity Framework configuration.
</Ingress>

## Overview

PostgreSQL is an advanced open-source relational database known for its reliability, feature robustness, and performance. Ivy provides seamless integration with PostgreSQL through Entity Framework Core.

## Connection String Format

```text
Host=localhost;Database=mydb;Username=user;Password=pass
```

### Authentication Options

**Standard Authentication**
```text
Host=localhost;Database=mydb;Username=myuser;Password=mypassword;Port=5432
```

**SSL Connection**
```text
Host=localhost;Database=mydb;Username=user;Password=pass;SSL Mode=Require
```

**Connection Pooling**
```text
Host=localhost;Database=mydb;Username=user;Password=pass;Pooling=true;MinPoolSize=1;MaxPoolSize=20
```

## Configuration

Ivy automatically configures the **Npgsql.EntityFrameworkCore.PostgreSQL** package for PostgreSQL connections.

## Advanced Configuration

### Custom Schema

```terminal
>ivy db add --provider Postgres --name MyPostgres --schema MyCustomSchema
```

### Multiple Schemas

PostgreSQL supports multiple schemas. You can specify different schemas when adding connections or configure them in the DbContext.

## PostgreSQL-Specific Features

PostgreSQL offers advanced features that Ivy can leverage:
- **JSONB columns** for document storage
- **Array types** for storing collections
- **Custom data types** and enums
- **Full-text search** capabilities

## Security Best Practices

- **Use SSL connections** in production environments
- **Create dedicated database users** with minimal required permissions
- **Enable row-level security** when appropriate
- **Use connection pooling** to optimize performance

## Troubleshooting

### Common Issues

**Connection Refused**
- Verify PostgreSQL server is running
- Check that PostgreSQL is listening on the correct port (default: 5432)
- Ensure firewall allows connections to PostgreSQL port

**Authentication Failed**
- Verify username and password are correct
- Check `pg_hba.conf` authentication configuration
- Ensure the database user exists and has appropriate permissions

**Performance Issues**
- Optimize PostgreSQL configuration for your workload
- Consider using read replicas for read-heavy applications

## Example Usage

```csharp
// In your Ivy app
public class ProductApp : AppBase<Product>
{
    public override Task<IView> BuildAsync(Product product)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Product: {product.Name}"),
                Text($"Price: ${product.Price:F2}")
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [SQL Server Provider](SqlServer.md)
- [MySQL Provider](MySQL.md)
- [Supabase Provider](Supabase.md)