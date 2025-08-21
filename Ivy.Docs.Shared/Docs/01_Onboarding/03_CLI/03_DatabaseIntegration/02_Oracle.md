# Oracle Database Provider

<Ingress>
Connect your Ivy application to Oracle Database with automatic Entity Framework configuration.
</Ingress>

## Overview

Oracle Database is a multi-model database management system developed by Oracle Corporation. It's widely used in enterprise environments and offers advanced features for mission-critical applications.

## Connection String Format

### Connection String Format
```text
Data Source=localhost:1521/XE;User Id=hr;Password=password;
```

> **Note**: While Oracle database typically supports TNS names and EZCONNECT syntax, these methods are not currently supported in Ivy's Oracle provider.

## Configuration

Ivy automatically configures the **Oracle.EntityFrameworkCore** package for Oracle connections.

## Advanced Configuration

### TNS Names Configuration

If using TNS names, ensure your `tnsnames.ora` file is properly configured:

```text
MYDB =
  (DESCRIPTION =
    (ADDRESS = (PROTOCOL = TCP)(HOST = oracle-server)(PORT = 1521))
    (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = myservice)
    )
  )
```

For detailed instructions on TNS configuration, see the [Oracle Net Configuration documentation](https://docs.oracle.com/en/database/oracle/oracle-database/19/netrf/local-naming-parameters-in-tns-ora-file.html).

### Connection Pooling

```text
Data Source=localhost:1521/XE;User Id=hr;Password=password;Pooling=true;Min Pool Size=1;Max Pool Size=20;
```

## Oracle-Specific Features

Key enterprise features Ivy can leverage:
- **Advanced Security** - encryption and access controls
- **Performance** - partitioning and indexing
- **PL/SQL** - stored procedures and functions

For complete features, see the [Oracle Database documentation](https://docs.oracle.com/en/database/oracle/oracle-database/index.html).

## Security Best Practices

- **Use encrypted connections** with SSL/TLS
- **Create dedicated database users** with minimal required privileges
- **Enable auditing** for sensitive operations
- **Use Oracle Advanced Security** features in production
- **Implement connection pooling** to optimize resource usage

## Troubleshooting

### Common Issues

**TNS: Could not resolve the connect identifier**
- Verify TNS names configuration and environment variables

**Authentication Failed**
- Check username/password and account status

**Character Set Issues**
- Use `NLS_LANG` environment variable as needed

For detailed troubleshooting, refer to [Oracle Database Error Messages](https://docs.oracle.com/en/database/oracle/oracle-database/19/errmg/index.html).


## Example Usage

```csharp
// In your Ivy app
public class EmployeeApp : AppBase<Employee>
{
    public override Task<IView> BuildAsync(Employee employee)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Employee ID: {employee.EmployeeId}"),
                Text($"Name: {employee.FirstName} {employee.LastName}"),
                Text($"Department: {employee.Department}"),
                Text($"Hire Date: {employee.HireDate:yyyy-MM-dd}"),
                employee.IsActive
                    ? Badge("Active", Colors.Green)
                    : Badge("Inactive", Colors.Red)
            )
        );
    }
}
```


## Related Documentation

- [Database Overview](01_Overview.md)
- [SQL Server Provider](SqlServer.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [Enterprise Features](../../02_Concepts/Services.md)
- [Official Oracle Database Documentation](https://docs.oracle.com/en/database/oracle/oracle-database/index.html)
- [Oracle.EntityFrameworkCore Package](https://docs.oracle.com/en/database/oracle/oracle-database/19/odpnt/EFCore.html)