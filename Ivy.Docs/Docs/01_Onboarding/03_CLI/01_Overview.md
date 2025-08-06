# Ivy CLI - Overview

Ivy is a powerful CLI tool designed to streamline the development of .NET applications with integrated database connectivity, authentication, and deployment capabilities.

## What is Ivy?

Ivy is a comprehensive development framework that provides:

- **Database Integration**: Connect to multiple database providers (SQL Server, PostgreSQL, MySQL, SQLite, and more)
- **Authentication**: Add authentication providers (Auth0, Supabase, Authelia, Basic Auth)
- **Deployment**: Deploy to cloud platforms (AWS, Azure, GCP)
- **Project Management**: Initialize and manage Ivy projects with ease

## Quick Start

Get started with Ivy in just a few commands:

```terminal
>ivy init
>ivy db add
>ivy auth add
>ivy deploy
```

## Key Features

### 🗄️ Database Support

- **SQL Server** - Microsoft's enterprise database
- **PostgreSQL** - Advanced open-source database
- **MySQL/MariaDB** - Popular open-source databases
- **SQLite** - Lightweight file-based database
- **Supabase** - Open-source Firebase alternative
- **Airtable** - Spreadsheet-database hybrid
- **Oracle** - Enterprise database system
- **Google Spanner** - Globally distributed database
- **ClickHouse** - Column-oriented database
- **Snowflake** - Cloud data platform

### 🔐 Authentication Providers

- **Auth0** - Universal authentication platform
- **Supabase Auth** - Built-in authentication
- **Authelia** - Open-source identity provider
- **Basic Auth** - Simple username/password authentication

### ☁️ Deployment Options

- **AWS** - Amazon Web Services
- **Azure** - Microsoft Azure
- **GCP** - Google Cloud Platform

## Project Structure

An Ivy project follows a standardized structure:

```text
YourProject/
├── Program.cs              # Main application entry point
├── appsettings.json        # Configuration settings
├── Connections/            # Database connections
│   └── [ConnectionName]/   # Individual connection configs
├── Auth/                   # Authentication providers
└── .ivy/                   # Ivy-specific configuration
```

## Getting Help

- Use `ivy --help` for general help
- Use `ivy [command] --help` for command-specific help
- Use `ivy docs` to open documentation
- Use `ivy samples` to see example projects

Most Ivy commands require authentication. Use `ivy login` to authenticate with your Ivy account.

## Next Steps

1. **Initialize a project**: `ivy init`
2. **Add a database**: `ivy db add`
3. **Add authentication**: `ivy auth add`
4. **Deploy your app**: `ivy deploy`

For detailed information on each feature, see the specific documentation files:

- [Init.md](Init.md) - Project initialization guide
- [Db.md](Db.md) - Database integration guide
- [Auth.md](Auth.md) - Authentication setup guide
- [Deploy.md](Deploy.md) - Deployment guide
