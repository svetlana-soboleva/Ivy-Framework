---
title: MySQL
---

# MySQL Database Provider

<Ingress>
Connect your Ivy application to MySQL with automatic Entity Framework configuration.
</Ingress>

## Overview

MySQL is one of the world's most popular open-source relational databases, known for its speed, reliability, and ease of use. Ivy provides seamless integration with MySQL through Entity Framework Core.

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

For all connection options, see [MySQL Connection String Options](https://dev.mysql.com/doc/connector-net/en/connector-net-connection-options.html).

## Configuration

Ivy automatically configures the **Pomelo.EntityFrameworkCore.MySql** package for MySQL connections.

> **Note**: When you provide a connection string, Ivy will verify that you're connecting to an actual MySQL server (not MariaDB). If it detects MariaDB instead, you'll be prompted to use the MariaDB provider.

## MySQL-Specific Features

Key features Ivy can leverage:
- **JSON columns** for document storage (MySQL 5.7+)
- **Full-text indexes** for search functionality
- **Multiple storage engines** (InnoDB, MyISAM)

See [MySQL Feature Reference](https://dev.mysql.com/doc/refman/8.0/en/features.html) for details.

## Security Best Practices

- **Use SSL connections** in production environments
- **Create dedicated database users** with minimal required permissions
- **Enable binary logging** for point-in-time recovery
- **Use connection pooling** to optimize performance

For more security recommendations, see [MySQL Security Guidelines](https://dev.mysql.com/doc/refman/8.0/en/security-guidelines.html).

## Troubleshooting

### Common Issues

**Connection Issues**
- Verify server is running on port 3306
- Check firewall settings

**Authentication Problems**
- Verify credentials and user privileges

**Character Set Issues**
- Use `CharSet=utf8mb4` in connection string

See [MySQL Troubleshooting](https://dev.mysql.com/doc/refman/8.0/en/problems.html) for more help.

## Example Usage

```csharp
// In your Ivy app
public class OrderApp : AppBase<Order>
{
    public override Task<IView> BuildAsync(Order order)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Order #{order.Id}"),
                Text($"Customer: {order.CustomerName}"),
                Text($"Total: ${order.Total:F2}"),
                Text($"Status: {order.Status}")
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [MariaDB Provider](MariaDB.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [SQL Server Provider](SqlServer.md)
- [Official MySQL Documentation](https://dev.mysql.com/doc/)
- [Pomelo.EntityFrameworkCore.MySql Documentation](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)