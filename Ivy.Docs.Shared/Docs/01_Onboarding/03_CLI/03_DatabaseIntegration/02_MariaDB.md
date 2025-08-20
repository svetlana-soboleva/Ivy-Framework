---
title: MariaDB
---

# MariaDB Database Provider

<Ingress>
Connect your Ivy application to MariaDB with automatic Entity Framework configuration.
</Ingress>

## Overview

MariaDB is a popular open-source relational database that started as a fork of MySQL. It offers enhanced features, improved performance, and better storage engines while maintaining MySQL compatibility.

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

Ivy automatically configures the **Pomelo.EntityFrameworkCore.MySql** package for MariaDB connections.

## MariaDB-Specific Features

MariaDB offers enhanced features over MySQL:
- **Advanced JSON support** with better performance
- **Temporal tables** for data versioning
- **Multiple storage engines** (InnoDB, Aria, ColumnStore)
- **Virtual columns** and computed fields
- **Improved optimizer** and performance schema

## Security Best Practices

- **Use SSL connections** in production environments
- **Create dedicated database users** with minimal required permissions
- **Enable binary logging** for point-in-time recovery
- **Implement proper backup strategies** with MariaDB backup tools

## Troubleshooting

### Common Issues

**Connection Timeout**
- Verify MariaDB server is running
- Check that MariaDB is listening on the correct port (default: 3306)
- Ensure firewall allows connections to MariaDB port

**Authentication Failed**
- Verify username and password are correct
- Check MariaDB user privileges: `SHOW GRANTS FOR 'username'@'host'`
- Verify host restrictions for the MariaDB user

**Character Set Issues**
- Specify charset in connection string: `CharSet=utf8mb4`
- Use `utf8mb4` for full UTF-8 support including emojis

**Storage Engine Issues**
- Verify the correct storage engine is being used (usually InnoDB)
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