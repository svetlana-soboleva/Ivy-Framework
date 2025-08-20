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

**Connection Pooling**
```text
Server=localhost;Database=mydb;Uid=user;Pwd=pass;Pooling=true;MinimumPoolSize=1;MaximumPoolSize=20
```

## Configuration

Ivy automatically configures the **Pomelo.EntityFrameworkCore.MySql** package for MySQL connections.

## MySQL-Specific Features

MySQL offers features that Ivy can leverage:
- **JSON columns** for document storage (MySQL 5.7+)
- **Full-text indexes** for search functionality
- **Partitioning** for large tables
- **Multiple storage engines** (InnoDB, MyISAM, etc.)

## Security Best Practices

- **Use SSL connections** in production environments
- **Create dedicated database users** with minimal required permissions
- **Enable binary logging** for point-in-time recovery
- **Use connection pooling** to optimize performance

## Troubleshooting

### Common Issues

**Connection Timeout**
- Verify MySQL server is running
- Check that MySQL is listening on the correct port (default: 3306)
- Ensure firewall allows connections to MySQL port

**Authentication Failed**
- Verify username and password are correct
- Check MySQL user privileges: `SHOW GRANTS FOR 'username'@'host'`
- Verify host restrictions for the MySQL user

**Character Set Issues**
- Specify charset in connection string: `CharSet=utf8mb4`
- Use `utf8mb4` for full UTF-8 support including emojis

**Performance Issues**
- Optimize MySQL configuration (`my.cnf`)
- Consider using read replicas for read-heavy applications
- Monitor slow query log for optimization opportunities

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