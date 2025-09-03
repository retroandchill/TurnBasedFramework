using Pokemon.Core.Characters;
using Pokemon.Core.Characters.Components;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;

namespace Pokemon.Core.Executor.Display;

public class NullDisplayService : IDisplayService
{
    public Task DisplayMessage(FText text, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public ValueTask ProcessLevelUp(UPokemon pokemon, FLevelUpStatChanges changes)
    {
        return ValueTask.CompletedTask;
    }
}
