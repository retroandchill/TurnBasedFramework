using Pokemon.Core.Characters.Components;
using Pokemon.Data.Pbs;
using UnrealSharp.Attributes;
using UnrealSharp.GameplayTags;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core.Characters;

[UClass(ClassFlags.Abstract)]
public class UPokemon : UTurnBasedUnit
{
    public UIdentityComponent IdentityComponent { get; private set; }
    
    public UStatComponent StatComponent { get; private set; }
}

public static class PokemonExtensions
{
    extension(UPokemon pokemon)
    {
        public FGameplayTag Species => pokemon.IdentityComponent.Species;
        
        public USpecies SpeciesData => pokemon.IdentityComponent.SpeciesData;
        
        public int Level => pokemon.StatComponent.Level;
    }
}