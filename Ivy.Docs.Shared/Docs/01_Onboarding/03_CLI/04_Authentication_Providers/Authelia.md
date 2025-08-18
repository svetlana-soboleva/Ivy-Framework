# Authelia Authentication Provider

<Ingress>
Secure your Ivy application with Authelia's open-source identity provider supporting multi-factor authentication and forward auth.
</Ingress>

## Overview

Authelia is an open-source identity provider that provides multi-factor authentication, single sign-on, and access control. It's designed to work with reverse proxies and supports various authentication backends including LDAP, file-based users, and OAuth2/OIDC.

## Setup

### Adding Authelia Authentication

```terminal
>ivy auth add --provider Authelia
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Base URL**: Your Authelia instance URL (e.g., `https://auth.yourdomain.com`)
2. **Client ID**: OAuth2 client identifier
3. **Client Secret**: OAuth2 client secret
4. **Scope**: OAuth2 scopes (optional, defaults to openid profile email)

## Authelia Server Setup

Before configuring Ivy, you need a running Authelia instance:

### Step 1: Install Authelia

**Docker Compose (Recommended)**
```yaml
version: '3.7'
services:
  authelia:
    image: authelia/authelia:latest
    container_name: authelia
    volumes:
      - ./authelia:/config
    ports:
      - "9091:9091"
    environment:
      - AUTHELIA_JWT_SECRET=your-jwt-secret
      - AUTHELIA_SESSION_SECRET=your-session-secret
      - AUTHELIA_STORAGE_ENCRYPTION_KEY=your-storage-key
```

**Binary Installation**
```bash
# Download from GitHub releases
wget https://github.com/authelia/authelia/releases/latest/download/authelia-linux-amd64.tar.gz
tar -xzf authelia-linux-amd64.tar.gz
./authelia --config /path/to/config.yml
```

### Step 2: Configure Authelia

Create `/config/configuration.yml`:

```yaml
server:
  host: 0.0.0.0
  port: 9091

log:
  level: info

authentication_backend:
  file:
    path: /config/users_database.yml
    password:
      algorithm: argon2id

access_control:
  default_policy: deny
  rules:
    - domain: "*.yourdomain.com"
      policy: one_factor

session:
  name: authelia_session
  domain: yourdomain.com
  same_site: lax
  secret: your-session-secret

regulation:
  max_retries: 3
  find_time: 120
  ban_time: 300

storage:
  local:
    path: /config/db.sqlite3

notifier:
  filesystem:
    filename: /config/notification.txt

identity_providers:
  oidc:
    issuer_private_key: |
      -----BEGIN RSA PRIVATE KEY-----
      # Your RSA private key here
      -----END RSA PRIVATE KEY-----
    
    clients:
      - id: ivy-app
        description: Ivy Application
        secret: '$pbkdf2-sha512$310000$...'  # Hashed secret
        redirect_uris:
          - https://your-app.com/callback
        scopes:
          - openid
          - profile
          - email
        grant_types:
          - authorization_code
        response_types:
          - code
```

### Step 3: Create User Database

Create `/config/users_database.yml`:

```yaml
users:
  john:
    displayname: "John Doe"
    password: '$argon2id$v=19$m=65536,t=3,p=4$...'  # Hashed password
    email: john.doe@example.com
    groups:
      - admins
      - users
  
  jane:
    displayname: "Jane Smith"
    password: '$argon2id$v=19$m=65536,t=3,p=4$...'
    email: jane.smith@example.com
    groups:
      - users
```

## Connection String Format

```text
BaseUrl=https://auth.yourdomain.com;ClientId=ivy-app;ClientSecret=your-client-secret
```

### With Custom Scopes

```text
BaseUrl=https://auth.yourdomain.com;ClientId=ivy-app;ClientSecret=your-client-secret;Scope=openid profile email groups
```

## Configuration

### Ivy Integration

Ivy automatically configures Authelia authentication in your `Program.cs`:

```csharp
server.UseAuth<AutheliaAuthProvider>(c => c.UseAuthelia());
```

### Custom Configuration

```csharp
server.UseAuth<AutheliaAuthProvider>(c => 
    c.UseAuthelia()
     .WithScopes("openid", "profile", "email", "groups")
     .WithAdditionalClaims("groups", "preferred_username")
);
```

## Authentication Flow

### OAuth2/OpenID Connect Flow

1. **User clicks "Sign in"** in your Ivy application
2. **Redirect to Authelia** with OAuth2 parameters
3. **Authelia checks session** (may prompt for login)
4. **User authenticates** with configured method (file, LDAP, etc.)
5. **MFA prompt** if configured and required
6. **Authorization granted** and redirect back to application
7. **Ivy exchanges code** for tokens

## Advanced Configuration

### Multi-Factor Authentication

Configure TOTP (Time-based One-Time Password):

```yaml
# In configuration.yml
totp:
  issuer: yourdomain.com
  algorithm: sha1
  digits: 6
  period: 30
  skew: 1

# Configure MFA policy
access_control:
  rules:
    - domain: "admin.yourdomain.com"
      policy: two_factor
    - domain: "*.yourdomain.com"  
      policy: one_factor
```

### LDAP Integration

Connect to Active Directory or OpenLDAP:

```yaml
authentication_backend:
  ldap:
    url: ldap://ldap.yourdomain.com
    base_dn: dc=yourdomain,dc=com
    username_attribute: uid
    additional_users_dn: ou=users
    users_filter: (&({username_attribute}={input})(objectClass=person))
    additional_groups_dn: ou=groups
    groups_filter: (&(member={dn})(objectClass=groupOfNames))
    group_name_attribute: cn
    mail_attribute: mail
    display_name_attribute: displayName
    user: cn=admin,dc=yourdomain,dc=com
    password: admin-password
```

### Notification Methods

**SMTP Email**
```yaml
notifier:
  smtp:
    username: authelia@yourdomain.com
    password: your-email-password
    host: smtp.gmail.com
    port: 587
    sender: authelia@yourdomain.com
    subject: "[Authelia] {title}"
```

**File-based (Development)**
```yaml
notifier:
  filesystem:
    filename: /config/notifications.txt
```

## Security Best Practices

- **Use HTTPS** for all Authelia communications
- **Generate strong secrets** for JWT and session encryption
- **Enable MFA** for sensitive applications
- **Configure rate limiting** to prevent brute force attacks
- **Use secure password hashing** (argon2id recommended)
- **Monitor logs** for authentication attempts
- **Keep Authelia updated** to latest version
- **Secure configuration files** with appropriate permissions

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
            Text($"Username: {user.PreferredUsername}"),
            user.Groups?.Any() == true
                ? Text($"Groups: {string.join(", ", user.Groups)}")
                : Empty(),
            Button("Sign Out", SignOutAsync)
        );
    }
}
```

### Group-based Authorization

Use Authelia groups for access control:

```csharp
public class AdminApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();
        
        if (!user.Groups?.Contains("admins") == true)
        {
            return Error("Access denied. Admin group membership required.");
        }
        
        return AdminDashboard();
    }
}
```

## Troubleshooting

### Common Issues

**Connection Refused**
- Verify Authelia service is running
- Check firewall settings allow connections to port 9091
- Ensure network connectivity between application and Authelia

**Invalid Client**
- Verify client ID matches configuration in Authelia
- Check client secret is correctly hashed in Authelia config
- Ensure client is properly configured in `identity_providers.oidc.clients`

**Redirect URI Mismatch**
- Verify redirect URIs in Authelia client configuration
- Check case sensitivity and exact URL matching
- Ensure HTTPS URLs in production

**Authentication Failed**
- Check user credentials in users database or LDAP
- Verify password hashing is correct
- Check authentication backend configuration

**MFA Issues**
- Verify TOTP configuration in Authelia
- Check time synchronization between servers
- Ensure MFA policy is correctly configured

### Performance Optimization

**Session Management**
- Configure appropriate session timeout
- Use Redis for session storage in production
- Implement proper session cleanup

**Caching**
- Enable caching for LDAP queries if using LDAP backend
- Configure appropriate cache timeouts
- Use Redis for distributed caching

## Production Deployment

### Reverse Proxy Integration

**Nginx Configuration**
```nginx
server {
    listen 443 ssl http2;
    server_name auth.yourdomain.com;

    ssl_certificate /path/to/cert.pem;
    ssl_certificate_key /path/to/key.pem;

    location / {
        proxy_pass http://authelia:9091;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

**Traefik Configuration**
```yaml
http:
  routers:
    authelia:
      rule: "Host(`auth.yourdomain.com`)"
      tls:
        certResolver: letsencrypt
      service: authelia
  
  services:
    authelia:
      loadBalancer:
        servers:
          - url: "http://authelia:9091"
```

### Database Storage

For production, use PostgreSQL or MySQL:

```yaml
storage:
  postgres:
    host: postgres
    port: 5432
    database: authelia
    username: authelia
    password: your-db-password
```

## Example Implementation

```csharp
public class SecureApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();
        
        if (user == null)
        {
            return Card(
                Text("Secure Application"),
                Text("Please authenticate to access this application."),
                Button("Sign In", () => LoginAsync("authelia"))
            );
        }
        
        var canAccess = await CheckAccess(user);
        if (!canAccess)
        {
            return Error("Access denied. Insufficient permissions.");
        }
        
        return Card(
            Text($"Welcome to Secure App, {user.Name}!"),
            Text($"Your groups: {string.Join(", ", user.Groups ?? new string[0])}"),
            
            user.Groups?.Contains("admins") == true
                ? Button("Admin Panel", () => NavigateToAdmin())
                : Empty(),
            
            Button("Sign Out", SignOutAsync)
        );
    }
    
    private async Task<bool> CheckAccess(IUser user)
    {
        // Implement your access control logic
        return user.Groups?.Contains("users") == true;
    }
}
```

## Related Documentation

- [Authentication Overview](../04_Auth.md)
- [Auth0 Provider](Auth0.md)
- [Microsoft Entra Provider](MicrosoftEntra.md)
- [Basic Authentication](BasicAuth.md)