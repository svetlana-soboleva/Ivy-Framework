# SQL Server Database Provider

<Ingress>
Connect your Ivy application to Microsoft SQL Server with automatic Entity Framework configuration.
</Ingress>

## Overview

SQL Server is Microsoft's enterprise-grade relational database management system. Ivy provides seamless integration with SQL Server through Entity Framework Core.

## Setup

### Adding SQL Server Connection

```terminal
>ivy db add --provider SqlServer --name MySqlServer
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Connection Name**: Enter a name for your connection (PascalCase recommended)
2. **Connection String**: Provide your SQL Server connection string
3. **Schema**: Specify the database schema (optional, defaults to `dbo`)

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

### Entity Framework Setup

Ivy automatically configures:
- **Microsoft.EntityFrameworkCore.SqlServer** package
- **Connection strings** stored in .NET User Secrets
- **DbContext** with SQL Server provider configuration

### Generated Files

```text
Connections/
└── MySqlServer/
    ├── MySqlServerContext.cs             # Entity Framework DbContext
    ├── MySqlServerContextFactory.cs      # DbContext factory
    ├── MySqlServerConnection.cs          # Connection configuration
    └── [EntityName].cs...                # Generated entity classes
```

## Advanced Configuration

### Custom Schema

```terminal
>ivy db add --provider SqlServer --name MySqlServer --schema MyCustomSchema
```

### Multiple Schemas

SQL Server supports multiple schemas. You can specify different schemas when adding connections or configure them in the DbContext.

## Security Best Practices

- **Use Windows Authentication** when possible for local development
- **Store connection strings** in User Secrets or Azure Key Vault
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
            Text($"Employee: {employee.Name}")
        );
    }
}
```

## Related Documentation

- [Database Overview](../03_Db.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [MySQL Provider](MySQL.md)
- [SQLite Provider](SQLite.md)