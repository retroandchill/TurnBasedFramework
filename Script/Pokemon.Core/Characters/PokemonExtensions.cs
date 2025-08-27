using Pokemon.Core.Characters.Components;

namespace Pokemon.Core.Characters;

public static partial class PokemonExtensions
{
    extension(UPokemon pokemon)
    {
        public ValueTask DisplayLevelUp(FLevelUpStatChanges changes)
        {
            return pokemon
                .GetGameInstanceSubsystem<UPokemonSubsystem>()
                .DisplayActions.ProcessLevelUp(pokemon, changes);
        }
    }
}
