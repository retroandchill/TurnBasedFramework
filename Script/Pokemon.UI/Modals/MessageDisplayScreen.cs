using TurnBased.UI;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonUI;

namespace Pokemon.UI.Modals;

[UClass]
public class UMessageDisplayScreen : UCommonActivatableWidget
{
    [UProperty]
    [BindWidget]
    private UDialogueDisplayWidget MessageBox { get; }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Message")]
    public async Task DisplayMessage(FText text, CancellationToken cancellationToken = default)
    {
        MessageBox.ActivateWidget();
        await MessageBox.DisplayDialogue(text, cancellationToken).ConfigureWithUnrealContext();
    }
}