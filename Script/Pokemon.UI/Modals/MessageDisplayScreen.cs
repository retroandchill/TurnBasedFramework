using JetBrains.Annotations;
using TurnBased.UI.Dialogue;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonInput;
using UnrealSharp.CommonUI;
using UnrealSharp.TurnBasedUI;

namespace Pokemon.UI.Modals;

[UStruct]
public readonly record struct FTextOption(
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    FName Id, 
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    FText Text);

[UStruct]
[PublicAPI]
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
    [UsedImplicitly]
    private UDialogueDisplayWidget MessageBox { get; }
    
    [UProperty]
    [BindWidget]
    [UsedImplicitly]
    private UOptionSelectionWidget OptionWindow { get; }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Message")]
    public Task DisplayMessage(FText text, CancellationToken cancellationToken = default)
    {
        return MessageBox.DisplayDialogue(text, cancellationToken);
    }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Message")]
    public Task<FChosenOption> DisplayOptions(FText text, IReadOnlyList<FTextOption> options,
                                              int cancelIndex = -1,
                                              CancellationToken cancellationToken = default)
    {
        return MessageBox.DisplayDialogueWithSelection(text, token => DisplayChoices(options, cancelIndex, token), cancellationToken);
    }

    private async Task<FChosenOption> DisplayChoices(IEnumerable<FTextOption> options, int cancelIndex, 
                                                     CancellationToken cancellationToken = default)
    {
        var inputData = await GetDefault<UCommonInputSettings>().InputData.LoadAsync();
        var backAction = inputData.DefaultObject.EnhancedInputBackAction;

        var optionsList = options
            .Select((o, i) => new FSelectableOption(o.Id, o.Text, i == cancelIndex ? backAction : null))
            .ToArray();

        if (optionsList.Length == 0)
        {
            throw new InvalidOperationException("No options provided");
        }
        
        OptionWindow.Options = optionsList;
        OptionWindow.SetDesiredFocusIndex(0);
        OptionWindow.ActivateWidget();
        var (index, id, _) = await OptionWindow.SelectOptionAsync(cancellationToken);
        OptionWindow.DeactivateWidget();
        return new FChosenOption(index, id);
    }
}