---
title: ClickHouse
---

# ClickHouse Database Provider

<Ingress>
Connect your Ivy application to ClickHouse with automatic Entity Framework configuration for high-performance analytics.
</Ingress>

## Overview

ClickHouse is an open-source column-oriented database management system designed for online analytical processing (OLAP) that allows generating analytical reports using SQL queries in real-time. Ivy provides seamless integration with ClickHouse through Entity Framework Core, enabling you to leverage its high-performance capabilities in your applications.

## Setup

### Adding ClickHouse Connection

```terminal
>ivy db add --provider ClickHouse --name MyClickHouse
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Connection Name**: Enter a name for your connection (PascalCase recommended)
2. **Connection String**: Provide your ClickHouse connection string

## Connection String Format

```text
Host=localhost;Port=9000;Database=default;Username=default;Password=;
```

### Authentication Options

**Basic Authentication**
```text
Host=localhost;Port=9000;Database=default;Username=default;Password=mypassword;
```

**Secure Connection with SSL**
```text
Host=localhost;Port=9440;Database=default;Username=default;Password=mypassword;Ssl=true;SslCa=/path/to/ca.crt;
```

**Connection with Compression**
```text
Host=localhost;Port=9000;Database=default;Username=default;Password=mypassword;Compression=true;
```

## Configuration

### Entity Framework Setup

Ivy automatically configures:
- **EntityFrameworkCore.ClickHouse** package
- **Connection strings** stored in .NET User Secrets
- **DbContext** with ClickHouse provider configuration

### Generated Files

```text
Connections/
└── MyClickHouse/
    ├── MyClickHouseContext.cs             # Entity Framework DbContext
    ├── MyClickHouseContextFactory.cs      # DbContext factory
    ├── MyClickHouseConnection.cs          # Connection configuration
    └── [EntityName].cs...                 # Generated entity classes
```

## Advanced Configuration

### Performance Settings

ClickHouse connection strings support various performance parameters:

**Buffering and Batch Size**
```text
Host=localhost;Port=9000;Database=default;Username=default;Password=mypassword;BufferSize=32768;MaxInsertBlockSize=1000000;
```

**Timeouts**
```text
Host=localhost;Port=9000;Database=default;Username=default;Password=mypassword;Timeout=30;ConnectionTimeout=10;
```

### ClickHouse-Specific Features

ClickHouse offers advanced features that Ivy can leverage:
- **Columnar storage** for efficient data compression and query processing
- **Vectorized query execution** for high-performance analytics
- **Materialized views** for pre-computed results
- **Approximate query processing** for fast aggregations
- **Distributed tables** for horizontal scaling

## Security Best Practices

- **Use TLS/SSL** for secure connections
- **Store connection strings** in User Secrets or secure vaults
- **Implement IP-based access restrictions**
- **Use a separate user** with limited permissions for application access
- **Enable secure communication** between ClickHouse nodes in a cluster

## Troubleshooting

### Common Issues

**Connection Refused**
- Verify ClickHouse server is running
- Check that ClickHouse is listening on the specified port
- Ensure firewall allows connections to the ClickHouse port

**Authentication Failed**
- Verify username and password are correct
- Check that the user has access to the specified database
- Ensure the user has appropriate permissions

**Query Performance Issues**
- Review table engine selection for your use case
- Check indexing strategy for frequently queried columns
- Optimize schema design for analytical workloads
- Consider using materialized views for common queries

## Example Usage

```csharp
// In your Ivy app
public class AnalyticsApp : AppBase<MetricData>
{
    public override Task<IView> BuildAsync(MetricData data)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Date: {data.EventDate}"),
                Text($"Event Type: {data.EventType}"),
                Text($"Count: {data.EventCount}")
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [Snowflake Provider](Snowflake.md)
- [ClickHouse Documentation](https://clickhouse.com/docs/)