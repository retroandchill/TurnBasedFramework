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
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public FGameplayTag Species { get; set; }

    public USpecies SpeciesData
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Identity")]
        get => GameData.Species.GetEntry(Species);
    }

    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public FText Nickname { get; set; }
    
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public uint PersonalityValue { get; set; }
    
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public int ID { get; set; }
    
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public int SecretID { get; set; }
    
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public FText OTName { get; set; }
    
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public ETrainerGender OTGender { get; set; }

}