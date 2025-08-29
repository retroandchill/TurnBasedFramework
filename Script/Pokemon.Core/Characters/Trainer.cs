using Pokemon.Data;
using Pokemon.Data.Pbs;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Characters;

[UClass]
public class UTrainer : UObject
{
    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Trainer")]
    [Categories(UTrainerType.TagCategory)]
    public FGameplayTag TrainerTypeId { get; set; }

    public UTrainerType TrainerType
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Trainer")]
        get => GameData.TrainerTypes.GetEntry(TrainerTypeId);
        [UFunction(FunctionFlags.BlueprintCallable, Category = "Trainer")]
        set => TrainerTypeId = value.Id;
    }
    
    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Trainer")]
    public FText Name { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "Trainer")]
    public FText FullName
    {
        get
        {
            ReadOnlySpan<char> trainerTypeName = TrainerType.DisplayName;
            ReadOnlySpan<char> trainerName = Name;
            return new FText($"{trainerTypeName} {trainerName}");
        }
    }

    public ETrainerGender Gender
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Trainer")]
        get => TrainerType.Gender;
    }
    
    [UProperty]
    private uint FullID { get; set; }

    private const uint TrainerIdMask = 1000000;
    
    public int ID
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Trainer")]
        get => (int) (FullID % TrainerIdMask);
    }
    
    public int SecretID
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Trainer")]
        get => (int) (FullID / TrainerIdMask);
    }

    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Party")]
    public TArray<UPokemon> Party { get; }

    public int Payout
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Trainer")]
        get
        {
            ArgumentOutOfRangeException.ThrowIfZero(Party.Count);
            return TrainerType.BasePayout * Party[^1].Level;
        }
    }

    public bool IsPartyFull
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Party")]
        get => Party.Count >= PokemonStatics.MaxPartySize;
    }

    public void Initialize(FGameplayTag trainerTypeId, FText name)
    {
        TrainerTypeId = trainerTypeId;
        Name = name;
        FullID = (uint) Random.Shared.Next(ushort.MaxValue + 1) | (uint) (Random.Shared.Next(ushort.MaxValue + 1) << 16);
    }
}