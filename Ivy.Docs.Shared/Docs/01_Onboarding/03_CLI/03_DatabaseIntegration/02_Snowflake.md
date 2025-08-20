---
title: Snowflake
---

# Snowflake Database Provider

<Ingress>
Connect your Ivy application to Snowflake with automatic Entity Framework configuration.
</Ingress>

## Overview

Snowflake is a cloud-based data warehousing platform that provides a single, integrated solution for data storage, processing, and analytics. Ivy offers seamless integration with Snowflake through Entity Framework Core, allowing you to leverage Snowflake's powerful data processing capabilities in your applications.

## Setup

### Adding Snowflake Connection

```terminal
>ivy db add --provider Snowflake --name MySnowflake
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Connection Name**: Enter a name for your connection (PascalCase recommended)
2. **Connection String**: Provide your Snowflake connection string with account, user, password, and database information

## Connection String Format

```text
account=myaccount;user=myuser;password=mypassword;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

### Authentication Options

**User/Password Authentication**
```text
account=myaccount;user=myuser;password=mypassword;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

**OAuth Authentication**
```text
account=myaccount;authenticator=oauth;token=mytoken;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

**Key Pair Authentication**
```text
account=myaccount;user=myuser;private_key_file=rsa_key.p8;private_key_file_pwd=passphrase;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

**SSO Authentication**
```text
account=myaccount;authenticator=externalbrowser;user=myuser;db=mydatabase;schema=myschema;warehouse=mywarehouse
```

## Configuration

### Entity Framework Setup

Ivy automatically configures:
- **EFCore.Snowflake** package
- **Connection strings** stored in .NET User Secrets
- **DbContext** with Snowflake provider configuration

### Generated Files

```text
Connections/
└── MySnowflake/
    ├── MySnowflakeContext.cs             # Entity Framework DbContext
    ├── MySnowflakeContextFactory.cs      # DbContext factory
    ├── MySnowflakeConnection.cs          # Connection configuration
    └── [EntityName].cs...                # Generated entity classes
```

## Advanced Configuration

### Custom Parameters

Snowflake connection strings support various additional parameters:

**Session Parameters**
```text
account=myaccount;user=myuser;password=mypassword;db=mydatabase;schema=myschema;warehouse=mywarehouse;TIMESTAMP_OUTPUT_FORMAT='YYYY-MM-DD HH24:MI:SS.FF';TIMESTAMP_TYPE_MAPPING='TIMESTAMP_LTZ'
```

**Connection Parameters**
```text
account=myaccount;user=myuser;password=mypassword;db=mydatabase;schema=myschema;warehouse=mywarehouse;connection_timeout=60;request_timeout=300
```

### Snowflake-Specific Features

Snowflake offers advanced features that Ivy can leverage:
- **Time Travel** for accessing historical data
- **Zero-copy cloning** for data duplication
- **Semi-structured data types** (JSON, Avro, Parquet)
- **Automatic clustering** for query optimization
- **Cross-database joins** for federated queries

## Security Best Practices

- **Use key pair authentication** instead of password authentication
- **Store connection strings** in User Secrets or secure vaults
- **Enable network policies** to restrict access
- **Use private connectivity** (AWS PrivateLink, Azure Private Link) when possible
- **Implement column-level security** for sensitive data

## Troubleshooting

### Common Issues

**Authentication Failed**
- Verify username and password are correct
- Check that your account identifier is correctly formatted
- Ensure your IP address is not blocked by network policies

**Connection Timeouts**
- Verify warehouse is running and properly sized
- Check network connectivity to Snowflake
- Increase the connection timeout in your connection string

**Schema Access Issues**
- Ensure the user has appropriate permissions
- Verify the schema exists in the specified database
- Check that the warehouse has access to the database

## Example Usage

```csharp
// In your Ivy app
public class AnalyticsApp : AppBase<AnalyticsData>
{
    public override Task<IView> BuildAsync(AnalyticsData data)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Period: {data.Period}"),
                Text($"Revenue: ${data.Revenue:F2}"),
                Text($"Growth: {data.GrowthPercentage}%")
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [SQL Server Provider](SqlServer.md)
- [Snowflake Documentation](https://docs.snowflake.com/)