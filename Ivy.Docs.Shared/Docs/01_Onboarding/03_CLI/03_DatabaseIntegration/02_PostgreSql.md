---
title: PostgreSQL
---

# PostgreSQL Database Provider

<Ingress>
Connect your Ivy application to PostgreSQL with automatic Entity Framework configuration.
</Ingress>

## Overview

PostgreSQL is an advanced open-source relational database known for its reliability, feature robustness, and performance. Ivy provides seamless integration with PostgreSQL through Entity Framework Core.

## Connection String Format

```text
Host=localhost;Database=mydb;Username=user;Password=pass
```

> **Note**: Ivy also supports URI-style connection strings (e.g., `postgresql://user:password@host:port/dbname`) and will automatically convert them to the key-value format.

### Authentication Options

**Standard Authentication**
```text
Host=localhost;Database=mydb;Username=myuser;Password=mypassword;Port=5432
```

**SSL Connection**
```text
Host=localhost;Database=mydb;Username=user;Password=pass;SSL Mode=Require
```

For all connection string options, see [Npgsql Connection String Parameters](https://www.npgsql.org/doc/connection-string-parameters.html).

## Configuration

Ivy automatically configures the **Npgsql.EntityFrameworkCore.PostgreSQL** package for PostgreSQL connections.

## Advanced Configuration

### Custom Schema

PostgreSQL supports multiple schemas. When configuring your PostgreSQL database with Ivy, you'll be prompted to select a schema from your database, or you can specify one directly using the `--schema` parameter:

```terminal
>ivy db add --provider Postgres --name MyPostgres --schema MyCustomSchema
```



## PostgreSQL-Specific Features

Key features Ivy can leverage:
- **JSONB columns** for document storage
- **Array types** for collections
- **Custom data types** and enums

See the [PostgreSQL features documentation](https://www.postgresql.org/docs/current/features.html) for details on these capabilities.

## Security Best Practices

- **Create dedicated database users** with minimal required permissions
- **Enable row-level security** when appropriate
- **Use connection pooling** to optimize performance

## Troubleshooting

### Common Issues

**Connection Issues**
- Verify server is running on port 5432
- Check credentials and firewall settings

**Authentication Problems**
- Check `pg_hba.conf` configuration

For detailed help, see the [PostgreSQL Documentation](https://www.postgresql.org/docs/current/) and search for common issues in the [PostgreSQL Wiki](https://wiki.postgresql.org/wiki/Main_Page).

## Related Documentation

- [Database Overview](01_Overview.md)
- [SQL Server Provider](SqlServer.md)
- [MySQL Provider](MySQL.md)
- [Supabase Provider](Supabase.md)
- [Official PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Npgsql Entity Framework Core Provider](https://www.npgsql.org/efcore/)