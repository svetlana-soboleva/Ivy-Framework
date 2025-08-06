# Ivy Authentication

The `ivy auth` commands allow you to add and configure authentication providers in your Ivy project. Ivy supports multiple authentication providers and automatically integrates them with your application.

## Supported Authentication Providers

Ivy supports the following authentication providers:

### Identity Providers

- **Auth0** - Universal authentication platform
- **Supabase Auth** - Built-in authentication for Supabase
- **Authelia** - Open-source identity provider

### Basic Authentication

- **Basic Auth** - Simple username/password authentication

## Basic Usage

### Adding an Authentication Provider

```terminal
>ivy auth add
```

This command will:

- Prompt you to select an authentication provider
- Configure the provider with your application
- Update your `Program.cs` with the necessary authentication setup
- Store sensitive configuration in .NET User Secrets

## Command Options

### `ivy auth add` Options

#### `--provider <PROVIDER>`

Specify the authentication provider directly:

```terminal
>ivy auth add --provider Auth0
```

Available providers: `Auth0`, `Supabase`, `Authelia`, `Basic`

#### `--connection-string <CONNECTION_STRING>`

Provide provider-specific configuration using connection string syntax:

```terminal
>ivy auth add --provider Auth0 --connection-string YourConnectionString
```

#### `--verbose`

Enable verbose output for detailed logging:

```terminal
>ivy auth add --verbose
```

## Interactive Mode

When you run `ivy auth add` without specifying options, Ivy will guide you through an interactive setup:

1. **Select Authentication Provider**: Choose from the available providers
2. **Provider Configuration**: Enter the necessary configuration details
3. **Integration Setup**: Ivy will automatically configure your application

## Authentication Provider Configuration

### Auth0

Auth0 is a universal authentication platform that supports multiple identity providers.

#### Setup Process

```terminal
>ivy auth add --provider Auth0
```

#### Required Configuration

- **Domain**: Your Auth0 domain (e.g., `your-app.auth0.com`)
- **Client ID**: Your Auth0 application client ID
- **Client Secret**: Your Auth0 application client secret

#### Connection String Format

```text
Domain=your-domain.auth0.com;ClientId=your-client-id;ClientSecret=your-client-secret
```

#### Auth0 Application Setup

1. Create an application in your Auth0 dashboard
2. Set the callback URL to: `https://your-app.com/callback`
3. Configure the allowed logout URLs
4. Note your Domain, Client ID, and Client Secret

### Supabase Auth

Supabase Auth provides built-in authentication for Supabase projects.

#### Setup Process

```terminal
>ivy auth add --provider Supabase
```

#### Required Configuration

- **Project URL**: Your Supabase project URL
- **Anon Key**: Your Supabase anonymous key
- **Service Role Key**: Your Supabase service role key (optional)

#### Connection String Format

```text
ProjectUrl=https://your-project.supabase.co;AnonKey=your-anon-key;ServiceRoleKey=your-service-role-key
```

#### Supabase Project Setup

1. Create a project in Supabase
2. Go to Settings > API
3. Copy your Project URL and anon key
4. Optionally copy your service role key for admin operations

### Authelia

Authelia is an open-source identity provider that can integrate with various authentication systems.

#### Setup Process

```terminal
>ivy auth add --provider Authelia
```

#### Required Configuration

- **Base URL**: Your Authelia instance URL
- **Client ID**: Your application client ID
- **Client Secret**: Your application client secret

#### Connection String Format

```text
BaseUrl=https://auth.your-domain.com;ClientId=your-client-id;ClientSecret=your-client-secret
```

#### Authelia Setup

1. Install and configure Authelia
2. Create an OAuth2 client for your application
3. Configure the redirect URIs
4. Note your Base URL, Client ID, and Client Secret

### Basic Auth

Basic authentication provides simple username/password authentication.

#### Setup Process

```terminal
>ivy auth add --provider Basic
```

#### Required Configuration

- **Username**: Default username for authentication
- **Password**: Default password for authentication

#### Connection String Format

```text
Username=admin;Password=secure-password
```

## Security and Secrets Management

Ivy automatically configures .NET User Secrets for secure authentication configuration:

```terminal
>dotnet user-secrets list
```

### Environment Variables

You can also use environment variables for authentication configuration:

```text
export Auth0__Domain="your-domain.auth0.com"
export Auth0__ClientId="your-client-id"
export Auth0__ClientSecret="your-client-secret"
```

## Program.cs Integration

Ivy automatically updates your `Program.cs` to include authentication:

### Auth0 Integration

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:ClientId"];
});

// Add Ivy services
builder.Services.AddIvy();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseIvy();
app.Run();
```

### Supabase Auth Integration

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Supabase authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Supabase:ProjectUrl"];
    options.Audience = "authenticated";
});

// Add Ivy services
builder.Services.AddIvy();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseIvy();
app.Run();
```

## Multiple Authentication Providers

You can configure multiple authentication providers in a single application:

```terminal
>ivy auth add --provider Auth0
>ivy auth add --provider Basic
```

## Authentication Flow

### OAuth2 Flow (Auth0, Supabase, Authelia)

1. User visits your application
2. User clicks "Login" and is redirected to the identity provider
3. User authenticates with the identity provider
4. User is redirected back to your application with an authorization code
5. Your application exchanges the code for an access token
6. The access token is used to authenticate API requests

### Basic Auth Flow

1. User visits your application
2. Browser prompts for username/password
3. Credentials are validated against your configuration
4. If valid, user is authenticated for the session

## Authorization

### Role-Based Authorization

Configure roles in your authentication provider and use them in your application:

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    // Admin-only endpoints
}
```

### Policy-Based Authorization

Create custom authorization policies:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PremiumUser", policy =>
        policy.RequireClaim("subscription", "premium"));
});
```

## Troubleshooting

### Authentication Provider Issues

- Verify your provider configuration is correct
- Check that your application is properly registered with the identity provider
- Ensure callback URLs are correctly configured
- Verify network connectivity to the authentication provider

### Token Validation Issues

- Check that your JWT tokens are properly signed
- Verify audience and issuer claims
- Ensure your application's clock is synchronized

### Configuration Issues

- Ensure authentication settings are properly stored in user secrets
- Verify environment variables are correctly set
- Check that your `Program.cs` includes the necessary authentication middleware

## Examples

### Auth0 Setup

```terminal
>ivy auth add --provider Auth0 --connection-string YourConnectionString
```

### Supabase Auth Setup

```terminal
>ivy auth add --provider Supabase --connection-string YourConnectionString
```

### Basic Auth Setup

```terminal
>ivy auth add --provider Basic --connection-string YourConnectionString
```

### Multiple Providers

```terminal
>ivy auth add --provider Auth0
>ivy auth add --provider Basic
```

## Best Practices

### Security

- Always use HTTPS in production
- Store sensitive configuration in user secrets or environment variables
- Regularly rotate client secrets
- Use strong passwords for Basic Auth
- Implement proper session management

### Configuration

- Use descriptive names for your authentication providers
- Keep configuration separate from code
- Use environment-specific settings
- Document your authentication setup

### Testing

- Test authentication flows in development
- Verify token validation works correctly
- Test authorization policies
- Ensure logout functionality works properly

## Related Commands

- `ivy init` - Initialize a new Ivy project
- `ivy db add` - Add database connections
- `ivy app create` - Create applications
- `ivy deploy` - Deploy your application 