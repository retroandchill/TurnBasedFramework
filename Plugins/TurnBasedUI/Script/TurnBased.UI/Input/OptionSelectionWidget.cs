using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LanguageExt;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonUI;
using UnrealSharp.EnhancedInput;
using UnrealSharp.TurnBasedUI;
using UnrealSharp.UnrealSharpCommonUI;

namespace TurnBased.UI.Input;

[UMultiDelegate]
public delegate void OnOptionClicked(int index, FName id, UTurnBasedButtonBase button);

[UStruct]
public readonly record struct FSelectableOption
{
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    public required FName Id { get; init; }
    
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    public required Option<FText> Text { get; init; }
    
    [field: UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadWrite)]
    public UInputAction? InputAction { get; init; }
    

    [SetsRequiredMembers]
    public FSelectableOption(FName id, FText text, UInputAction? inputAction = null)
    {
        Id = id;
        Text = text;
        InputAction = inputAction;
    }
    
    [SetsRequiredMembers]
    public FSelectableOption(FName id, UInputAction inputAction)
    {
        Id = id;
        Text = Option<FText>.None;
        InputAction = inputAction;
    }
    
    public void Deconstruct(out FName id, out Option<FText> text, out UInputAction? inputAction)
    {
        id = Id;
        text = Text;
        inputAction = InputAction;
    }
}

public readonly record struct SelectedOption(int Index, FName Id, UTurnBasedButtonBase Button);

[UClass(ClassFlags.Abstract)]
public class UOptionSelectionWidget : USelectableWidget
{
    [UProperty(PropertyFlags.EditAnywhere, Category = "Selection")]
    [UsedImplicitly]
    private TArray<FSelectableOption> SelectableOptions { get; }

    public IReadOnlyList<FSelectableOption> Options
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Selection")]
        get => SelectableOptions;
        [UFunction(FunctionFlags.BlueprintCallable, Category = "Selection")]
        set {
            SelectableOptions.Clear();
            foreach (var option in value)
            {
                SelectableOptions.Add(option);
            }

            CreateOptions();
        }
    }
    
    [UProperty(PropertyFlags.EditAnywhere, Category = "Selection")]
    [UsedImplicitly]
    private TSubclassOf<UTurnBasedButtonBase> ButtonClass { get; }
    
    [UProperty(PropertyFlags.EditAnywhere, Category = "Style")]
    [UMetaData("EditInlineToggle")]
    [UsedImplicitly]
    public bool OverrideButtonStyle { get; }

    [UProperty(PropertyFlags.EditAnywhere, Category = "Style")]
    [EditCondition(nameof(OverrideButtonStyle))]
    [UsedImplicitly]
    public TSubclassOf<UCommonButtonStyle> ButtonStyle { get; }
    
    public event OnOptionClicked? NativeOnOptionClicked;
    
    [UProperty(PropertyFlags.BlueprintAssignable)]
    [UsedImplicitly]
    private TMulticastDelegate<OnOptionClicked> OnOptionClicked { get; }
    
    public override void PreConstruct(bool isDesignTime)
    {
        base.PreConstruct(isDesignTime);
        Buttons.ButtonAdded += (index, button) => PlaceOptionIntoWidget(index, SelectableOptions[index].Id, (UTurnBasedButtonBase)button);
        Buttons.ButtonRemoved += button => button.RemoveFromParent();
        
        Buttons.OnButtonBaseClicked += [UFunction](button, index) =>
        {
            var (id, _, _) = SelectableOptions[index];
            var newButton = (UTurnBasedButtonBase)button;
            NativeOnOptionClicked?.Invoke(index, id, newButton);
            OnOptionClicked.Invoke(index, id, newButton);
        };

        CreateOptions();
    }

    [UFunction(FunctionFlags.BlueprintEvent, Category = "Selection")]
    protected virtual void PlaceOptionIntoWidget(int index, FName id, UTurnBasedButtonBase button)
    {
        
    }

    private void CreateOptions()
    {
        Buttons.RemoveAll();
        foreach (var (_, text, inputAction) in SelectableOptions)
        {
            var newButton = UChildWidgetUtils.CreateChildWidget(this, ButtonClass);

            text.IfSome(t => newButton.ButtonText = t);
            if (inputAction is not null)
            {
                newButton.TriggeringEnhancedInputAction = inputAction;
            }

            if (OverrideButtonStyle)
            {
                newButton.Style = ButtonStyle;
            }
            
            Buttons.AddWidget(newButton);
        }
    }
    
    public async Task<SelectedOption> SelectOptionAsync(CancellationToken cancellationToken = default)
    {
        var (button, index) = await Buttons.SelectButtonAsync(cancellationToken).ConfigureWithUnrealContext();
        return new SelectedOption(index, Options[index].Id, (UTurnBasedButtonBase)button);
    }
}