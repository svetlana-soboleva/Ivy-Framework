# How Ivy Works

Ivy applications are written in pure C#. 

Ivy takes inspirations from React. Views are similar to Components in React. Views must implement a Build method that can return Another view a widget or any othere .net type that Ivy tries to render.

After the initial rendering the widget tree is sent over websocket to a React based rendering frontend. This frontend is included in the Ivy framework and is something a user never have to modify. 

On the the frontend side widgets can trigger events. Ivy auto-detects state changes and rerenders subset of the view tree. 
