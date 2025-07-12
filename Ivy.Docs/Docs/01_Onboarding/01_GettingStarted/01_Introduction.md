# Introduction

Ivy is a full-stack open-source web framework optimized for rapidly building interactive data-centric applications.

<Embed Url="https://www.youtube.com/watch?v=pQKSQR9BfD8"/>

## Motivation Behind Ivy

These are the core design goals that drove the creation of Ivy:

### **Cost & Speed Optimization**

We want these applications to be built as cheaply and quickly as possible. Everyday tasks should be simple and idiomatic, while rare or complex tasks remain possible.

### **Open-Source & Cloud-Native**

Developers prefer open-source frameworks, which can be hosted in any cloud, preferably in a Docker container. Many existing solutions in this space are closed-source and/or force you to their hosting offering.

### **Avoiding Technical Debt**

Many low-code SaaS products are limited and expensive in the long run and have significant graduation risks and just add to your technical debt.

### **Eliminating Boilerplate**

Existing SPA solutions (including WASM-based frameworks) require both a front-end and back-end that communicates through an API. This results in lots of boilerplate code. We want to present "anonymous" data results and CRUD operations with as little ceremony as possible.

### **Declarative UI Design**

Styling and layout possibilities should be at a minimum. We want to express our UI declaratively, and then the framework will generate a stunning user interface.

### **Desktop-First Experience**

Some alternatives offer a mobile-first approach. Internal apps are more likely to be lean-forward user experiences, and if there's a trade-off to be made, it should be in the desktop experience's favour.

## Core Features

### **Framework Architecture**

- **Full-stack framework** to build data-centric applications in pure C#
- **Declarative React-like C# application model** with no need for a separate backend APIs
- **Views that render into Widgets** (Like React's components that are rendered into elements)
- **Scaffolded builder patterns** for Tables, Forms and Record views

### **Real-Time & Interactive**

- **UI is maintained using WebSockets**, similar to [Streamlit](https://streamlit.io/) and many other modern frameworks
- **Hot Reloading** (with maintained state)
- **Anything in dotnet can be built** using the ContentBuilder pipelines (inspired by LinqPad's Dump)

### **Modern Frontend Integration**

- **Widgets are rendered using React/Shadcn/TailwindCSS**, allowing simple integration with components from this massive ecosystem
- **External widgets can be imported through nugets**. Any JavaScript/React component can be an Ivy widget
- **Dark Mode/Color Themes**
- **Customizable "chromes"** (they are also Ivy apps)

### **Enterprise Features**

- **Authentication/Authorization Providers/RBAC**
- **Data Providers (EF Core)**
- **Secrets Management**
- **Dependency Injection**
- **Caching**
- **Routing**

### **Development & Deployment**

- **Container Deployment to any cloud** (AWS, Azure, GCP). The Ivy tooling helps you create and deploy a docker container with all environment variables and secrets in place
- **CLI** (new project templates, add data provider, deploy, AI scaffolding and more)
- **Unit testing** is possible without the need for browser automation

## Roadmap

### **Advanced Data Handling**

- **Data tables and graphs using Apache Arrow Tables and GRPC** (filtering/column selection/ordering/pagination)
- **Build an Airtable-like experience** scaffolded from EF IQueryable with the presentation of millions of rows without losing performance
