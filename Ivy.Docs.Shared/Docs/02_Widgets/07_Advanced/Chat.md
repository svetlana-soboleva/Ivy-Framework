# Chat

The `Chat` widget renders a conversation between a user and an assistant. 

Messages are supplied as `ChatMessage` objects and new messages are sent through the `OnSendMessage` event.

## Chat Components

```csharp demo-tabs
new Chat(
    new[] { new ChatMessage(ChatSender.Assistant, "Hello! I'm an echo bot. I'll repeat whatever you say!") },
    e => { }
)
```

### AI Assistant with Loading States

```csharp demo-tabs
new Chat(
    new[] { new ChatMessage(ChatSender.Assistant, "I'm an AI assistant! Ask me anything and I'll respond with a loading state.") },
    e => { }
)
```

### Interactive Chat with Buttons

```csharp demo-tabs
new Chat(
    new[] { new ChatMessage(ChatSender.Assistant, "I can show interactive buttons! Try sending any message.") },
    e => { }
)
```

### Error Handling Chat

```csharp demo-tabs
new Chat(
    new[] { new ChatMessage(ChatSender.Assistant, "I demonstrate error handling! Try sending 'error' to see it in action.") },
    e => { }
)
```

### Advanced Chat with Commands

```csharp demo-tabs
new Chat(
    new[] { 
        new ChatMessage(ChatSender.Assistant, 
            "Welcome to the Advanced Chat! Try these commands:\n" +
            "• 'analyze code' - I'll show code analysis\n" +
            "• 'create form' - I'll show an interactive form\n" +
            "• 'show chart' - I'll display a chart\n" +
            "• Any other message - I'll respond normally")
    },
    e => { }
)
```

<WidgetDocs Type="Ivy.Chat" ExtensionTypes="Ivy.ChatExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Chat.cs"/>
