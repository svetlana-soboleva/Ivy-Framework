---
title: PostgreSQL
---

# PostgreSQL Database Provider

<Ingress>
Connect your Ivy application to PostgreSQL with automatic Entity Framework configuration.
</Ingress>

## Overview

PostgreSQL is an advanced open-source relational database known for its reliability, feature robustness, and performance. Ivy provides seamless integration with PostgreSQL through Entity Framework Core.

## Setup

### Adding PostgreSQL Connection

```terminal
>ivy db add --provider Postgres --name MyPostgres
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Connection Name**: Enter a name for your connection (PascalCase recommended)
2. **Connection String**: Provide your PostgreSQL connection string
3. **Schema**: Specify the database schema (optional, defaults to `public`)

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

### Entity Framework Setup

Ivy automatically configures:
- **Npgsql.EntityFrameworkCore.PostgreSQL** package
- **Connection strings** stored in .NET User Secrets
- **DbContext** with PostgreSQL provider configuration

### Generated Files

```text
Connections/
└── MyPostgres/
    ├── MyPostgresContext.cs             # Entity Framework DbContext
    ├── MyPostgresContextFactory.cs      # DbContext factory
    ├── MyPostgresConnection.cs          # Connection configuration
    └── [EntityName].cs...               # Generated entity classes
```

## Advanced Configuration

### Custom Schema

```terminal
>ivy db add --provider Postgres --name MyPostgres --schema MyCustomSchema
```

### Multiple Schemas

PostgreSQL supports multiple schemas. You can specify different schemas when adding connections or configure them in the DbContext.

### PostgreSQL-Specific Features

PostgreSQL offers advanced features that Ivy can leverage:
- **JSONB columns** for document storage
- **Array types** for storing collections
- **Custom data types** and enums
- **Full-text search** capabilities

## Security Best Practices

- **Use SSL connections** in production environments
- **Store connection strings** in User Secrets or secure vaults
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

**SSL/TLS Issues**
- Check SSL certificate configuration
- Verify SSL mode requirements match server configuration
- Ensure certificates are properly installed

**Performance Issues**
- Enable connection pooling in connection string
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