# Basic Authentication Provider

<Ingress>
Secure your Ivy application with simple username/password authentication.
</Ingress>

## Overview

Basic Authentication in Ivy provides a straightforward way to secure your application using username and password credentials. It's ideal for development, testing, and simple internal applications where you need quick authentication setup with minimal complexity.

## Adding Authentication

To set up Basic Authentication with Ivy, run the following command:

```terminal
>ivy auth add --provider Basic
```

You will be prompted to provide user credentials in the following format:

```text
user1:password1; user2:password2; ...
```

Your credentials will be stored securely in .NET user secrets, along with an automatically-generated secret key.

## Connection String Format

For automated setup, you can provide credentials via the connection string parameter:

```terminal
>ivy auth add --provider Basic --connection-string "USERS=user1:password1;user2:password2"
```

The connection string uses the following parameters:

- **USERS**: Required. A semicolon-separated list of username:password pairs.
- **JWT_SECRET**: Optional. A custom secret key for token generation. If not provided, one will be generated automatically.

## Configuration

Ivy automatically configures your application for Basic Authentication:

1. Updates your `Program.cs` file with `server.UseAuth<BasicAuthProvider>()`.
2. Adds `Ivy.Auth` to your global usings.

Your credentials are stored in .NET user secrets and can be accessed as environment variables in your application.

## Authentication Flow

1. User submits credentials through your application
2. Server validates credentials against the stored user list
3. If valid, an authentication token is issued to the client
4. The client includes this token in subsequent requests
5. The server validates the token for each protected resource access

## Basic Auth-Specific Features

Key features of the Basic Auth provider:

- **Simple Configuration**: Quick setup with minimal parameters
- **In-memory Authentication**: Fast credential validation without external services
- **Token-based Security**: Stateless authentication using secure tokens
- **Multiple Users**: Support for multiple user credentials

## Security Best Practices

- **Always use HTTPS** in production environments
- **Use strong passwords** for all user accounts
- **Rotate credentials** periodically
- **Consider upgrading** to a more robust authentication provider for production applications

## Troubleshooting

### Common Issues

**Authentication Failed**
- Verify user credentials are correctly formatted (`user:password;user2:password2`)
- Check that the correct username and password are being submitted
- Ensure credentials are properly stored in user secrets

**Missing Configuration**
- Verify the Basic Auth provider is properly configured in your `Program.cs`
- Check user secrets are accessible to your application

**Token Issues**
- Tokens have a limited lifetime; ensure clients request new tokens as needed
- Verify the application is correctly sending the token in the Authorization header

## Related Documentation

- [Authentication Overview](01_Overview.md)
- [Auth0 Provider](Auth0.md)
- [Microsoft Entra Provider](MicrosoftEntra.md)