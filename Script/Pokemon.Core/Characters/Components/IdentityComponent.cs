using Pokemon.Data;
using Pokemon.Data.Pbs;
using TurnBased.Core;
using TurnBased.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Characters.Components;

[UClass]
public class UIdentityComponent : UTurnBasedUnitComponent
{
    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Identity")]
    [Categories(UTrainerType.TagCategory)]
    public FGameplayTag SpeciesId { get; set; }

    public USpecies Species
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Identity")]
        get => GameData.Species.GetEntry(SpeciesId);
    }

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Identity")]
    public FText Nickname { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Identity")]
    public uint PersonalityValue { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Identity")]
    public int ID { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Identity")]
    public int SecretID { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite, DisplayName = "OT Name", Category = "Identity")]
    public FText OTName { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite, DisplayName = "OT Gender", Category = "Identity")]
    public ETrainerGender OTGender { get; set; }
    
    [ExcludeFromExtensions]
    public UPokemon Pokemon
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Components")]
        get => (UPokemon)OwningUnit;
    }

    public virtual void Initialize(FGameplayTag species)
    {
        SpeciesId = species;
        PersonalityValue = (uint) Random.Shared.Next(ushort.MaxValue + 1) | (uint) (Random.Shared.Next(ushort.MaxValue + 1) << 16);
        
        var trainer = Pokemon.Trainer;
        ID = trainer.ID;
        SecretID = trainer.SecretID;
        OTName = trainer.Name;
        OTGender = trainer.Gender;
    }
}
