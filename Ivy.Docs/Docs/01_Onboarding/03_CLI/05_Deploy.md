# Ivy Deployment

The `ivy deploy` command allows you to deploy your Ivy application to various cloud platforms. Ivy supports multiple deployment providers and automatically handles the deployment process including containerization, configuration, and infrastructure setup.

## Supported Deployment Providers

Ivy supports the following cloud deployment providers:

### Cloud Platforms

- **AWS** - Amazon Web Services
- **Azure** - Microsoft Azure
- **GCP** - Google Cloud Platform

## Basic Usage

### Deploying Your Application

```terminal
>ivy deploy
```

This command will:

- Prompt you to select a deployment provider
- Build and containerize your application
- Configure the necessary cloud resources
- Deploy your application to the selected platform
- Provide you with the deployment URL

## Command Options

### `ivy deploy` Options

#### `--project-path <PATH>`

Specify the path to your project directory. Defaults to the current directory.

```terminal
>ivy deploy --project-path /path/to/your/project
```

#### `--verbose`

Enable verbose output for detailed logging during deployment.

```terminal
>ivy deploy --verbose
```

## Interactive Mode

When you run `ivy deploy` without specifying options, Ivy will guide you through an interactive deployment process:

1. **Select Deployment Provider**: Choose from AWS, Azure, or GCP
2. **Configuration Setup**: Configure provider-specific settings
3. **Build Process**: Ivy will build and containerize your application
4. **Deployment**: Deploy to the selected cloud platform

## Deployment Provider Configuration

### AWS (Amazon Web Services)

AWS provides a comprehensive cloud platform with various services for application deployment.

#### Setup Process

```terminal
>ivy deploy
# Select AWS when prompted
```

#### Required Configuration

- **AWS Credentials**: Access key and secret key
- **Region**: AWS region for deployment
- **ECR Repository**: Container registry for your application
- **App Runner Service**: Serverless container service for running your app

#### AWS Services Used

- **Amazon ECR** - Container registry
- **AWS App Runner** - Serverless container service
- **Amazon S3** - Storage for build artifacts
- **AWS IAM** - Identity and access management

#### AWS Setup Prerequisites

1. Create an AWS account
2. Install and configure AWS CLI
3. Create an IAM user with appropriate permissions
4. Configure AWS credentials: `aws configure`

### Azure (Microsoft Azure)

Azure provides cloud services for building, deploying, and managing applications.

#### Setup Process

```terminal
>ivy deploy
# Select Azure when prompted
```

#### Required Configuration

- **Azure Subscription**: Your Azure subscription
- **Resource Group**: Azure resource group for your resources
- **Container Registry**: Azure Container Registry (ACR)
- **Container Apps Environment**: Environment for your containerized app

#### Azure Services Used

- **Azure Container Registry** - Container registry
- **Azure Container Apps** - Serverless container platform
- **Azure Resource Manager** - Resource management
- **Azure Active Directory** - Identity management

#### Azure Setup Prerequisites

1. Create an Azure account
2. Install Azure CLI
3. Login to Azure: `az login`
4. Set your subscription: `az account set --subscription <subscription-id>`

### GCP (Google Cloud Platform)

GCP provides cloud computing services for building, testing, and deploying applications.

#### Setup Process

```terminal
>ivy deploy
# Select GCP when prompted
```

#### Required Configuration

- **GCP Project**: Your Google Cloud project
- **Container Registry**: Google Container Registry (GCR)
- **Cloud Run Service**: Serverless container platform
- **Region**: GCP region for deployment

#### GCP Services Used

- **Google Container Registry** - Container registry
- **Cloud Run** - Serverless container platform
- **Cloud Build** - Build service
- **IAM** - Identity and access management

#### GCP Setup Prerequisites

1. Create a Google Cloud account
2. Install Google Cloud CLI
3. Login to GCP: `gcloud auth login`
4. Set your project: `gcloud config set project <project-id>`

## Deployment Process

### 1. Project Validation

Ivy validates your project before deployment:

- Ensures it's an Ivy project
- Checks for required files and configuration
- Validates authentication status

### 2. Build Process

Ivy builds your application:

- Restores NuGet packages
- Builds the application
- Creates Docker container
- Pushes to container registry

### 3. Infrastructure Setup

Ivy configures cloud resources:

- Creates necessary cloud services
- Configures networking and security
- Sets up monitoring and logging

### 4. Application Deployment

Ivy deploys your application:

- Deploys container to cloud platform
- Configures environment variables
- Sets up custom domains (if configured)
- Provides deployment URL

## Containerization

### Docker Configuration

Ivy automatically generates a `Dockerfile` for your application:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["YourProject.csproj", "./"]
RUN dotnet restore "YourProject.csproj"
COPY . .
RUN dotnet build "YourProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YourProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourProject.dll"]
```

### Environment Configuration

Ivy configures environment variables for your deployed application:

```text
# Database connection strings
ConnectionStrings__MyDatabase="your-connection-string"

# Authentication settings
Auth0__Domain="your-auth0-domain"
Auth0__ClientId="your-client-id"

# Application settings
ASPNETCORE_ENVIRONMENT="Production"
```

## Security and Configuration

### Secrets Management

Ivy handles secrets securely during deployment:

- **Connection Strings**: Stored as environment variables
- **Authentication Secrets**: Configured securely
- **API Keys**: Managed through cloud provider secrets

### Network Security

Ivy configures network security:

- **HTTPS**: Automatic SSL/TLS configuration
- **Firewall Rules**: Appropriate network access
- **VPC Configuration**: Network isolation (where applicable)

## Monitoring and Logging

### Application Monitoring

Ivy sets up monitoring for your deployed application:

- **Health Checks**: Application health monitoring
- **Metrics**: Performance and resource metrics
- **Logging**: Centralized log collection
- **Alerts**: Automated alerting for issues

### Cloud Provider Monitoring

Each cloud provider offers specific monitoring tools:

- **AWS**: CloudWatch
- **Azure**: Application Insights
- **GCP**: Cloud Monitoring

## Deployment Options

### Build Choices

Ivy offers different build options:

#### Local Build

- Builds container locally
- Requires Docker installed
- Faster for development

#### Cloud Build

- Builds in cloud provider
- No local Docker required
- Consistent build environment

### Scaling Configuration

Configure application scaling:

- **Auto-scaling**: Automatic scaling based on demand
- **Manual scaling**: Fixed number of instances
- **Min/Max instances**: Scaling boundaries

## Troubleshooting

### Common Deployment Issues

#### Build Failures

- Check your application builds locally
- Verify all dependencies are included
- Check for compilation errors

#### Container Registry Issues

- Verify container registry credentials
- Check network connectivity
- Ensure proper permissions

#### Cloud Provider Issues

- Verify cloud provider credentials
- Check resource quotas and limits
- Ensure required services are enabled

### Debugging Deployment

#### Enable Verbose Logging

```terminal
>ivy deploy --verbose
```

#### Check Cloud Provider Logs

- **AWS**: CloudWatch logs
- **Azure**: Application Insights
- **GCP**: Cloud Logging

#### Verify Deployment Status

```bash
# Check deployment status in cloud console
# Verify application is accessible
# Check environment variables and configuration
```

## Examples

### Basic AWS Deployment

```terminal
>ivy deploy
# Select AWS
# Follow prompts for configuration
```

### Azure Deployment with Custom Settings

```terminal
>ivy deploy --verbose
# Select Azure
# Configure custom resource group and region
```

### GCP Deployment

```terminal
>ivy deploy
# Select GCP
# Configure project and region
```

## Best Practices

### Pre-deployment Checklist

1. **Test Locally**: Ensure your application runs locally
2. **Check Dependencies**: Verify all required services are configured
3. **Review Configuration**: Check environment variables and settings
4. **Security Review**: Verify authentication and authorization setup

### Deployment Strategy

- **Blue-Green Deployment**: Zero-downtime deployments
- **Rolling Updates**: Gradual deployment across instances
- **Canary Deployments**: Test with subset of users

### Cost Optimization

- **Right-sizing**: Choose appropriate instance sizes
- **Auto-scaling**: Configure scaling based on actual usage
- **Resource cleanup**: Remove unused resources
- **Monitoring**: Track resource usage and costs

### Security Best Practices

- **Secrets Management**: Use cloud provider secrets services
- **Network Security**: Configure appropriate firewall rules
- **Access Control**: Implement least-privilege access
- **Regular Updates**: Keep dependencies and runtime updated

## Post-deployment

### Application Management

After deployment, you can:

- **Monitor Performance**: Use cloud provider monitoring tools
- **Scale Application**: Adjust instance count based on demand
- **Update Application**: Deploy new versions
- **Configure Domains**: Set up custom domains and SSL

### Maintenance

Regular maintenance tasks:

- **Security Updates**: Keep dependencies updated
- **Performance Monitoring**: Monitor and optimize performance
- **Cost Monitoring**: Track and optimize costs
- **Backup Management**: Configure and test backups

## Related Commands

- `ivy init` - Initialize a new Ivy project
- `ivy db add` - Add database connections
- `ivy auth add` - Add authentication providers
- `ivy app create` - Create applications

## Cloud Provider Documentation

For detailed information about each cloud provider:

- **AWS**: [AWS Documentation](https://docs.aws.amazon.com/)
- **Azure**: [Azure Documentation](https://docs.microsoft.com/azure/)
- **GCP**: [Google Cloud Documentation](https://cloud.google.com/docs/) 