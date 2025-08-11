# Ivy Init - Getting Started

<Ingress Text="Quickly scaffold new Ivy projects with the necessary structure, configuration files, and boilerplate code using the init command." />

The `ivy init` command creates a new Ivy project with the necessary structure and configuration files to get you started quickly.

## Basic Usage

```terminal
>ivy init
```

This command will:

- Create a new Ivy project in the current directory
- Set up the basic project structure
- Generate necessary configuration files

## Command Options

`--namespace <NAMESPACE>` - Specify the namespace for your Ivy project. If not provided, Ivy will suggest a namespace based on the folder name.

```terminal
>ivy init --namespace MyCompany.MyApp
```

`--dangerous-clear` - Clear the current folder before creating the new project. **Use with caution!**

```terminal
>ivy init --dangerous-clear
```

`--dangerous-overwrite` - Overwrite existing files in the current folder. **Use with caution!**

```terminal
>ivy init --dangerous-overwrite
```

`--verbose` - Enable verbose output for detailed logging during initialization.

```terminal
>ivy init --verbose
```

`--helloworld` or `--hello` - Include a simple demo application in the new project to help you get started.

```terminal
>ivy init --helloworld
```

## Interactive Mode

When you run `ivy init` without specifying a namespace, Ivy will prompt you to enter one:

```terminal
Namespace for the new Ivy project: [suggested-namespace]
```

Ivy will suggest a namespace based on your current folder name. You can accept the suggestion or enter a custom namespace.

## Project Structure

After running `ivy init`, your project will have the following structure:

```text
YourProject/
├── Program.cs              # Main application entry point
├── appsettings.json        # Application configuration
├── appsettings.Development.json  # Development-specific settings
├── .ivy/                   # Ivy-specific configuration
│   └── config.json        # Ivy project configuration
├── .gitignore             # Git ignore file
├── GlobalUsings.cs        # Global using statements
└── README.md              # Project documentation
```

## Generated Files

### Program.cs

The main entry point for your Ivy application:

```csharp
using Ivy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddIvy();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseIvy();

app.Run();
```

### appsettings.json

Basic configuration file with Ivy settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Ivy": {
    "ProjectName": "YourProject"
  }
}
```

### GlobalUsings.cs

Global using statements for common Ivy namespaces:

```csharp
global using Ivy;
global using Ivy.Data;
global using Ivy.Auth;
```

## Prerequisites

Before running `ivy init`, ensure you have:

1. **.NET SDK** installed (version 8.0 or later)
2. **Git** installed (optional, but recommended)
3. **Empty directory** or use `--dangerous-clear`/`--dangerous-overwrite`

## Validation

Ivy performs several validations during initialization:

- **Directory Check**: Ensures the target directory is empty (unless using overwrite options)
- **Namespace Validation**: Validates the provided namespace format
- **Git Status**: Checks for uncommitted changes if Git is initialized
- **.NET Tools**: Ensures required .NET tools are installed

## Error Handling

**Empty Directory Required** - If the current directory is not empty, Ivy will show an error:

```terminal
The current folder is not empty. Please clear the folder or use the --dangerous-clear option or --dangerous-overwrite
```

**Invalid Namespace** - If you provide an invalid namespace, Ivy will prompt you to enter a valid one:

```terminal
Invalid 'invalid-namespace' namespace. Please enter a valid namespace.
```

## Next Steps

After initializing your project:

1. **Add a database connection**: `ivy db add`
2. **Add authentication**: `ivy auth add`
3. **Create an application**: `ivy app create`
4. **Deploy your application**: `ivy deploy`

## Examples

**Basic Project Initialization**

```terminal
>mkdir MyIvyApp
>cd MyIvyApp
>ivy init
```

**Project with Custom Namespace**

```terminal
>ivy init --namespace AcmeCorp.InventorySystem
```

**Project with Demo App**

```terminal
>ivy init --helloworld --namespace MyDemoApp
```

**Verbose Initialization**

```terminal
>ivy init --verbose --namespace MyApp
```

## Troubleshooting

**Permission Issues** - If you encounter permission issues, ensure you have write access to the current directory.

**NET Tools Not Found** - If required .NET tools are missing, Ivy will attempt to install them automatically. You may need to run:

```terminal
>dotnet tool install -g dotnet-ef
>dotnet tool install -g dotnet-user-secrets
```

**Git Issues** - If Git is not installed or configured, Ivy will still create the project but may skip some Git-related operations.

## Related Commands

- `ivy db add` - Add database connections
- `ivy auth add` - Add authentication providers
- `ivy app create` - Create applications
- `ivy deploy` - Deploy your application
