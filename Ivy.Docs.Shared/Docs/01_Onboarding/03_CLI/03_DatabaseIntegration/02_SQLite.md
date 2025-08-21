---
title: SQLite
---

# SQLite Database Provider

<Ingress>
Connect your Ivy application to SQLite with automatic Entity Framework configuration.
</Ingress>

## Overview

SQLite is a lightweight, file-based relational database that's perfect for development, testing, and applications that need a simple, self-contained database solution. No server setup required! Learn more about SQLite at the [official SQLite website](https://www.sqlite.org/).

## Database File Selection

Unlike other providers, SQLite doesn't use connection strings stored in user secrets. When setting up SQLite with Ivy, you'll be prompted for the path to your database file:

```terminal
Path to database file: data.db
```

Ivy will attempt to suggest existing SQLite files in your project, but you can specify any path. 

The connection string will be automatically generated as:

```text
Data Source=data.db
```

Ivy automatically adds the database file to your project with `<CopyToOutputDirectory>Always</CopyToOutputDirectory>` so it's included when building your project.

### Common Configurations

**In-Memory Database (for testing)**
```text
Data Source=:memory:
```

See [SQLite Connection Strings](https://www.connectionstrings.com/sqlite/) for additional options.

## Configuration

Ivy automatically configures the **Microsoft.EntityFrameworkCore.Sqlite** package for SQLite connections.

SQLite database files are referenced directly in your project file rather than stored as connection strings in User Secrets.

## SQLite-Specific Features

Key advantages:
- **Zero-configuration** - no server setup
- **Cross-platform** database files
- **Full-text search** and **JSON support**

See [SQLite Features](https://www.sqlite.org/features.html) for details.

## Security Best Practices

- **Secure file permissions**
- **Use WAL mode**: `PRAGMA journal_mode=WAL`
- **Enable foreign keys**: `PRAGMA foreign_keys=ON`

See [SQLite Security Considerations](https://www.sqlite.org/security.html) for more.

## Troubleshooting

### Common Issues

**File Access Issues**
- Check read/write permissions and directory existence
- Ensure file isn't locked by another process

**Database Locked Errors**
- Close connections properly and use WAL mode

See [SQLite Troubleshooting](https://www.sqlite.org/faq.html) for more help.



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


## Related Documentation

- [Database Overview](01_Overview.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [SQL Server Provider](SqlServer.md)
- [MySQL Provider](MySQL.md)
- [Official SQLite Documentation](https://www.sqlite.org/docs.html)
- [EF Core SQLite Provider](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/)