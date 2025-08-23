using TurnBasedUI.Glue.Extensions;
using UnrealSharp.CommonUI;
using UnrealSharp.Engine;
using UnrealSharp.GameplayTags;
using UnrealSharp.UnrealSharpAsync;

namespace UnrealSharp.TurnBasedUI;

internal partial class UCSPushWidgetToLayerAsync
{
    private readonly TaskCompletionSource<UCommonActivatableWidget> _tcs = new();
    
    private readonly Action _onAsyncCompleted;

    internal UCSPushWidgetToLayerAsync()
    {
        _onAsyncCompleted = OnAsyncCompleted;
    }

    
    private void OnAsyncCompleted()
    {
        switch (GetResult(out var widget))
        {
            case EAsyncLoadSuccessState.Success:
                _tcs.SetResult(widget);
                break;
            case EAsyncLoadSuccessState.NoSuchLayer:
                _tcs.SetException(new InvalidOperationException($"No layer named {LayerName}"));
                break;
            case EAsyncLoadSuccessState.Cancelled:
                _tcs.SetCanceled();
                break;
            case EAsyncLoadSuccessState.InProgress:
                break;
            default:
                throw new InvalidOperationException("Unknown result enum");
        }
    }
    
    public static async ValueTask<T> PushWidgetToLayerAsync<T>(APlayerController playerController, FGameplayTag layerName, 
                                                               TSoftClassPtr<T> widgetClass, bool suspendInputUntilComplete = true,
                                                               CancellationToken cancellationToken = default)
        where T : UCommonActivatableWidget
    {
        var loader = NewObject<UCSPushWidgetToLayerAsync>(AsyncLoadUtilities.WorldContextObject);
        NativeAsyncUtilities.InitializeAsyncAction(loader, loader._onAsyncCompleted);
        loader.PushWidgetToLayerStack(playerController, layerName, suspendInputUntilComplete, widgetClass.Cast<UCommonActivatableWidget>());
        cancellationToken.Register(loader.Cancel);
        
        var result = await loader._tcs.Task;
        if (result is not T widget)
        {
            throw new InvalidOperationException("Widget is not of expected type");
        }
        
        return widget;
    }
}