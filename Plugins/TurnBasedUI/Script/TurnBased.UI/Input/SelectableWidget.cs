using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LanguageExt;
using UnrealSharp.Attributes;
using UnrealSharp.CommonUI;
using UnrealSharp.CoreUObject;
using UnrealSharp.UMG;

namespace TurnBased.UI.Input;

[UClass(ClassFlags.Abstract)]
public class USelectableWidget : UCommonActivatableWidget
{
    private const int NoFocusIndex = -1;
    
    [UProperty]
    [UsedImplicitly]
    protected UTurnBasedButtonGroup Buttons { get; private set; }
    
    [UProperty(PropertyFlags.EditAnywhere, Category = "Selection")]
    public Option<int> DesiredFocusIndex { get; private set; }

    public override void PreConstruct(bool isDesignTime)
    {
#if WITH_EDITOR
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Buttons?.RemoveAll();
#endif
        
        Buttons = NewObject<UTurnBasedButtonGroup>(this);
        Buttons.OnButtonBaseClicked += [UFunction] (buttons, index) =>
        {
            DesiredFocusIndex = index;
        };
    }
    
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Selection")]
    public bool TryGetDesiredFocusIndex(out int outIndex)
    {
        outIndex = DesiredFocusIndex.Match(x => x, () => NoFocusIndex);
        return outIndex != NoFocusIndex;
    }
    
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Selection")]
    public void SetDesiredFocusIndex(int index)
    {
        DesiredFocusIndex = Math.Clamp(index, 0, Buttons.ButtonCount - 1);
    }
    
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Selection")]
    public void ClearDesiredFocusIndex()
    {
        DesiredFocusIndex = Option<int>.None;
    }

    protected override UWidget? BP_GetDesiredFocusTarget()
    {
        return DesiredFocusIndex.Filter(i => i >= 0 && i < Buttons.ButtonCount)
            .Match(Buttons.GetButtonBaseAtIndex, () => null);
    }
}