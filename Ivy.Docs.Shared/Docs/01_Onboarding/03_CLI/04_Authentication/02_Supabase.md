# Supabase Authentication Provider

<Ingress>
Secure your Ivy application with Supabase's built-in authentication supporting email/password, social logins, and magic links.
</Ingress>

## Overview

Supabase Auth provides a complete authentication system built on top of PostgreSQL. It offers email/password authentication, social logins, magic links, and integrates seamlessly with Supabase's database and real-time features.

## Setup

### Adding Supabase Authentication

```terminal
>ivy auth add --provider Supabase
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Project URL**: Your Supabase project URL (e.g., `https://your-project.supabase.co`)
2. **Anon Key**: Your Supabase project's anonymous key
3. **Service Role Key**: Your service role key (optional, for admin operations)

## Supabase Project Setup

Before configuring Ivy, ensure your Supabase project is properly set up:

### Step 1: Create Supabase Project

1. **Sign up at Supabase** (https://supabase.com)
2. **Create a new project**
3. **Wait for the database** to be ready

### Step 2: Get Configuration Values

In your Supabase project dashboard:
1. **Go to Settings > API**
2. **Note your Project URL**: `https://your-project.supabase.co`
3. **Copy your anon key**: Used for client-side authentication
4. **Copy your service role key**: Used for admin operations (optional)

### Step 3: Configure Authentication Settings

1. **Go to Authentication > Settings**
2. **Configure Site URL**: Set your application's URL
3. **Add Redirect URLs**: Add allowed callback URLs
4. **Configure Email Settings**: Set up email templates and SMTP (optional)

## Connection String Format

```text
ProjectUrl=https://your-project.supabase.co;AnonKey=your-anon-key;ServiceRoleKey=your-service-role-key
```

### Minimal Configuration (Client-side only)

```text
ProjectUrl=https://your-project.supabase.co;AnonKey=your-anon-key
```

## Configuration

### Ivy Integration

Ivy automatically configures Supabase authentication in your `Program.cs`:

```csharp
server.UseAuth<SupabaseAuthProvider>(c => c.UseEmailPassword().UseGoogle().UseApple());
```

### Available Authentication Methods

Configure which authentication methods to enable:

**Email/Password Authentication**
```csharp
server.UseAuth<SupabaseAuthProvider>(c => c.UseEmailPassword());
```

**Magic Links**
```csharp
server.UseAuth<SupabaseAuthProvider>(c => c.UseMagicLink());
```

**Social Logins**
```csharp
server.UseAuth<SupabaseAuthProvider>(c =>
    c.UseGoogle()
     .UseApple()
     .UseFacebook()
     .UseGitHub()
);
```

## Social Provider Configuration

Each social provider requires setup in both Supabase and the provider's platform:

### Google OAuth

**In Google Cloud Console:**
1. Create OAuth 2.0 credentials
2. Add authorized redirect URIs: `https://your-project.supabase.co/auth/v1/callback`

**In Supabase Dashboard:**
1. **Go to Authentication > Providers**
2. **Enable Google provider**
3. **Add Client ID and Client Secret**

### Apple OAuth

**In Apple Developer Console:**
1. Create App ID and Services ID
2. Configure Sign in with Apple

**In Supabase Dashboard:**
1. **Enable Apple provider**
2. **Add required Apple configuration**

### GitHub OAuth

**In GitHub Settings:**
1. Go to Developer settings > OAuth Apps
2. Create new OAuth App
3. Set Authorization callback URL to `https://your-project.supabase.co/auth/v1/callback`

**In Supabase Dashboard:**
1. **Enable GitHub provider**
2. **Add Client ID and Client Secret**

## Authentication Flow

### Email/Password Flow

1. **User provides credentials** in your Ivy application
2. **Supabase validates credentials** and creates session
3. **User receives access token** for authenticated requests
4. **Ivy manages session state** automatically

### Magic Link Flow

1. **User enters email address**
2. **Supabase sends magic link** via email
3. **User clicks link** to authenticate
4. **Redirected to application** with authentication tokens

### Social Login Flow

1. **User clicks social login button**
2. **Redirect to social provider** (Google, Apple, etc.)
3. **User authorizes application**
4. **Provider redirects to Supabase** with authorization code
5. **Supabase creates user session** and redirects to your app

## Advanced Configuration

### Row Level Security (RLS)

Combine authentication with database security:

```sql
-- Enable RLS on your table
ALTER TABLE posts ENABLE ROW LEVEL SECURITY;

-- Create policy for user's own data
CREATE POLICY "Users can only see their own posts" ON posts
FOR SELECT USING (auth.uid() = user_id);

-- Create policy for inserting
CREATE POLICY "Users can insert their own posts" ON posts
FOR INSERT WITH CHECK (auth.uid() = user_id);
```

### User Metadata

Store additional user information:

```csharp
public class UserProfileApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();

        return Card(
            Text($"Welcome, {user.Email}!"),
            Text($"Display Name: {user.UserMetadata?.DisplayName}"),
            Text($"Joined: {user.CreatedAt:yyyy-MM-dd}"),
            Button("Update Profile", () => UpdateProfile())
        );
    }
}
```

## Real-time Integration

Combine authentication with Supabase real-time features:

```csharp
public class ChatApp : AppBase
{
    public override Task<IView> BuildAsync()
    {
        return Task.FromResult<IView>(
            // Subscribe to real-time messages for authenticated user
            RealtimeView("messages", (messages) =>
                List(messages.Select(msg =>
                    Card(
                        Text(msg.Content),
                        Text($"By: {msg.UserEmail}")
                    )
                ))
            )
        );
    }
}
```

## Security Best Practices

- **Store keys securely** in User Secrets or environment variables
- **Use HTTPS** in production environments
- **Enable RLS** on database tables containing user data
- **Validate tokens** server-side for sensitive operations
- **Monitor authentication logs** in Supabase Dashboard
- **Configure email rate limiting** to prevent abuse
- **Use service role key** only for admin operations

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
            Text($"User ID: {user.Id}"),
            Text($"Email: {user.Email}"),
            Text($"Phone: {user.Phone ?? "Not provided"}"),
            user.IsEmailConfirmed
                ? Badge("Email Verified", Colors.Green)
                : Badge("Email Not Verified", Colors.Orange),
            Button("Sign Out", SignOutAsync)
        );
    }
}
```

### Custom User Claims

Add custom claims to JWT tokens using Supabase Database Functions:

```sql
CREATE OR REPLACE FUNCTION public.custom_access_token_hook(event jsonb)
RETURNS jsonb
LANGUAGE plpgsql
AS $$
DECLARE
  claims jsonb;
  user_role text;
BEGIN
  -- Get user role from your custom table
  SELECT role INTO user_role FROM public.user_roles WHERE user_id = (event->>'user_id')::uuid;

  claims := event->'claims';
  claims := jsonb_set(claims, '{user_role}', to_jsonb(user_role));

  RETURN jsonb_set(event, '{claims}', claims);
END;
$$;
```

## Troubleshooting

### Common Issues

**Invalid API Key**
- Verify anon key is correct and copied from Settings > API
- Check that the key hasn't been regenerated
- Ensure you're using the anon key, not service role key for client auth

**Email Not Delivered**
- Check spam/junk folders
- Verify SMTP settings in Authentication > Settings
- Test with different email providers
- Check Supabase email quota limits

**Redirect URL Mismatch**
- Verify Site URL in Authentication > Settings
- Check redirect URLs are properly configured
- Ensure URLs use HTTPS in production
- Test locally with localhost URLs

**RLS Blocking Access**
- Check Row Level Security policies are correctly configured
- Verify `auth.uid()` matches expected user ID
- Test policies using the Supabase SQL editor
- Ensure user is properly authenticated before accessing data

### Performance Optimization

**Token Refresh**
- Implement automatic token refresh for long sessions
- Cache user data appropriately
- Use session storage for temporary data

**Database Optimization**
- Create appropriate indexes on user-related columns
- Use connection pooling for database operations
- Implement proper query optimization

## Migration from Other Providers

When migrating from other authentication providers:

1. **Export user data** from current system
2. **Use Supabase Admin API** to create users
3. **Ask users to reset passwords** (for security)
4. **Test all authentication flows**

## Example Implementation

```csharp
public class AuthApp : AppBase
{
    public override Task<IView> BuildAsync()
    {
        return Task.FromResult<IView>(
            Card(
                Text("Welcome! Please sign in to continue."),

                Form(
                    TextInput("Email", "email"),
                    TextInput("Password", "password", isPassword: true),
                    Button("Sign In", HandleSignIn),
                    Button("Sign Up", HandleSignUp)
                ),

                Separator(),

                Text("Or continue with:"),
                Button("Google", () => LoginWithProvider("google")),
                Button("GitHub", () => LoginWithProvider("github")),

                Separator(),

                Button("Send Magic Link", HandleMagicLink)
            )
        );
    }

    private async Task HandleSignIn()
    {
        var email = GetFormValue("email");
        var password = GetFormValue("password");
        await SignInAsync(email, password);
    }

    private async Task HandleSignUp()
    {
        var email = GetFormValue("email");
        var password = GetFormValue("password");
        await SignUpAsync(email, password);
    }

    private async Task LoginWithProvider(string provider)
    {
        await LoginAsync(provider);
    }

    private async Task HandleMagicLink()
    {
        var email = GetFormValue("email");
        await SendMagicLinkAsync(email);
    }
}
```

## Combining with Supabase Database

When using both Supabase authentication and database:

```csharp
public class UserPostsApp : AppBase
{
    public async override Task<IView> BuildAsync()
    {
        var user = await GetCurrentUserAsync();

        // Query posts using RLS - only user's posts will be returned
        var posts = await DatabaseContext.Posts
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        return View(
            Text($"Your Posts ({posts.Count})"),
            List(posts.Select(post =>
                Card(
                    Text(post.Title),
                    Text(post.Content),
                    Text($"Created: {post.CreatedAt:yyyy-MM-dd}")
                )
            )),
            Button("New Post", CreatePost)
        );
    }
}
```

## Related Documentation

- [Authentication Overview](01_Overview.md)
- [Supabase Database Provider](../03_DatabaseIntegration/Supabase.md)
- [Auth0 Provider](Auth0.md)
- [Real-time Features](../../02_Concepts/TasksAndObservables.md)