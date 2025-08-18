# Basic Authentication Provider

<Ingress>
Secure your Ivy application with simple username/password authentication using HTTP Basic Auth.
</Ingress>

## Overview

Basic Authentication is a simple authentication scheme built into the HTTP protocol. It uses a username and password combination transmitted in base64 encoding. While simple to implement, it should be used with HTTPS in production environments.

## Setup

### Adding Basic Authentication

```terminal
>ivy auth add --provider Basic
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Username**: The username for authentication
2. **Password**: The password for authentication
3. **Realm**: Optional realm name (defaults to "Ivy Application")

## Connection String Format

```text
Username=admin;Password=secure-password
```

### With Custom Realm

```text
Username=admin;Password=secure-password;Realm=My Application
```

### Multiple Users

For multiple users, use semicolon-separated pairs:

```text
Username=admin;Password=admin-pass;Username2=user;Password2=user-pass
```

## Configuration

### Ivy Integration

Ivy automatically configures Basic Authentication in your `Program.cs`:

```csharp
server.UseAuth<BasicAuthProvider>();
```

### Custom Configuration

```csharp
server.UseAuth<BasicAuthProvider>(c => 
    c.WithRealm("My Secure Application")
     .WithUsers(new Dictionary<string, string>
     {
         { "admin", "secure-admin-password" },
         { "user", "user-password" },
         { "readonly", "readonly-password" }
     })
);
```

## Authentication Flow

### HTTP Basic Auth Flow

1. **User accesses protected resource**
2. **Server responds with 401 Unauthorized** and `WWW-Authenticate: Basic realm="..."`
3. **Browser prompts for credentials**
4. **User enters username and password**
5. **Browser sends credentials** in `Authorization: Basic base64(username:password)` header
6. **Server validates credentials** and grants access

## Advanced Configuration

### File-based Users

Store users in a configuration file:

```json
{
  "Users": {
    "admin": "hashed-password",
    "user1": "hashed-password",
    "user2": "hashed-password"
  }
}
```

```csharp
server.UseAuth<BasicAuthProvider>(c => 
    c.LoadUsersFromFile("users.json")
);
```

### Password Hashing

Use hashed passwords instead of plain text:

```csharp
server.UseAuth<BasicAuthProvider>(c => 
    c.WithHashedPasswords()
     .WithUsers(new Dictionary<string, string>
     {
         { "admin", BCrypt.Net.BCrypt.HashPassword("secure-password") }
     })
);
```

### Role-based Access

Implement role-based access control:

```csharp
public class UserManager
{
    private static readonly Dictionary<string, string[]> UserRoles = new()
    {
        { "admin", new[] { "admin", "user", "readonly" } },
        { "manager", new[] { "user", "readonly" } },
        { "viewer", new[] { "readonly" } }
    };
    
    public static string[] GetUserRoles(string username)
    {
        return UserRoles.TryGetValue(username, out var roles) ? roles : Array.Empty<string>();
    }
}
```

## Security Considerations

### Production Security

**⚠️ Important Security Notes:**
- **Always use HTTPS** in production
- **Never use Basic Auth over HTTP** (credentials are easily intercepted)
- **Use strong passwords** and consider password policies
- **Implement rate limiting** to prevent brute force attacks
- **Log authentication attempts** for security monitoring

### Password Security

```csharp
// Use strong password requirements
server.UseAuth<BasicAuthProvider>(c => 
    c.WithPasswordPolicy(policy => policy
        .RequireMinimumLength(12)
        .RequireUppercase()
        .RequireLowercase()
        .RequireDigits()
        .RequireSpecialCharacters()
    )
);
```

### Rate Limiting

Implement rate limiting to prevent brute force attacks:

```csharp
server.UseAuth<BasicAuthProvider>(c => 
    c.WithRateLimit(maxAttempts: 5, windowMinutes: 15)
);
```

## User Management

### Accessing User Information

```csharp
public class DashboardApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();
        
        if (user == null)
        {
            return Redirect("/login");
        }
        
        return Card(
            Text($"Welcome, {user.Username}!"),
            Text($"Authentication Method: Basic Auth"),
            Text($"Session Started: {user.LoginTime:yyyy-MM-dd HH:mm}"),
            Button("Sign Out", SignOutAsync)
        );
    }
}
```

### Role-based Access Control

```csharp
public class AdminApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();
        var roles = UserManager.GetUserRoles(user.Username);
        
        if (!roles.Contains("admin"))
        {
            return Error("Access denied. Administrator privileges required.");
        }
        
        return AdminDashboard();
    }
}
```

## Use Cases

### Development and Testing

Basic Auth is ideal for:
- **Development environments** where simplicity is key
- **Internal tools** with limited user bases
- **API endpoints** requiring simple authentication
- **Proof of concepts** and prototypes

### Quick Prototyping

```csharp
public class PrototypeApp : AppBase
{
    public override Task<IView> BuildAsync()
    {
        return Task.FromResult<IView>(
            Card(
                Text("Prototype Dashboard"),
                Text("This is a quick prototype with basic authentication."),
                
                Grid(
                    Card("Users", Text("5 active users")),
                    Card("Revenue", Text("$1,234")),
                    Card("Orders", Text("42 today"))
                )
            )
        );
    }
}
```

## Limitations

### Technical Limitations

- **No encryption** of credentials (mitigated by HTTPS)
- **No password reset** functionality built-in
- **No session management** (stateless)
- **Limited scalability** for large user bases
- **No OAuth/SSO integration**

### Browser Behavior

- **Browser caching** of credentials (can't programmatically clear)
- **No custom login UI** (uses browser's built-in prompt)
- **Limited logout options** (browser-dependent)

## Migration Paths

### From Basic Auth to OAuth

When you outgrow Basic Auth:

1. **Keep Basic Auth** running during transition
2. **Add OAuth provider** (Auth0, Microsoft Entra, etc.)
3. **Update applications** to use OAuth
4. **Migrate users** to new system
5. **Deprecate Basic Auth** once migration is complete

### Hybrid Approach

Support multiple authentication methods:

```csharp
server.UseAuth<MultiAuthProvider>(c => 
    c.UseBasicAuth()
     .UseAuth0()
     .UseMicrosoftEntra()
);
```

## Troubleshooting

### Common Issues

**Credentials Not Working**
- Verify username and password are correct
- Check for case sensitivity
- Ensure special characters are properly handled

**Browser Not Prompting**
- Clear browser cache and credentials
- Check if already authenticated
- Verify WWW-Authenticate header is sent

**401 Errors After Success**
- Check session management configuration
- Verify authentication middleware order
- Ensure credentials are being passed correctly

**Performance Issues**
- Implement credential caching if validating against external systems
- Consider connection pooling for database lookups
- Monitor authentication response times

## Example Implementation

```csharp
public class SimpleApp : AppBase
{
    public override Task<IView> BuildAsync()
    {
        return Task.FromResult<IView>(
            Card(
                Text("Simple Authenticated App"),
                Text("You have successfully authenticated with Basic Auth."),
                
                Text("Available Features:"),
                List(
                    "View dashboard",
                    "Manage settings",
                    "Export data"
                ),
                
                Button("Dashboard", () => NavigateToDashboard()),
                Button("Settings", () => NavigateToSettings()),
                Button("Sign Out", () => SignOutAsync())
            )
        );
    }
}

// Program.cs configuration
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIvy();

var app = builder.Build();

app.UseIvy(server => 
{
    server.UseAuth<BasicAuthProvider>(auth => 
        auth.WithUsers(new Dictionary<string, string>
        {
            { "admin", "secure-password" },
            { "demo", "demo-password" }
        })
        .WithRealm("Simple App")
    );
    
    server.UseApp<SimpleApp>();
});

app.Run();
```

## Related Documentation

- [Authentication Overview](../04_Auth.md)
- [Auth0 Provider](Auth0.md)
- [Supabase Provider](Supabase.md)
- [Security Best Practices](../../02_Concepts/Services.md)