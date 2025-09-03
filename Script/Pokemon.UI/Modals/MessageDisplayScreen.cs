using JetBrains.Annotations;
using TurnBased.UI.Dialogue;
using TurnBased.UI.Input;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonInput;
using UnrealSharp.CommonUI;
using UnrealSharp.SlateCore;
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

    private bool _advancedText;
    
    [UProperty]
    [BindWidget]
    [UsedImplicitly]
    private UOptionSelectionWidget OptionWindow { get; }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Message")]
    public async Task DisplayMessage(FText text, CancellationToken cancellationToken = default)
    {
        _advancedText = false;
        await MessageBox.DisplayDialogue(text, cancellationToken).ConfigureWithUnrealContext();
        _advancedText = true;
    }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Message")]
    public async Task<FChosenOption> DisplayOptions(FText text, IReadOnlyList<FTextOption> options,
                                              int cancelIndex = -1,
                                              CancellationToken cancellationToken = default)
    {
        _advancedText = false;
        var option = await MessageBox.DisplayDialogueWithSelection(text, token => DisplayChoices(options, cancelIndex, token), cancellationToken).ConfigureWithUnrealContext();
        _advancedText = true;
        return option;
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
        var (index, id, _) = await OptionWindow.SelectOptionAsync(cancellationToken).ConfigureWithUnrealContext();
        OptionWindow.DeactivateWidget();
        return new FChosenOption(index, id);
    }
    
    public override void Tick(FGeometry myGeometry, float deltaTime)
    {
        if (_advancedText)
        {
            this.PopContentFromLayer();
        }
    }
}