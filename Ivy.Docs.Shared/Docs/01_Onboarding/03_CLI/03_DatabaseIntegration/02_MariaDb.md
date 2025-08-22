---
title: MariaDB
---

# MariaDB Database Provider

<Ingress>
Connect your Ivy application to MariaDB with automatic Entity Framework configuration.
</Ingress>

## Overview

MariaDB is a popular open-source relational database that started as a fork of MySQL. It offers enhanced features, improved performance, and better storage engines while maintaining MySQL compatibility. For more information, visit the [MariaDB Knowledge Base](https://mariadb.com/kb/en/about-mariadb-server/).

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

See [MariaDB Connection Parameters](https://mariadb.com/kb/en/about-mariadb-connector-net/) for all options.

## Configuration

Ivy automatically configures the **Pomelo.EntityFrameworkCore.MySql** package for MariaDB connections.

> **Note**: When you provide a connection string, Ivy will verify that you're connecting to a MariaDB server (not MySQL). If it detects MySQL instead, you'll be prompted to use the MySQL provider.

## MariaDB-Specific Features

Key advantages over MySQL:
- **Advanced JSON support** with better performance
- **Temporal tables** for data versioning
- **Multiple storage engines** including Aria and ColumnStore

See the [MariaDB Documentation](https://mariadb.com/kb/en/library/documentation/) for details.

## Security Best Practices

- **Use SSL connections** in production environments
- **Create dedicated database users** with minimal required permissions
- **Enable binary logging** for point-in-time recovery
- **Implement proper backup strategies** with MariaDB backup tools

## Troubleshooting

### Common Issues

**Connection Issues**
- Verify server is running on port 3306
- Check firewall settings

**Authentication Problems**
- Verify credentials and user privileges

**Character Set Issues**
- Use `CharSet=utf8mb4` in connection string

See [MariaDB Troubleshooting](https://mariadb.com/kb/en/troubleshooting-connection-issues/) for help.

## Related Documentation

- [Database Overview](01_Overview.md)
- [MySQL Provider](MySql.md)
- [PostgreSQL Provider](PostgreSql.md)
- [SQL Server Provider](SqlServer.md)
- [Official MariaDB Documentation](https://mariadb.com/kb/en/documentation/)
- [Pomelo.EntityFrameworkCore.MySql for MariaDB](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)