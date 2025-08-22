---
title: SQL Server
---

# SQL Server Database Provider

<Ingress>
Connect your Ivy application to Microsoft SQL Server with automatic Entity Framework configuration.
</Ingress>

## Overview

SQL Server is Microsoft's enterprise-grade relational database management system. Ivy provides seamless integration with SQL Server through Entity Framework Core.

## Connection String Format

```text
Server=localhost;Database=mydb;Trusted_Connection=true;
```

### Authentication Options

**Windows Authentication (Recommended)**
```text
Server=localhost;Database=mydb;Trusted_Connection=true;
```

**SQL Server Authentication**
```text
Server=localhost;Database=mydb;User Id=username;Password=password;
```

For Azure SQL and authentication details, see [SQL Server Authentication documentation](https://learn.microsoft.com/en-us/sql/relational-databases/security/choose-an-authentication-mode).

## Configuration

Ivy automatically configures the **Microsoft.EntityFrameworkCore.SqlServer** package for SQL Server connections.

## Advanced Configuration

### Custom Schema

```terminal
>ivy db add --provider SqlServer --name MySqlServer --schema MyCustomSchema
```

### Schema Support

SQL Server supports multiple schemas. When connecting with Ivy, you'll be prompted to select a schema from your database, or you can specify one directly using the `--schema` parameter:

```terminal
>ivy db add --provider SqlServer --name MySqlServer --schema MyCustomSchema
```

See [SQL Server Schema documentation](https://learn.microsoft.com/en-us/sql/relational-databases/security/schemas-general) for more details.

## Security Best Practices

- **Use Windows Authentication** when possible for local development
- **Use Azure AD authentication** for Azure SQL Database
- **Enable encryption** in connection strings for production

## Troubleshooting

### Common Issues

**Connection Problems**
- Verify server is running and network connectivity
- Check credentials and permissions
- Ensure firewall allows port 1433

For detailed troubleshooting, see [SQL Server Connection Troubleshooting](https://learn.microsoft.com/en-us/troubleshoot/sql/connect/resolve-connectivity-errors).

## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSql.md)
- [MySQL Provider](MySql.md)
- [SQLite Provider](SQLite.md)
- [Official SQL Server Documentation](https://learn.microsoft.com/en-us/sql/sql-server/)
- [Entity Framework Core for SQL Server](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/)