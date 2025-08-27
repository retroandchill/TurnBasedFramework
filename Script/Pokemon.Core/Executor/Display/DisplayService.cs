using Pokemon.Core.Characters;
using Pokemon.Core.Characters.Components;

namespace Pokemon.Core.Executor.Display;

public interface IDisplayService
{
    ValueTask ProcessLevelUp(UPokemon pokemon, FLevelUpStatChanges changes);
}
