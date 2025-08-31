using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CommonUI;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;
using UnrealSharp.SlateCore;
using UnrealSharp.TurnBasedUI;
using UnrealSharp.UMG;
using UnrealSharp.UnrealSharpCommonUI;

namespace TurnBased.UI;

[UClass(ClassFlags.Abstract)]
public class UDialogueDisplayWidget : UCommonActivatableWidget
{
    [UProperty]
    [BindWidget]
    private UDialogueBox DialogueBox { get; }
    
    [UProperty]
    [BindWidgetOptional]
    private UWidget? PauseIndicator { get; }
    
    [UProperty]
    [BindWidget]
    private UScrollBox ScrollBox { get; }
    
    [UProperty(PropertyFlags.EditAnywhere, Category = "Actions")]
    private UInputAction AdvanceAction { get; }
    
    private FUIActionBindingHandle _advanceActionBinding;

    [UProperty(PropertyFlags.EditAnywhere, Category = "Display")]
    [UIMin("0.001f")]
    [ClampMin("0.001f")]
    private float ScrollSpeed { get; } = 0.1f;

    private readonly record struct ScrollStatus(float OriginalScroll, float Timer);

    private ScrollStatus? _scroll;

    public event Action? OnAdvance;

    private bool AwaitingInput
    {
        get;
        set
        {
            field = value;
            _advanceActionBinding.DisplayInActionBar = value;

            PauseIndicator?.Visibility = value ? ESlateVisibility.SelfHitTestInvisible : ESlateVisibility.Collapsed;
        }
    }
    
    public override void Construct()
    {
        _advanceActionBinding = RegisterUIActionBinding(new BindUIActionArgs(this, AdvanceAction, false, OnAdvanceAction));
    }

    public override void Tick(FGeometry myGeometry, float deltaTime)
    {
        base.Tick(myGeometry, deltaTime);

        var bottomScroll = ScrollBox.ScrollOffsetOfEnd;
        if (_scroll.HasValue)
        {
            _scroll = _scroll.Value with { Timer = _scroll.Value.Timer + deltaTime };
            ScrollBox.ScrollOffset =
                float.Lerp(_scroll.Value.OriginalScroll, bottomScroll, _scroll.Value.Timer / ScrollSpeed);

            if (MathLibrary.NearlyEqual_FloatFloat(ScrollBox.ScrollOffset, bottomScroll))
            {
                _scroll = null;
            }
        }
        else if (!MathLibrary.NearlyEqual_FloatFloat(ScrollBox.ScrollOffset, bottomScroll))
        {
            _scroll = new ScrollStatus(ScrollBox.ScrollOffset, 0);
        }
    }

    public override void Destruct()
    {
        RemoveActionBinding(_advanceActionBinding);
    }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Display")]
    public async Task DisplayDialogue(FText text, CancellationToken cancellationToken = default)
    {
        await DialogueBox.PlayLineAsync(text, cancellationToken);

        if (text.Empty) return;
        
        var tcs = new TaskCompletionSource();

        OnAdvance += Advance;
        
        AwaitingInput = true;
        await tcs.Task;
        return;

        void Advance()
        {
            OnAdvance -= Advance;
            tcs.SetResult();
        }
    }
    
    private void OnAdvanceAction()
    {
        if (!AwaitingInput || !DialogueBox.HasFinishedPlayingLine) return;
        
        OnAdvance?.Invoke();
        AwaitingInput = false;
    }
}