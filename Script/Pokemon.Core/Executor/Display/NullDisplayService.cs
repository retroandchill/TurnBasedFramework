using Pokemon.Core.Characters;
using Pokemon.Core.Characters.Components;

namespace Pokemon.Core.Executor.Display;

public class NullDisplayService : IDisplayService
{
    public ValueTask ProcessLevelUp(UPokemon pokemon, FLevelUpStatChanges changes)
    {
        return ValueTask.CompletedTask;
    }
}
