using Pokemon.Core.Characters;
using Pokemon.Core.Characters.Components;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;

namespace Pokemon.Core.Executor.Display;

[UInterface(CannotImplementInterfaceInBlueprint = true)]
public interface IDisplayService
{
    public static IDisplayService Instance => UGameplayStatics.GameInstance.GetGameInstanceSubsystem<UPokemonSubsystem>().DisplayActions;

    Task DisplayMessage(FText text, bool autoRemove = true, CancellationToken cancellationToken = default);
    
    ValueTask ProcessLevelUp(UPokemon pokemon, FLevelUpStatChanges changes);
}
