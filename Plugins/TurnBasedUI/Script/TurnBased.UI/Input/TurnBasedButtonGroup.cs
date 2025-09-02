using JetBrains.Annotations;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CommonUI;
using UnrealSharp.UnrealSharpCommonUI;

namespace TurnBased.UI.Input;

[UClass]
public class UTurnBasedButtonGroup : UCSCommonButtonGroupBase
{
    
    public event Action<int, UCommonButtonBase>? ButtonAdded;
    
    public event Action<UCommonButtonBase>? ButtonRemoved;
    
    protected override void OnButtonAdded(UCommonButtonBase button)
    {
        ButtonAdded?.Invoke(ButtonCount - 1, button);
    }

    protected override void OnButtonRemoved(UCommonButtonBase button)
    {
        ButtonRemoved?.Invoke(button);
    }
}