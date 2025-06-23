using System.Reactive.Subjects;
using Ivy.Core;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

namespace Ivy.Test;

public class TextInputTests
{
    [Fact]
    public void NonGenericTextInput_DefaultsToStringType()
    {
        var state = new MockState<string>("test");
        
        var textInput = new TextInput(state);
        
        Assert.Equal("test", textInput.Value);
        Assert.Equal(TextInputs.Text, textInput.Variant);
    }
    
    [Fact]
    public void NonGenericTextInput_WithValueAndOnChange_WorksCorrectly()
    {
        var onChange = new Action<Event<IInput<string>, string>>(e => _ = e.Value);
        
        var textInput = new TextInput("initial", onChange);
        
        Assert.Equal("initial", textInput.Value);
        Assert.NotNull(textInput.OnChange);
    }
    
    [Fact]
    public void NonGenericTextInput_WithPlaceholderAndVariant_WorksCorrectly()
    {
        var textInput = new TextInput(placeholder: "Enter text", variant: TextInputs.Password);
        
        Assert.Equal("Enter text", textInput.Placeholder);
        Assert.Equal(TextInputs.Password, textInput.Variant);
    }
    
    [Fact]
    public void GenericTextInput_StillWorksForStringType()
    {
        var state = new MockState<string>("test");
        
        var textInput = new TextInput<string>(state);
        
        Assert.Equal("test", textInput.Value);
        Assert.Equal(TextInputs.Text, textInput.Variant);
    }
    
    private class MockState<T>(T value) : IState<T>
    {
        private readonly Subject<T> _subject = new();

        public T Value { get; set; } = value;

        public T Set(T value) => Value = value;
        public Type GetStateType() => typeof(T);
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnNext(Value);
            return _subject.Subscribe(observer);
        }
        
        public void Dispose()
        {
            _subject.Dispose();
        }
        
        public IDisposable SubscribeAny(Action action)
        {
            return _subject.Subscribe(_ => action());
        }
        
        public IDisposable SubscribeAny(Action<object?> action)
        {
            return _subject.Subscribe(x => action(x));
        }
        
        public IEffectTrigger ToTrigger()
        {
            return EffectTrigger.AfterChange(this);
        }
    }
} 