---
title: MariaDB
---

# MariaDB Database Provider

<Ingress>
Connect your Ivy application to MariaDB with automatic Entity Framework configuration.
</Ingress>

## Overview

MariaDB is a popular open-source relational database that started as a fork of MySQL. It offers enhanced features, improved performance, and better storage engines while maintaining MySQL compatibility.

## Setup

### Adding MariaDB Connection

```terminal
>ivy db add --provider MariaDb --name MyMariaDb
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Connection Name**: Enter a name for your connection (PascalCase recommended)
2. **Connection String**: Provide your MariaDB connection string
3. **Schema**: Specify the database schema (optional)

## Connection String Format

```text
Server=localhost;Database=mydb;Uid=user;Pwd=pass
```

### Authentication Options

**Standard Authentication**
```text
Server=localhost;Database=mydb;Uid=myuser;Pwd=mypassword;Port=3306
```

**SSL Connection**
```text
Server=localhost;Database=mydb;Uid=user;Pwd=pass;SslMode=Required
```

**Connection Pooling**
```text
Server=localhost;Database=mydb;Uid=user;Pwd=pass;Pooling=true;MinimumPoolSize=1;MaximumPoolSize=20
```

## Configuration

### Entity Framework Setup

Ivy automatically configures:
- **Pomelo.EntityFrameworkCore.MySql** package
- **Connection strings** stored in .NET User Secrets
- **DbContext** with MariaDB provider configuration

### Generated Files

```text
Connections/
└── MyMariaDb/
    ├── MyMariaDbContext.cs              # Entity Framework DbContext
    ├── MyMariaDbContextFactory.cs       # DbContext factory
    ├── MyMariaDbConnection.cs           # Connection configuration
    └── [EntityName].cs...               # Generated entity classes
```

## Advanced Configuration

### Custom Port

```terminal
>ivy db add --provider MariaDb --connection-string "Server=localhost;Port=3307;Database=mydb;Uid=user;Pwd=pass"
```

### MariaDB-Specific Features

MariaDB offers enhanced features over MySQL:
- **Advanced JSON support** with better performance
- **Temporal tables** for data versioning
- **Multiple storage engines** (InnoDB, Aria, ColumnStore)
- **Virtual columns** and computed fields
- **Improved optimizer** and performance schema

## Security Best Practices

- **Use SSL connections** in production environments
- **Store connection strings** in User Secrets or secure vaults
- **Create dedicated database users** with minimal required permissions
- **Enable binary logging** for point-in-time recovery
- **Use connection pooling** to optimize performance
- **Implement proper backup strategies** with MariaDB backup tools

## Troubleshooting

### Common Issues

**Connection Timeout**
- Verify MariaDB server is running
- Check that MariaDB is listening on the correct port (default: 3306)
- Ensure firewall allows connections to MariaDB port
- Increase connection timeout in connection string

**Authentication Failed**
- Verify username and password are correct
- Check MariaDB user privileges: `SHOW GRANTS FOR 'username'@'host'`
- Ensure the database user exists and has appropriate permissions
- Verify host restrictions for the MariaDB user

**SSL/TLS Issues**
- Check SSL certificate configuration
- Verify SSL mode requirements match server configuration
- Ensure certificates are properly installed and accessible

**Character Set Issues**
- Specify charset in connection string: `CharSet=utf8mb4`
- Ensure database and tables use consistent character sets
- Use `utf8mb4` for full UTF-8 support including emojis

**Storage Engine Issues**
- Verify the correct storage engine is being used (usually InnoDB)
- Check storage engine compatibility with your queries
- Consider using Aria for read-heavy workloads

## Migration from MySQL

MariaDB maintains high compatibility with MySQL, making migration straightforward:

1. **Backup your MySQL database**
2. **Update connection strings** to point to MariaDB
3. **Test application functionality** thoroughly
4. **Take advantage of MariaDB-specific features** as needed

## Example Usage

```csharp
// In your Ivy app
public class CustomerApp : AppBase<Customer>
{
    public override Task<IView> BuildAsync(Customer customer)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Customer: {customer.Name}"),
                Text($"Email: {customer.Email}"),
                Text($"Registration: {customer.CreatedAt:yyyy-MM-dd}"),
                customer.IsActive
                    ? Badge("Active", Colors.Green)
                    : Badge("Inactive", Colors.Red)
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [MySQL Provider](MySQL.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [SQL Server Provider](SqlServer.md)