using Pokemon.Core.Characters;
using Pokemon.Core.Characters.Components;
using Pokemon.Core.Executor.Display;
using Pokemon.UI.Modals;
using TurnBased.UI;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.TurnBasedUI;

namespace Pokemon.UI.Async;

public class DefaultDisplayService : IDisplayService
{
    public async Task DisplayMessage(FText text, bool autoRemove = true, CancellationToken cancellationToken = default)
    {
        var playerController = UTurnBasedUIExtensions.PrimaryPlayerController;
        var topWidget = playerController.GetTopWidgetForPlayer(GameplayTags.UI_Layer_Modal);
        if (topWidget is not UMessageDisplayScreen messageDisplayScreen)
        {
            messageDisplayScreen = await playerController.PushContentToLayerAsync(
                GameplayTags.UI_Layer_Modal, UObject.GetDefault<UPokemonUISettings>().MessageDisplayScreen, 
                cancellationToken: cancellationToken).ConfigureWithUnrealContext();
        }
        
        await messageDisplayScreen.DisplayMessage(text, autoRemove, cancellationToken).ConfigureWithUnrealContext();
    }

    public ValueTask ProcessLevelUp(UPokemon pokemon, FLevelUpStatChanges changes)
    {
        throw new NotImplementedException();
    }
}