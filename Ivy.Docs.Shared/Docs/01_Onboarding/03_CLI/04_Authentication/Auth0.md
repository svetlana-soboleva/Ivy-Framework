# Auth0 Authentication Provider

<Ingress>
Secure your Ivy application with Auth0's universal authentication platform supporting multiple identity providers and social logins.
</Ingress>

## Overview

Auth0 is a universal authentication and authorization platform that provides secure authentication for applications. It supports multiple identity providers, social logins, multi-factor authentication, and enterprise integrations.

## Setup

### Adding Auth0 Authentication

```terminal
>ivy auth add --provider Auth0
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Auth0 Domain**: Your Auth0 tenant domain (e.g., `your-project.auth0.com`)
2. **Client ID**: Your Auth0 application's client ID
3. **Client Secret**: Your Auth0 application's client secret
4. **Audience**: API identifier (optional, for API authentication)

## Auth0 Application Setup

Before configuring Ivy, set up your application in the Auth0 Dashboard:

### Step 1: Create Application

1. **Log into Auth0 Dashboard** at https://manage.auth0.com
2. **Navigate to Applications**
3. **Create Application** and select "Regular Web Applications"
4. **Configure Application Settings**:
   - **Allowed Callback URLs**: `https://your-app.com/callback`
   - **Allowed Logout URLs**: `https://your-app.com`
   - **Allowed Web Origins**: `https://your-app.com`

### Step 2: Get Configuration Values

In your Auth0 application settings, note:
- **Domain**: `your-project.auth0.com`
- **Client ID**: Found in the application details
- **Client Secret**: Found in the application details

## Connection String Format

```text
Domain=your-domain.auth0.com;ClientId=your-client-id;ClientSecret=your-client-secret
```

### With API Audience (for API authentication)

```text
Domain=your-domain.auth0.com;ClientId=your-client-id;ClientSecret=your-client-secret;Audience=https://your-api.com
```

## Configuration

### Ivy Integration

Ivy automatically configures Auth0 authentication in your `Program.cs`:

```csharp
server.UseAuth<Auth0AuthProvider>(c => c.UseEmailPassword().UseGoogle().UseApple());
```

### Available Authentication Methods

Configure which authentication methods to enable:

**Email/Password Authentication**
```csharp
server.UseAuth<Auth0AuthProvider>(c => c.UseEmailPassword());
```

**Social Logins**
```csharp
server.UseAuth<Auth0AuthProvider>(c =>
    c.UseGoogle()
     .UseApple()
     .UseFacebook()
     .UseTwitter()
);
```

**Enterprise Connections**
```csharp
server.UseAuth<Auth0AuthProvider>(c =>
    c.UseMicrosoftEntra()
     .UseActiveDirectory()
     .UseSAML()
);
```

## Authentication Flow

### OAuth2 / OpenID Connect Flow

1. **User clicks "Login"** in your Ivy application
2. **Redirect to Auth0** with appropriate parameters
3. **User authenticates** with chosen method (email/password, social, etc.)
4. **Auth0 redirects back** to your application with authorization code
5. **Ivy exchanges code** for access and ID tokens
6. **User is authenticated** and can access protected resources

## Advanced Configuration

### Custom Domains

If using Auth0 Custom Domains:

```text
Domain=login.yourdomain.com;ClientId=your-client-id;ClientSecret=your-client-secret
```

### Multi-Factor Authentication

Enable MFA in Auth0 Dashboard:
1. **Go to Security > Multi-factor Auth**
2. **Choose MFA providers** (SMS, Email, Authenticator apps)
3. **Configure policies** for when MFA is required

### Social Connections

Configure social identity providers in Auth0 Dashboard:

1. **Go to Authentication > Social**
2. **Choose providers** (Google, Facebook, Apple, etc.)
3. **Configure each provider** with their respective credentials

## Security Best Practices

- **Use HTTPS** in production environments
- **Store secrets securely** in User Secrets or environment variables
- **Enable MFA** for sensitive applications
- **Configure proper scopes** to limit access
- **Use refresh tokens** for long-lived sessions
- **Monitor authentication logs** in Auth0 Dashboard
- **Implement proper logout** to clear sessions

## User Management

### Accessing User Information

```csharp
public class UserProfileApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();

        return Card(
            Text($"Welcome, {user.Name}!"),
            Text($"Email: {user.Email}"),
            user.IsEmailVerified
                ? Badge("Verified", Colors.Green)
                : Badge("Unverified", Colors.Orange),
            Button("Logout", LogoutAsync)
        );
    }
}
```

### User Roles and Permissions

Configure roles in Auth0 and access them in Ivy:

```csharp
public class AdminApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();

        if (!user.HasRole("admin"))
        {
            return Error("Access denied. Admin role required.");
        }

        return AdminDashboard();
    }
}
```

## Troubleshooting

### Common Issues

**Invalid Client Credentials**
- Verify Client ID and Client Secret are correct
- Check that credentials haven't been regenerated in Auth0 Dashboard
- Ensure connection string format is correct

**Callback URL Mismatch**
- Verify Allowed Callback URLs in Auth0 Dashboard
- Check that the callback URL matches your application URL
- Ensure HTTPS is used in production

**CORS Issues**
- Configure Allowed Web Origins in Auth0 Dashboard
- Verify your application domain is included
- Check browser developer tools for CORS errors

**Token Validation Errors**
- Verify the audience claim if using API authentication
- Check that token hasn't expired
- Ensure clock synchronization between servers

### Performance Optimization

**Token Caching**
- Implement proper token caching to reduce Auth0 API calls
- Use refresh tokens for session management
- Cache user profile information appropriately

**Connection Pooling**
- Configure HTTP client properly for Auth0 API calls
- Implement retry policies for network issues

## Migration from Other Providers

Auth0 supports importing users from other systems:

1. **Export users** from current system
2. **Use Auth0 User Import** feature
3. **Test authentication flows** thoroughly
4. **Update application configuration** to use Auth0

## Example Implementation

```csharp
public class LoginApp : AppBase
{
    public override Task<IView> BuildAsync()
    {
        return Task.FromResult<IView>(
            Card(
                Text("Welcome to Our Application"),
                Button("Login with Email", () => LoginWithEmail()),
                Button("Login with Google", () => LoginWithGoogle()),
                Button("Login with Apple", () => LoginWithApple())
            )
        );
    }

    private async Task LoginWithEmail()
    {
        // Ivy handles the Auth0 redirect automatically
        await LoginAsync("email");
    }

    private async Task LoginWithGoogle()
    {
        await LoginAsync("google");
    }

    private async Task LoginWithApple()
    {
        await LoginAsync("apple");
    }
}
```

## Related Documentation

- [Authentication Overview](01_Overview.md)
- [Supabase Authentication](Supabase.md)
- [Microsoft Entra Provider](MicrosoftEntra.md)
- [User Management](../../02_Concepts/Services.md)