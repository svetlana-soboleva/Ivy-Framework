# Airtable Database Provider

<Ingress>
Connect your Ivy application to Airtable with automatic Entity Framework configuration for seamless integration with your Airtable bases.
</Ingress>

## Overview

Airtable is a cloud-based spreadsheet-database hybrid that combines the simplicity of a spreadsheet with the power of a database. Ivy provides integration with Airtable through Entity Framework Core, allowing you to leverage Airtable's flexible data organization in your applications.

## Connection String Format

```text
BaseId=appXXXXXXXXXXXXXX;ApiKey=patXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
```

## Authentication

Airtable uses personal access tokens (PATs) for authentication. You can generate a personal access token from your Airtable account settings:

1. Go to your [Airtable account page](https://airtable.com/account)
2. Navigate to the API section
3. Create a personal access token with appropriate permissions
4. Copy the token and use it when prompted during Ivy setup

## Configuration

Ivy automatically configures the **Ivy.Airtable.EFCore** package and imports the `Airtable.EFCore` and `AirtableApiClient` namespaces for Airtable connections.

## Working with Airtable Tables

In Airtable, tables function as entities in your Entity Framework context. The Ivy implementation:

- Maps Airtable tables to entity classes
- Handles Airtable's specific data types
- Manages relationships between tables
- Provides CRUD operations through the Entity Framework API

## Airtable-Specific Features

Airtable offers unique features that Ivy can leverage:
- **Rich field types** including attachments, links, and formulas
- **Views** for filtered and sorted data presentation
- **Record linking** for establishing relationships
- **Formulas** for computed fields
- **Attachments** for file storage

## Security Best Practices

- **Use scoped access tokens** with minimum required permissions
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