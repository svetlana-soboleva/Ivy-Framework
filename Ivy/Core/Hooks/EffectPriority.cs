namespace Ivy.Core.Hooks;

public enum EffectPriority
{
    // Effects that happen immediately after state changes
    StateChange,
    // Effects that run after virtual DOM is updated
    AfterRender,
    // Effects that run during initialization
    AfterInit
}