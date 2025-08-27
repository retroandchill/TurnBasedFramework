using LanguageExt.UnsafeValueAccess;
using Pokemon.Core.Characters.Components;
using TurnBased.Core;
using UnrealSharp.GameplayTags;

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