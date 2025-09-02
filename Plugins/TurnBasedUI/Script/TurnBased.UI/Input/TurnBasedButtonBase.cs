using LanguageExt;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CommonInput;
using UnrealSharp.UnrealSharpCommonUI;

namespace TurnBased.UI.Input;

[UClass(ClassFlags.Abstract)]
public class UTurnBasedButtonBase : UCSCommonButtonBase
{
    [UProperty(PropertyFlags.EditAnywhere, DisplayName = "Button Text", Category = "Display")]
    private Option<FText> ButtonTextOverride { get; set; }

    public FText ButtonText
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Display")]
        get => ButtonTextOverride.Match(x => x, 
            () => InputActionWidget?.DisplayText ?? FText.None);
        [UFunction(FunctionFlags.BlueprintCallable, Category = "Display")]
        set => ButtonTextOverride = !value.Empty ? value : Option<FText>.None;
    }

    public override void PreConstruct(bool isDesignTime)
    {
        UpdateButtonStyle();
        RefreshButtonText();
    }

    protected override void UpdateInputActionWidget()
    {
        UpdateButtonStyle();
        RefreshButtonText();
    }

    protected override void BP_OnInputMethodChanged(ECommonInputType currentInputType)
    {
        UpdateButtonStyle();
    }

    public void RefreshButtonText()
    {
        UpdateButtonText(ButtonText);
    }

    [UFunction(FunctionFlags.BlueprintEvent, Category = "Display")]
    protected virtual void UpdateButtonStyle()
    {
        // Override in subclass 
    }

    [UFunction(FunctionFlags.BlueprintEvent, Category = "Display")]
    protected virtual void UpdateButtonText(FText text)
    {
        // Override in subclass 
    }
}