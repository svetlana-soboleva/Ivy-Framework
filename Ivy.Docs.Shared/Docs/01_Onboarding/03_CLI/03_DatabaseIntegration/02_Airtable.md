# Airtable Database Provider

<Ingress>
Connect your Ivy application to Airtable with automatic Entity Framework configuration for seamless integration with your Airtable bases.
</Ingress>

## Overview

Airtable is a cloud-based spreadsheet-database hybrid that combines the simplicity of a spreadsheet with the power of a database. Ivy provides integration with Airtable through Entity Framework Core, allowing you to leverage Airtable's flexible data organization in your applications.

## Setup

### Adding Airtable Connection

```terminal
>ivy db add --provider Airtable --name MyAirtable
```

### Interactive Setup

When using interactive mode, Ivy will guide you through:

1. **Connection Name**: Enter a name for your connection (PascalCase recommended)
2. **Access Token**: Your Airtable personal access token (stored securely)
3. **Base ID**: The ID of the Airtable base you want to connect to

## Connection String Format

```text
BaseId=appXXXXXXXXXXXXXX;ApiKey=patXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
```

### Authentication

Airtable uses personal access tokens (PATs) for authentication. You can generate a personal access token from your Airtable account settings:

1. Go to your [Airtable account page](https://airtable.com/account)
2. Navigate to the API section
3. Create a personal access token with appropriate permissions
4. Copy the token and use it when prompted during Ivy setup

## Configuration

### Entity Framework Setup

Ivy automatically configures:
- **Ivy.Airtable.EFCore** package
- Imports the `Airtable.EFCore` and `AirtableApiClient` namespaces
- **Connection strings** stored in .NET User Secrets
- **DbContext** with Airtable provider configuration

### Generated Files

```text
Connections/
└── MyAirtable/
    ├── MyAirtableContext.cs             # Entity Framework DbContext
    ├── MyAirtableContextFactory.cs      # DbContext factory
    ├── MyAirtableConnection.cs          # Connection configuration
    └── [EntityName].cs...               # Generated entity classes
```

## Advanced Configuration

### Working with Airtable Tables

In Airtable, tables function as entities in your Entity Framework context. The Ivy implementation:

- Maps Airtable tables to entity classes
- Handles Airtable's specific data types
- Manages relationships between tables
- Provides CRUD operations through the Entity Framework API

### Airtable-Specific Features

Airtable offers unique features that Ivy can leverage:
- **Rich field types** including attachments, links, and formulas
- **Views** for filtered and sorted data presentation
- **Record linking** for establishing relationships
- **Formulas** for computed fields
- **Attachments** for file storage

## Security Best Practices

- **Use scoped access tokens** with minimum required permissions
- **Store tokens** in User Secrets or secure vaults
- **Rotate tokens** periodically for enhanced security
- **Never expose tokens** in client-side code
- **Create separate tokens** for development and production

## Troubleshooting

### Common Issues

**Authentication Failed**
- Verify your personal access token is correct and has not expired
- Ensure the token has appropriate permissions
- Check that your token has not been revoked

**Base Access Issues**
- Confirm the Base ID is correct
- Verify your account has access to the specified base
- Check that the token has permission to access the specific base

**Rate Limiting**
- Airtable enforces API rate limits
- Implement retry logic for rate limit errors
- Consider caching frequently accessed data

## Example Usage

```csharp
// In your Ivy app
public class ProductApp : AppBase<Product>
{
    public override Task<IView> BuildAsync(Product product)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Product: {product.Name}"),
                Text($"Category: {product.Category}"),
                Text($"Price: ${product.Price:F2}")
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [SQLite Provider](SQLite.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [Airtable API Documentation](https://airtable.com/developers/web/api/introduction)