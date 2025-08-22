# Snowflake Database Provider

<Ingress>
Connect your Ivy application to Snowflake with automatic Entity Framework configuration.
</Ingress>

## Overview

Snowflake is a cloud-based data warehousing platform that provides a single, integrated solution for data storage, processing, and analytics. Ivy offers seamless integration with Snowflake through Entity Framework Core, allowing you to leverage Snowflake's powerful data processing capabilities in your applications.

## Connection String Format

```text
account=myaccount;user=myuser;password=mypassword;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

### Authentication Options

**User/Password Authentication**
```text
account=myaccount;user=myuser;password=mypassword;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

**Key Pair Authentication (Recommended)**
```text
account=myaccount;user=myuser;private_key_file=rsa_key.p8;private_key_file_pwd=passphrase;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

For additional authentication methods (OAuth, SSO) and detailed options, see the [Snowflake Authentication documentation](https://docs.snowflake.com/en/user-guide/authentication).

## Configuration

Ivy automatically configures the **EFCore.Snowflake** package for Snowflake connections.

## Advanced Configuration

### Custom Parameters

Common connection options include timeouts and date formatting:

```text
account=myaccount;user=myuser;password=mypassword;db=mydatabase;schema=myschema;warehouse=mywarehouse;connection_timeout=60;TIMESTAMP_OUTPUT_FORMAT='YYYY-MM-DD HH24:MI:SS.FF'
```

For all available parameters, see [Snowflake Connection Parameters](https://docs.snowflake.com/en/user-guide/dotnet-driver-connection).

## Snowflake-Specific Features

Key features Ivy can leverage:
- **Semi-structured data types** (JSON, Avro, Parquet)
- **Time Travel** for historical data access
- **Zero-copy cloning** and **Automatic clustering**

Learn more in the [Snowflake Documentation](https://docs.snowflake.com/en/user-guide).

## Security Best Practices

- **Use key pair authentication** instead of password authentication
- **Enable network policies** to restrict access
- **Use private connectivity** (AWS PrivateLink, Azure Private Link) when possible
- **Implement column-level security** for sensitive data

## Troubleshooting

### Common Issues

**Authentication Failed**
- Verify username and password are correct
- Check that your account identifier is correctly formatted

**Connection Timeouts**
- Verify warehouse is running
- Check network connectivity to Snowflake

**Schema Access Issues**
- Ensure the user has appropriate permissions
- Verify the schema exists in the specified database

## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSql.md)
- [SQL Server Provider](SqlServer.md)
- [Official Snowflake Documentation](https://docs.snowflake.com)
- [Snowflake .NET Connector](https://docs.snowflake.com/en/developer-guide/dotnet-driver)
- [EFCore.Snowflake Package](https://github.com/Sielnowy/EFCore.Snowflake)
