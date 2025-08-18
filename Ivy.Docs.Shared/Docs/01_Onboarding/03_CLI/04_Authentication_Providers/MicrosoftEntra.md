# Microsoft Entra Authentication Provider

<Ingress>
Secure your Ivy application with Microsoft Entra ID (formerly Azure Active Directory) for enterprise identity and access management.
</Ingress>

## Overview

Microsoft Entra ID is Microsoft's cloud-based identity and access management service. It provides secure authentication for enterprise applications, supports single sign-on (SSO), multi-factor authentication, and integrates with Microsoft 365 and other Microsoft services.

## Setup

### Adding Microsoft Entra Authentication

```terminal
>ivy auth add --provider MicrosoftEntra
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Tenant ID**: Your Microsoft Entra tenant ID
2. **Client ID**: Your application's client ID (Application ID)
3. **Client Secret**: Your application's client secret
4. **Authority**: The authority URL (optional, defaults to common)

## Microsoft Entra Application Setup

Before configuring Ivy, set up your application in the Microsoft Entra admin center:

### Step 1: Register Application

1. **Sign in to Microsoft Entra admin center** (https://entra.microsoft.com)
2. **Navigate to Identity > Applications > App registrations**
3. **Click "New registration"**
4. **Configure application**:
   - **Name**: Your application name
   - **Supported account types**: Choose appropriate option
   - **Redirect URI**: `https://your-app.com/signin-oidc`

### Step 2: Configure Authentication

1. **Go to Authentication** in your app registration
2. **Add platform** > **Web**
3. **Configure redirect URIs**:
   - Add `https://your-app.com/signin-oidc`
   - Add logout URL if needed
4. **Enable ID tokens** under "Implicit grant and hybrid flows"

### Step 3: Create Client Secret

1. **Go to Certificates & secrets**
2. **Click "New client secret"**
3. **Add description** and set expiration
4. **Copy the secret value** (you won't see it again)

### Step 4: Get Configuration Values

Note the following from your app registration:
- **Tenant ID**: Found in the Overview section
- **Client ID (Application ID)**: Found in the Overview section  
- **Client Secret**: The value you just created

## Connection String Format

```text
TenantId=your-tenant-id;ClientId=your-client-id;ClientSecret=your-client-secret
```

### With Custom Authority

```text
TenantId=your-tenant-id;ClientId=your-client-id;ClientSecret=your-client-secret;Authority=https://login.microsoftonline.com/your-tenant-id
```

### Multi-tenant Applications

```text
TenantId=common;ClientId=your-client-id;ClientSecret=your-client-secret;Authority=https://login.microsoftonline.com/common
```

## Configuration

### Ivy Integration

Ivy automatically configures Microsoft Entra authentication in your `Program.cs`:

```csharp
server.UseAuth<MicrosoftEntraAuthProvider>(c => c.UseMicrosoftEntra());
```

### Account Types Configuration

Configure which account types can sign in:

**Single Tenant (Organization only)**
```csharp
server.UseAuth<MicrosoftEntraAuthProvider>(c => 
    c.UseMicrosoftEntra()
     .WithTenant("your-tenant-id")
);
```

**Multi-tenant (Any organization)**
```csharp
server.UseAuth<MicrosoftEntraAuthProvider>(c => 
    c.UseMicrosoftEntra()
     .WithAuthority("https://login.microsoftonline.com/organizations")
);
```

**Multi-tenant + Personal accounts**
```csharp
server.UseAuth<MicrosoftEntraAuthProvider>(c => 
    c.UseMicrosoftEntra()
     .WithAuthority("https://login.microsoftonline.com/common")
);
```

## Authentication Flow

### OpenID Connect Flow

1. **User clicks "Sign in with Microsoft"**
2. **Redirect to Microsoft Entra** with appropriate parameters
3. **User authenticates** with their Microsoft credentials
4. **Microsoft Entra redirects back** with authorization code
5. **Ivy exchanges code** for access and ID tokens
6. **User is authenticated** with Microsoft identity

## Advanced Configuration

### API Permissions

Configure API permissions in Microsoft Entra admin center:

1. **Go to API permissions**
2. **Add permissions** for Microsoft APIs you need:
   - **Microsoft Graph**: User.Read, Mail.Read, etc.
   - **Custom APIs**: Your own API scopes

### Group and Role Claims

Enable group/role claims in token configuration:

1. **Go to Token configuration**
2. **Add optional claims**
3. **Select ID tokens** and choose:
   - Groups
   - Roles
   - Email
   - Preferred username

### Conditional Access

Configure conditional access policies:
1. **Go to Security > Conditional Access**
2. **Create policies** for your application
3. **Configure requirements** (MFA, device compliance, etc.)

## Security Best Practices

- **Use HTTPS** in production environments
- **Store secrets securely** in Azure Key Vault or User Secrets
- **Enable MFA** through conditional access policies
- **Configure token lifetime** appropriately
- **Monitor sign-in logs** in Microsoft Entra
- **Use managed identities** when running on Azure
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
            Text($"Job Title: {user.JobTitle}"),
            Text($"Department: {user.Department}"),
            user.Groups?.Any() == true
                ? Text($"Groups: {string.Join(", ", user.Groups)}")
                : Empty(),
            Button("Sign Out", SignOutAsync)
        );
    }
}
```

### Role-based Access Control

Use Microsoft Entra roles in your application:

```csharp
public class AdminApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();
        
        if (!user.IsInRole("Global Administrator"))
        {
            return Error("Access denied. Administrator role required.");
        }
        
        return AdminDashboard();
    }
}
```

### Microsoft Graph Integration

Access Microsoft Graph APIs with user context:

```csharp
public class EmailApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var graphClient = await GetGraphClientAsync();
        var messages = await graphClient.Me.Messages
            .Request()
            .Top(10)
            .GetAsync();
        
        return List(messages.Select(msg =>
            Card(
                Text($"From: {msg.From?.EmailAddress?.Name}"),
                Text($"Subject: {msg.Subject}"),
                Text($"Received: {msg.ReceivedDateTime:yyyy-MM-dd HH:mm}")
            )
        ));
    }
}
```

## Troubleshooting

### Common Issues

**Invalid Client Credentials**
- Verify Client ID and Client Secret are correct
- Check that the client secret hasn't expired
- Ensure the secret value (not ID) is being used

**Redirect URI Mismatch**
- Verify redirect URIs in Microsoft Entra match your application URLs
- Check for case sensitivity in URLs
- Ensure HTTPS is used in production

**Insufficient Privileges**
- Check API permissions are granted in Microsoft Entra
- Ensure admin consent is provided for application permissions
- Verify user has necessary roles/groups

**Token Validation Errors**
- Verify audience and issuer claims
- Check token signature validation
- Ensure system clock is synchronized

**Conditional Access Blocking**
- Check conditional access policies in Microsoft Entra
- Verify device compliance if required
- Ensure MFA requirements are met

### Performance Optimization

**Token Caching**
- Implement proper token caching to reduce Microsoft Entra calls
- Use refresh tokens for session management
- Cache Microsoft Graph responses appropriately

**Connection Management**
- Configure HTTP client timeouts appropriately
- Implement retry policies for network issues
- Use connection pooling for Microsoft Graph calls

## Azure Integration

### Managed Identity

When running on Azure, use managed identity:

```csharp
// Configure managed identity in Azure
server.UseAuth<MicrosoftEntraAuthProvider>(c => 
    c.UseManagedIdentity()
);
```

### Azure Key Vault

Store secrets in Azure Key Vault:

```text
# In Key Vault
ClientSecret=your-actual-secret

# In connection string
TenantId=your-tenant-id;ClientId=your-client-id;ClientSecret=@Microsoft.KeyVault(SecretUri=https://vault.vault.azure.net/secrets/ClientSecret/)
```

## Migration Considerations

### From Azure AD Graph to Microsoft Graph

If migrating from older implementations:
- Update API calls to Microsoft Graph endpoints
- Review permission scopes (may have changed)
- Test all Graph API integrations thoroughly

### Multi-factor Authentication

Enable MFA through conditional access:
1. Create conditional access policy
2. Target your application
3. Require MFA for specific scenarios
4. Test authentication flows

## Example Implementation

```csharp
public class WorkspaceApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();
        
        if (user == null)
        {
            return Card(
                Text("Welcome to the Workspace"),
                Button("Sign in with Microsoft", () => LoginAsync("microsoft"))
            );
        }
        
        return Card(
            Text($"Welcome back, {user.GivenName}!"),
            Text($"Organization: {user.CompanyName}"),
            
            Tabs(
                Tab("Profile", await BuildProfileTab()),
                Tab("Calendar", await BuildCalendarTab()),
                Tab("Email", await BuildEmailTab())
            ),
            
            Button("Sign Out", SignOutAsync)
        );
    }
    
    private async Task<IView> BuildProfileTab()
    {
        var user = await GetCurrentUserAsync();
        
        return Card(
            Text($"Display Name: {user.DisplayName}"),
            Text($"Job Title: {user.JobTitle}"),
            Text($"Department: {user.Department}"),
            Text($"Office Location: {user.OfficeLocation}")
        );
    }
}
```

## Related Documentation

- [Authentication Overview](../04_Auth.md)
- [Auth0 Provider](Auth0.md)
- [Authelia Provider](Authelia.md)
- [Enterprise Features](../../02_Concepts/Services.md)