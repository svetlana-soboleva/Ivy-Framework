# Google Spanner Database Provider

<Ingress>
Connect your Ivy application to Google Cloud Spanner with automatic Entity Framework configuration.
</Ingress>

## Overview

Google Cloud Spanner is a fully managed, mission-critical, relational database service that offers transactional consistency at global scale, automatic, synchronous replication for high availability, and support for schema changes without downtime. Ivy provides seamless integration with Google Cloud Spanner through Entity Framework Core.

## Connection String Format

```text
Data Source=projects/{projectId}/instances/{instanceId}/databases/{databaseId}
```

## Authentication Options

Google Cloud Spanner uses application default credentials for authentication. Before using Spanner with Ivy, ensure that you have:

1. Installed the [Google Cloud SDK](https://cloud.google.com/sdk/docs/install)
2. Authenticated with `gcloud auth application-default login`
3. Set up the appropriate permissions for your service account

For local development, you can set up authentication with:
```terminal
>gcloud auth application-default login
```

For production environments, you'll typically use service account credentials. See the [Google Cloud documentation](https://cloud.google.com/docs/authentication/provide-credentials-adc) for more details.

## Configuration

Ivy automatically configures the **Google.Cloud.EntityFrameworkCore.Spanner** package for Spanner connections.

## Advanced Configuration

### Working with the Spanner Emulator

For development purposes, you can use the [Spanner emulator](https://cloud.google.com/spanner/docs/emulator):

```terminal
>gcloud emulators spanner start
```

Then set the emulator environment variables:
```terminal
>$(gcloud emulators spanner env-init)
```

## Spanner-Specific Features

Spanner offers advanced features that Ivy can leverage:
- **Interleaved tables** for parent-child relationships
- **Strong consistency** across regions
- **Automatic sharding** for high scalability
- **Commit timestamps** for auditing and versioning
- **Secondary indexes** for query optimization

## Security Best Practices

- **Use service accounts** with minimum required permissions
- **Enable audit logging** for security monitoring
- **Implement row-level security** where appropriate
- **Use VPC Service Controls** to restrict access in production

## Troubleshooting

### Common Issues

**Authentication Failed**
- Ensure Google application credentials are properly configured
- Verify service account has appropriate permissions
- Check that your credentials have not expired

**Connection Timeouts**
- Check network connectivity to Google Cloud
- Verify firewall rules allow connections
- Ensure your Spanner instance is running and healthy

**Schema Creation Issues**
- Spanner has specific constraints on schema design
- Check Spanner documentation for supported DDL operations
- Ensure your model follows Spanner's data modeling best practices

## Example Usage

```csharp
// In your Ivy app
public class CustomerApp : AppBase<Customer>
{
    public override Task<IView> BuildAsync(Customer customer)
    {
        return Task.FromResult<IView>(
            Card(
                Text($"Customer: {customer.Name}"),
                Text($"Email: {customer.Email}")
            )
        );
    }
}
```

## Related Documentation

- [Database Overview](01_Overview.md)
- [SQL Server Provider](SqlServer.md)
- [PostgreSQL Provider](PostgreSQL.md)
- [Cloud Spanner Documentation](https://cloud.google.com/spanner/docs)