# Supabase Database Provider

<Ingress>
Connect your Ivy application to Supabase with automatic Entity Framework configuration for PostgreSQL.
</Ingress>

## Overview

Supabase is an open-source Firebase alternative that provides a PostgreSQL database with real-time capabilities, authentication, and storage. Ivy integrates seamlessly with Supabase's PostgreSQL backend.

## Connection String Format

```text
Host=your-project.supabase.co;Database=postgres;Username=postgres;Password=your-password
```

### Getting Supabase Connection Details

1. **Go to your Supabase project dashboard**
2. **Navigate to Settings > Database**
3. **Copy the connection details:**
   - Host: `db.your-project-ref.supabase.co`
   - Database: `postgres`
   - Username: `postgres`
   - Password: Your database password

### Alternative Connection String Formats

**With SSL (Recommended)**
```text
Host=db.your-project-ref.supabase.co;Database=postgres;Username=postgres;Password=your-password;SSL Mode=Require
```

**With Connection Pooling**
```text
Host=db.your-project-ref.supabase.co;Database=postgres;Username=postgres;Password=your-password;Pooling=true;Maximum Pool Size=20
```

## Configuration

Ivy automatically configures the **Npgsql.EntityFrameworkCore.PostgreSQL** package for Supabase connections since Supabase uses PostgreSQL under the hood.

## Supabase-Specific Features

### Real-time Subscriptions

While Entity Framework handles data access, you can also use Supabase's real-time features:

```csharp
// Subscribe to database changes (requires Supabase client library)
var supabase = new Supabase.Client("your-url", "your-anon-key");
await supabase.Realtime.Connect();
```

### Row Level Security (RLS)

Supabase supports PostgreSQL's Row Level Security. Configure RLS policies in your Supabase dashboard:

1. **Go to Authentication > Policies**
2. **Create policies** for your tables
3. **Test policies** in the SQL editor

### Built-in Authentication Integration

When using both Supabase database and authentication:

```terminal
>ivy db add --provider Supabase --name MySupabase
>ivy auth add --provider Supabase
```

## Security Best Practices

- **Use SSL connections** (enabled by default with Supabase)
- **Implement Row Level Security** policies for user data isolation
- **Use service role key** only for administrative operations
- **Monitor database activity** through Supabase dashboard

## Troubleshooting

### Common Issues

**Connection Timeout**
- Verify your Supabase project is active (not paused)
- Check your internet connection
- Ensure you're using the correct host URL from Settings > Database

**Authentication Failed**
- Verify your database password is correct
- Check if you're using the database password (not project password)
- Reset database password in Supabase dashboard if needed

**SSL Certificate Issues**
- Use `SSL Mode=Require` in connection string
- Ensure your system has up-to-date certificate stores

**Rate Limiting**
- Supabase has connection limits based on your plan
- Implement connection pooling to optimize usage

## Migration from Local PostgreSQL

If you're migrating from local PostgreSQL to Supabase:

1. **Export your local database** using `pg_dump`
2. **Import to Supabase** using the SQL editor or `psql`
3. **Update connection strings** to point to Supabase
4. **Test authentication flows** if using Supabase Auth

## Example Usage

```csharp
// In your Ivy app with Supabase integration
public class ProfileApp : AppBase<UserProfile>
{
    public override Task<IView> BuildAsync(UserProfile profile)
    {
        return Task.FromResult<IView>(
            Card(
                Avatar(profile.AvatarUrl),
                Text($"Welcome, {profile.DisplayName}!"),
                Text($"Member since: {profile.CreatedAt:MMMM yyyy}"),
                profile.IsEmailVerified
                    ? Badge("Verified", Colors.Green)
                    : Badge("Unverified", Colors.Orange)
            )
        );
    }
}
```

## Combining with Supabase Auth

When using both Supabase database and authentication, you can leverage user context:

```csharp
public class UserDataApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync(); // From Supabase Auth
        var userPosts = await GetUserPostsAsync(user.Id);

        return View(userPosts.Select(post =>
            Card(
                Text(post.Title),
                Text(post.Content),
                Text($"Posted: {post.CreatedAt:yyyy-MM-dd}")
            )
        ));
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [Supabase Authentication](../04_Authentication/Supabase.md)
- [PostgreSQL Provider](PostgreSQL.md)