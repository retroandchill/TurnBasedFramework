using Pokemon.Data;
using Pokemon.Data.Pbs;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameplayTags;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core.Characters.Components;

[UClass]
public class UIdentityComponent : UTurnBasedUnitComponent
{
    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Identity")]
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

    public virtual void Initialize(FGameplayTag species)
    {
        SpeciesId = species;
        PersonalityValue = (uint) Random.Shared.Next(ushort.MaxValue + 1) | (uint) (Random.Shared.Next(ushort.MaxValue + 1) << 16);
        
        // TODO: Calculate values from the trainer (which is going to be the outer)
    }
}
