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

**Authentication Failed**
- Check username/password and account status

**Character Set Issues**
- Use `NLS_LANG` environment variable as needed

For detailed troubleshooting, refer to [Oracle Database Error Messages](https://docs.oracle.com/en/database/oracle/oracle-database/19/errmg/index.html).

## Related Documentation

- [Database Overview](Overview.md)
- [SQL Server Provider](SqlServer.md)
- [PostgreSQL Provider](PostgreSql.md)
- [Enterprise Features](../../02_Concepts/Services.md)
- [Official Oracle Database Documentation](https://docs.oracle.com/en/database/oracle/oracle-database/index.html)
- [Oracle.EntityFrameworkCore Package](https://docs.oracle.com/en/database/oracle/oracle-database/19/odpnt/EFCore.html)