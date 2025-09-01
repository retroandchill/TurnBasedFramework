using TurnBased.UI;
using TurnBased.UI.Dialogue;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonUI;
using UnrealSharp.EnhancedInput;
using UnrealSharp.TurnBasedUI;
using UnrealSharp.UMG;

namespace Pokemon.UI.Modals;

[UStruct]
public readonly record struct FTextOption(
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    FName Id, 
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    FText Text);

[UStruct]
public readonly record struct FChosenOption(
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    int Index,
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    FName Id);

[UClass]
public class UMessageDisplayScreen : UCommonActivatableWidget
{
    [UProperty]
    [BindWidget]
    private UDialogueDisplayWidget MessageBox { get; }
    
    [UProperty]
    [BindWidget]
    private UOptionSelectionWidget OptionWindow { get; }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Message")]
    public Task DisplayMessage(FText text, CancellationToken cancellationToken = default)
    {
        return MessageBox.DisplayDialogue(text, cancellationToken);
    }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Message")]
    public Task<FChosenOption> DisplayOptions(FText text, IReadOnlyList<FTextOption> options,
                                              bool allowCancel = false,
                                              CancellationToken cancellationToken = default)
    {
        return MessageBox.DisplayDialogueWithSelection(text, token => DisplayChoices(options, allowCancel, token), cancellationToken);
    }

    private async Task<FChosenOption> DisplayChoices(IEnumerable<FTextOption> options,
                                                     bool allowCancel = false, 
                                                     CancellationToken cancellationToken = default)
    {
        
        OptionWindow.Options = options.Select(o => new FSelectableOption(o.Id, o.Text)).ToArray();
        OptionWindow.Visibility = ESlateVisibility.Visible;
        OptionWindow.ActivateWidget();
        var (index, id, _) = await OptionWindow.SelectOptionAsync(cancellationToken);
        return new FChosenOption(index, id);
    }
}