using UnrealSharp;
using UnrealSharp.CommonUI;
using UnrealSharp.Engine;
using UnrealSharp.GameplayTags;
using  UnrealSharp.TurnBasedUI;

namespace TurnBasedUI;

public static class WidgetLayerExtensions
{
    public static ValueTask<TWidget> PushContentToLayerAsync<TWidget>(this APlayerController playerController,
                                                                 FGameplayTag layerName,
                                                                 TSoftClassPtr<TWidget> widgetClass,
                                                                 bool suspendInputUntilComplete = true,
                                                                 CancellationToken cancellationToken = default)
        where TWidget : UCommonActivatableWidget
    {
        return UCSPushWidgetToLayerAsync.PushWidgetToLayerAsync(playerController, layerName, widgetClass, suspendInputUntilComplete, cancellationToken);
    }

    public static ValueTask<TWidget> PushContentToLayerAsync<TWidget>(this APlayerController playerController,
                                                                      FGameplayTag layerName,
                                                                      bool suspendInputUntilComplete = true,
                                                                      CancellationToken cancellationToken = default)
        where TWidget : UCommonActivatableWidget
    {
        return playerController.PushContentToLayerAsync<TWidget>(layerName, typeof(TWidget), suspendInputUntilComplete, cancellationToken);
    }
}