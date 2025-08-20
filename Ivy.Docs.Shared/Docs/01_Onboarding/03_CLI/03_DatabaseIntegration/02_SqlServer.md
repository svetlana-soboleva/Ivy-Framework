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

**Windows Authentication (Recommended for local development)**
```text
Server=localhost;Database=mydb;Trusted_Connection=true;
```

**SQL Server Authentication**
```text
Server=localhost;Database=mydb;User Id=username;Password=password;
```

**Azure SQL Database**
```text
Server=tcp:yourserver.database.windows.net,1433;Database=mydb;User ID=username;Password=password;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;
```

## Configuration

Ivy automatically configures the **Microsoft.EntityFrameworkCore.SqlServer** package for SQL Server connections.

## Advanced Configuration

### Custom Schema

```terminal
>ivy db add --provider SqlServer --name MySqlServer --schema MyCustomSchema
```

### Multiple Schemas

SQL Server supports multiple schemas. You can specify different schemas when adding connections or configure them in the DbContext.

## Security Best Practices

- **Use Windows Authentication** when possible for local development
- **Use Azure AD authentication** for Azure SQL Database
- **Enable encryption** in connection strings for production

## Troubleshooting

### Common Issues

**Connection Timeout**
- Increase connection timeout in connection string
- Check network connectivity to SQL Server instance
- Verify SQL Server is running and accepting connections

**Authentication Failures**
- Verify credentials are correct
- Check if Windows Authentication is properly configured
- Ensure user has necessary database permissions

**Firewall Issues**
- Configure Windows Firewall to allow SQL Server traffic
- Check Azure SQL Database firewall rules
- Verify port 1433 is accessible

## Example Usage

```csharp
// In your Ivy app
public class EmployeeApp : AppBase<Employee>
{
    public override Task<IView> BuildAsync(Employee employee)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Employee: {employee.Name}"),
                Text($"Department: {employee.Department}")
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [MySQL Provider](MySQL.md)
- [SQLite Provider](SQLite.md)