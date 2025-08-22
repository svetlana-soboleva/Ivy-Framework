---
title: ClickHouse
---

# ClickHouse Database Provider

<Ingress>
Connect your Ivy application to ClickHouse with automatic Entity Framework configuration for high-performance analytics.
</Ingress>

## Overview

ClickHouse is an open-source column-oriented database management system designed for online analytical processing (OLAP) that allows generating analytical reports using SQL queries in real-time. Ivy provides seamless integration with ClickHouse through Entity Framework Core, enabling you to leverage its high-performance capabilities in your applications.

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
Host=localhost;Port=9440;Database=default;Username=default;Password=mypassword;Ssl=true;
```

For all connection options, see the [ClickHouse Client Configuration](https://clickhouse.com/docs/en/interfaces/tcp).

## Configuration

Ivy automatically configures the **EntityFrameworkCore.ClickHouse** package and imports the `ClickHouse.EntityFrameworkCore.Extensions` namespace for ClickHouse connections.

## Advanced Configuration

### Performance Settings

Common performance parameters include buffering and timeouts:

```text
Host=localhost;Port=9000;Database=default;Username=default;Password=mypassword;BufferSize=32768;ConnectionTimeout=10;
```

For performance tuning, see the [ClickHouse Performance Tuning](https://clickhouse.com/docs/en/operations/performance-tuning) documentation.

## ClickHouse-Specific Features

Key features for analytics applications:
- **Columnar storage** for efficient query processing
- **Materialized views** for pre-computed results
- **Vectorized query execution** for high performance

See the [ClickHouse Features Overview](https://clickhouse.com/docs/en/about-us/distinctive-features) for details.

## Security Best Practices

- **Use TLS/SSL** for secure connections
- **Implement IP-based access restrictions**
- **Use a separate user** with limited permissions for application access
- **Enable secure communication** between ClickHouse nodes in a cluster

## Troubleshooting

### Common Issues

**Connection Issues**
- Verify server is running and listening on the specified port
- Check credentials and permissions

**Performance Issues**
- Review table engine selection and indexing strategy
- Consider materialized views for common queries

See the [ClickHouse Troubleshooting Guide](https://clickhouse.com/docs/en/operations/troubleshooting) for more help.

## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSql.md)
- [Snowflake Provider](Snowflake.md)
- [Official ClickHouse Documentation](https://clickhouse.com/docs/)
- [EntityFrameworkCore.ClickHouse Package](https://github.com/denis-ivanov/EntityFrameworkCore.ClickHouse)
- [ClickHouse.EntityFrameworkCore.Extensions Package](https://github.com/DarkWanderer/ClickHouse.EntityFrameworkCore)
