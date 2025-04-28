# Ivy: Build Internal Applications with AI and Pure C\#

Ivy - The ultimate framework for building internal tools with LLM code generation by unifying front-end and back-end into a single C# codebase. With Ivy, you can build robust internal tools and dashboards using C# and AI assistance based on your existing database.

https://www.youtube.com/watch?v=CrybY7pmjO4

## Features

We plan to support the following features.

- **CLI**: Init new projects, add data providers, generate apps using AI and finally deploy.
- **Authentication**: Integrations with Supabase, Microsoft Entra, and more.
- **Databases and APIs**: Easy integration with popular databases and APIs.
- **LLM Code Agent**: Scaffold CRUD apps based on your database schema.
- **Secrets Management**
- **Container Deployment**: Easily deploy to Azure, AWS, or Google Cloud.
- **Hot Reload**: With maintained state
- **Dependency Injection**
- **State Management**
- **Routing**
- **External Widget Framework**: Integrate any React, Angular, or Vue component
- **Data Tables**: Sort, filter, and paginate data.
- **Graphs**

## Inspirations

- React
- FuncUI
- Streamlit
- LINQPad ("Dump" method)

## Usage

### Quick Start

Make sure you have the .Net 9 SDK installed. 

1. **Install Ivy CLI**:
   ```
   dotnet tool install -g Ivy.Console
   ```
2. **Create a new project**:
   ```
    ivy init
    ```
3. **Add a data provider**: 
   ```
   ivy db add
   ```
4. **Run**:
   ```
   dotnet watch
   ```
5. **Open** [http://localhost:5000](http://localhost:5000) in your browser.

You can also run `ivy samples` to see all the components that Ivy offers. 

### Developer Build

1. **Install dependencies**:
   - [Node & npm](https://docs.npmjs.com/downloading-and-installing-node-js-and-npm)
   - [Vite](https://vitejs.dev/)
   - [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
2. **Clone this repository**.
3. **Run Ivy.Samples backend**:
   ```bash
   cd Ivy.Samples
   dotnet watch
   ```
4. **Build the frontend**:
   ```bash
   cd ../frontend
   npm install
   npm run build
   npm run dev
   ```   
5. **Open** [http://localhost:5000](http://localhost:5173) in your browser.

Changes in /frontend will be hot-reloaded by Vite and supported changes in /Ivy.Samples will be hot-reloaded by Ivy. 

Ivy is still early in development and we are working on more robust documentation. 

## Contributing

1. **Fork** the repository.
2. **Create** a new feature branch.
3. **Submit** a pull request.

Feel free to file issues and feature requests.


