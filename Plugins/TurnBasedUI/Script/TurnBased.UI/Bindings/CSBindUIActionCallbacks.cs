using UnrealSharp.Attributes;
using UnrealSharp.TurnBasedUI;

namespace TurnBased.UI.Bindings;

[UClass]
internal sealed class UBindUIActionCallbacks : UCSBindUIActionCallbacksBase
{
    public Action? OnExecuteAction { get; set; }
    public Action? OnHoldActionPressed { get; set; }
    public Action? OnHoldActionReleased { get; set; }
    public Action<float>? OnHoldActionProgressed { get; set; }
    
    public override void InvokeOnExecuteAction()
    {
        OnExecuteAction?.Invoke();
    }

    public override void InvokeOnHoldActionProgressed(float progress)
    {
        OnHoldActionProgressed?.Invoke(progress);
    }

    public override void InvokeOnHoldActionPressed()
    {
        OnHoldActionPressed?.Invoke();
    }
    
    public override void InvokeOnHoldActionReleased()
    {
        OnHoldActionReleased?.Invoke();   
    }
}