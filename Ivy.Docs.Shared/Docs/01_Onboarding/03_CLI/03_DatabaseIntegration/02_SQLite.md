---
title: SQLite
---

# SQLite Database Provider

<Ingress>
Connect your Ivy application to SQLite with automatic Entity Framework configuration.
</Ingress>

## Overview

SQLite is a lightweight, file-based relational database that's perfect for development, testing, and applications that need a simple, self-contained database solution. No server setup required!

## Connection String Format

```text
Data Source=data.db
```

### Common Configurations

**Relative Path**
```text
Data Source=data.db
```

**Absolute Path**
```text
Data Source=/path/to/your/database.db
```

**In-Memory Database (for testing)**
```text
Data Source=:memory:
```

**With Additional Options**
```text
Data Source=data.db;Cache=Shared;Foreign Keys=True
```

## Configuration

Ivy automatically configures the **Microsoft.EntityFrameworkCore.Sqlite** package for SQLite connections.

Unlike other providers, SQLite connection strings are stored as file paths rather than in User Secrets.

## SQLite-Specific Features

SQLite offers unique characteristics:
- **Zero-configuration** - no server setup required
- **Cross-platform** database files
- **ACID transactions** with rollback support
- **Full-text search** capabilities (FTS5)
- **JSON support** (since SQLite 3.45.0)
- **Lightweight** - entire database in a single file

## Security Best Practices

- **Secure file permissions** on database files
- **Backup database files** regularly
- **Use WAL mode** for better concurrency: `PRAGMA journal_mode=WAL`
- **Enable foreign keys** for referential integrity: `PRAGMA foreign_keys=ON`
- **Consider encryption** with SQLCipher for sensitive data

## Troubleshooting

### Common Issues

**File Access Issues**
- Verify the application has read/write permissions to the database file location
- Check that the directory exists if using a specific path
- Ensure the database file isn't locked by another process

**Database Locked Errors**
- Close all connections properly after use
- Consider using WAL mode for better concurrency
- Check for long-running transactions

**Performance Issues**
- Create appropriate indexes for your queries
- Use `ANALYZE` to update query planner statistics
- Consider `VACUUM` to reclaim space and optimize

**Migration Issues**
- SQLite has limited `ALTER TABLE` support
- Some schema changes require table recreation
- Always backup before major migrations

## Development vs Production

### Development Benefits
- **No setup required** - perfect for getting started quickly
- **Easy debugging** - database is just a file
- **Version control friendly** - can commit small test databases
- **Cross-platform** - works the same everywhere

### Production Considerations
- **Concurrent writes** are limited compared to server databases
- **No network access** - database must be on same machine
- **Consider server databases** for high-concurrency applications

## Example Usage

```csharp
// In your Ivy app
public class NoteApp : AppBase<Note>
{
    public override Task<IView> BuildAsync(Note note)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Note: {note.Title}"),
                Text(note.Content),
                Text($"Created: {note.CreatedAt:yyyy-MM-dd HH:mm}"),
                Button("Edit", () => EditNote(note.Id)),
                Button("Delete", () => DeleteNote(note.Id))
            )
        );
    }
}
```

## Migration Path

SQLite is often used for development and then migrated to a server database for production:

1. **Develop with SQLite** for quick prototyping
2. **Export data** when ready for production
3. **Switch provider** to PostgreSQL, SQL Server, etc.
4. **Import data** to production database

## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [SQL Server Provider](SqlServer.md)
- [MySQL Provider](MySQL.md)