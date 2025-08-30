using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonUI;
using UnrealSharp.EnhancedInput;
using UnrealSharp.TurnBasedUI;
using UnrealSharp.UnrealSharpCommonUI;

namespace TurnBased.UI;

[UClass(ClassFlags.Abstract)]
public class UDialogueDisplayWidget : UCommonActivatableWidget
{
    [UProperty]
    [BindWidget]
    private UDialogueBox DialogueBox { get; }
    
    [UProperty(PropertyFlags.EditAnywhere, Category = "Actions")]
    private UInputAction AdvanceAction { get; }
    
    public override void Construct()
    {
        RegisterUIActionBinding(new BindUIActionArgs(this, AdvanceAction, false, () =>
        {

        }));
    }
}