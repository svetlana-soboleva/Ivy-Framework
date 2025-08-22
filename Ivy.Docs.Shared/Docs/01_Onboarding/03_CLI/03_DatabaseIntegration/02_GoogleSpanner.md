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

Google Cloud Spanner uses application default credentials. Set up authentication with:

```terminal
>gcloud auth application-default login
```

Ivy will check for Google application credentials during setup. If credentials aren't found, you can still proceed (especially when using the Spanner emulator), but you'll need proper credentials for production usage.

For detailed instructions, see the [Google Cloud Authentication documentation](https://cloud.google.com/docs/authentication/provide-credentials-adc).

## Configuration

Ivy automatically configures the **Google.Cloud.EntityFrameworkCore.Spanner** package for Spanner connections.

## Advanced Configuration

### Working with the Spanner Emulator

Ivy works with the Spanner emulator for development. Set up the emulator with:

```terminal
>gcloud emulators spanner start
>$(gcloud emulators spanner env-init)
```

When using the emulator, you don't need to set up Google application credentials.

For detailed emulator setup, see the [Spanner Emulator documentation](https://cloud.google.com/spanner/docs/emulator).

## Spanner-Specific Features

Key features that Ivy can leverage:
- **Interleaved tables** for parent-child relationships
- **Strong consistency** across regions
- **Secondary indexes** for query optimization

For more information, visit the [Cloud Spanner Documentation](https://cloud.google.com/spanner/docs).

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

**Connection Timeouts**
- Check network connectivity to Google Cloud
- Ensure your Spanner instance is running and healthy

## Related Documentation

- [Database Overview](01_Overview.md)
- [SQL Server Provider](SqlServer.md)
- [PostgreSQL Provider](PostgreSql.md)
- [Official Google Cloud Spanner Documentation](https://cloud.google.com/spanner/docs)
- [Google.Cloud.EntityFrameworkCore.Spanner Package](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.EntityFrameworkCore.Spanner/latest)
