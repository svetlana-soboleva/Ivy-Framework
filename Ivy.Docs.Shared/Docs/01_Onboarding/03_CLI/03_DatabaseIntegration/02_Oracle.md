# Oracle Database Provider

<Ingress>
Connect your Ivy application to Oracle Database with automatic Entity Framework configuration.
</Ingress>

## Overview

Oracle Database is a multi-model database management system developed by Oracle Corporation. It's widely used in enterprise environments and offers advanced features for mission-critical applications.

## Connection String Format

### Basic Connection String
```text
Data Source=localhost:1521/XE;User Id=hr;Password=password;
```

### TNS Connection String
```text
Data Source=MyTnsAlias;User Id=username;Password=password;
```

### Advanced Connection String
```text
Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=hr;Password=password;
```

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

### Connection Pooling

```text
Data Source=localhost:1521/XE;User Id=hr;Password=password;Pooling=true;Min Pool Size=1;Max Pool Size=20;
```

## Oracle-Specific Features

Oracle offers enterprise-grade features:
- **Advanced Security** - encryption, auditing, and access controls
- **High Availability** - RAC, Data Guard, and flashback
- **Performance** - partitioning, indexing, and materialized views
- **PL/SQL** - stored procedures and functions
- **Advanced Analytics** - window functions and statistical functions

## Security Best Practices

- **Use encrypted connections** with SSL/TLS
- **Create dedicated database users** with minimal required privileges
- **Enable auditing** for sensitive operations
- **Use Oracle Advanced Security** features in production
- **Implement connection pooling** to optimize resource usage

## Troubleshooting

### Common Issues

**TNS: Could not resolve the connect identifier**
- Verify TNS names are correctly configured
- Check `ORACLE_HOME` and `TNS_ADMIN` environment variables
- Ensure `tnsnames.ora` file is accessible
- Test connection using `tnsping` command

**ORA-12154: TNS:could not resolve the connect identifier specified**
- Verify the service name or SID in the connection string
- Check network connectivity to the Oracle server
- Ensure Oracle listener is running on the target server

**ORA-01017: invalid username/password; logon denied**
- Verify username and password are correct
- Check if the user account is locked or expired
- Ensure the user has CONNECT privilege

**Character Set Issues**
- Ensure consistent character set configuration
- Use `NLS_LANG` environment variable if needed
- Consider Unicode (UTF8) character sets for international applications

## Oracle Cloud Integration

### Autonomous Database

For Oracle Autonomous Database, use wallet-based connections:

```text
Data Source=mydb_high;User Id=ADMIN;Password=password;
```

Ensure your Oracle Wallet is configured and accessible.

### Always Free Tier

Oracle Cloud offers Always Free Autonomous Database:
- Up to 2 databases
- 1 OCPU and 20 GB storage each
- Perfect for development and small applications

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

## Migration Considerations

When migrating to/from Oracle:

### From Other Databases
- **Data types** may need mapping (e.g., `IDENTITY` to `SEQUENCE`)
- **String comparisons** are case-sensitive by default
- **Date/Time handling** differs from other databases
- **Naming conventions** (Oracle uses uppercase by default)

### To Other Databases
- Export data using Oracle Data Pump or SQL Developer
- Map Oracle-specific data types to target database equivalents
- Convert PL/SQL procedures to target database stored procedures

## Related Documentation

- [Database Overview](01_Overview.md)
- [SQL Server Provider](SqlServer.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [Enterprise Features](../../02_Concepts/Services.md)