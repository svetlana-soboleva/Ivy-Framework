# How Ivy Works

Ivy applications are written in pure C#. 

Ivy takes inspirations from React. Views are similar to Components in React. Views must implement a Build method that can return Another view a widget or any othere .net type that Ivy tries to render.

After the initial rendering the widget tree is sent over websocket to a React based rendering frontend. This frontend is included in the Ivy framework and is something a user never have to modify. 

On the frontend side widgets can trigger events. Ivy auto-detects state changes and rerenders subset of the view tree. 

![Foo](/Assets/niels.jpg)


Ivy is a lightweight C# framework for building internal tools and small web apps. The framework runs entirely on the server and renders HTML using a set of strongly typed widgets.  A typical Ivy application consists of one or more **Views**.  A view is just a C# class that inherits from `ViewBase` and implements a single `Build()` method.  The `Build()` method returns either another view or a widget that Ivy knows how to render.

Ivy borrows ideas from React.  Views are pure functions of their props and state.  Hooks such as `UseState`, `UseEffect` and `UseService` provide a reactive programming model.  When state changes Ivy automatically re-renders the view hierarchy.

Under the hood Ivy compiles your views to razor pages that are served by a minimal ASP.NET application.  Hot‑reloading is built in – the development workflow is simply `dotnet watch` which recompiles and refreshes the browser whenever you change a C# file.

The framework ships with a library of widgets for forms, lists, tables and layout primitives.  Because everything is just C#, developers can easily integrate existing libraries, services and database code.  The `Ivy.Agent.Examples` project included in this repository demonstrates several complete applications built with the framework.