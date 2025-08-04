# Chat

The `Chat` widget renders a conversation between a user and an assistant. Messages
are supplied as `ChatMessage` objects and new messages are sent through the
`OnSendMessage` event.

```csharp
var messages = UseState(ImmutableArray.Create<ChatMessage>());
void OnSendMessage(Event<Chat,string> e)
{
    messages.Set(messages.Value.Add(new ChatMessage(ChatSender.User, e.Value)));
}

return new Chat(messages.Value.ToArray(), OnSendMessage);
```

<WidgetDocs Type="Ivy.Chat" ExtensionTypes="Ivy.ChatExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Chat.cs"/>
