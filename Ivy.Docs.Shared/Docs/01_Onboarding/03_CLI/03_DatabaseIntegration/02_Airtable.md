# Airtable Database Provider

<Ingress>
Connect your Ivy application to Airtable with automatic Entity Framework configuration for seamless integration with your Airtable bases.
</Ingress>

## Overview

Airtable is a cloud-based spreadsheet-database hybrid that combines the simplicity of a spreadsheet with the power of a database. Ivy provides integration with Airtable through Entity Framework Core, allowing you to leverage Airtable's flexible data organization in your applications.

## Connection Information

When setting up Airtable with Ivy, you'll be prompted for two pieces of information:

1. **Access Token**: Your Airtable personal access token (PAT)
2. **Base ID**: The ID of your Airtable base (starts with 'app')

These values will be combined into a connection string and stored in user secrets:

```text
BaseId=appXXXXXXXXXXXXXX;ApiKey=patXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
```

## Authentication

Airtable uses personal access tokens (PATs) for authentication. Generate a token from your [Airtable account page](https://airtable.com/account) in the API section.

For detailed instructions, see the [Airtable Authentication documentation](https://airtable.com/developers/web/api/authenticate).

## Configuration

Ivy automatically configures the **Ivy.Airtable.EFCore** package and imports the `Airtable.EFCore` and `AirtableApiClient` namespaces for Airtable connections.

## Working with Airtable Tables

Ivy maps Airtable tables to entity classes, handles Airtable's data types, and provides standard Entity Framework CRUD operations.

## Airtable-Specific Features

Key features Ivy can leverage:
- **Rich field types** (attachments, links, formulas)
- **Record linking** for relationships
- **Views** for filtered data presentation

See [Airtable Field Types documentation](https://airtable.com/developers/web/api/field-model) for details.

## Security Best Practices

- **Use scoped access tokens** with minimum required permissions
- **Rotate tokens** periodically for enhanced security
- **Never expose tokens** in client-side code
- **Create separate tokens** for development and production

## Troubleshooting

### Common Issues

**Authentication Failed**
- Verify token validity and permissions

**Base Access Issues**
- Confirm Base ID and account access

**Rate Limiting**
- Implement retry logic and caching

For API limits details, see [Airtable API Limits](https://airtable.com/developers/web/api/rate-limits).

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
- [Official Airtable API Documentation](https://airtable.com/developers/web/api/introduction)
- [Airtable .NET Client](https://github.com/ngocnicholas/airtable.net)
