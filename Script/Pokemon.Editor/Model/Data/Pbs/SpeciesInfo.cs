using System.Text.Json.Nodes;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Serializers.Pbs.Attributes;
using Pokemon.Editor.Serializers.Pbs.Converters;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public readonly record struct EvYield([property: PbsScalar<MainStatConverter>] FGameplayTag Stat, 
                                      [property: PbsRange<int>(1)]int Amount);

public record struct LevelUpMoveInfo([property: PbsGameplayTag(UMove.TagCategory)] FGameplayTag Move, 
                                     [property: PbsRange<int>(0)] int Level = 0);

[PbsScalar<EvolutionMethodConverter>]
public record struct EvolutionConditionInfo(FName Species, 
                                            FGameplayTag Method, 
                                            TSubclassOf<UEvolutionConditionData> DataType, 
                                            JsonObject? Data);

public record SpeciesInfo
{
    [PbsKey]
    [PbsGameplayTag(USpecies.TagCategory, Create = true)]
    public required FGameplayTag Id { get; init; }

    [PbsIndex] public int RowIndex { get; init; }

    [PbsName("Name")] public FText DisplayName { get; init; } = "Unnamed";

    public FText? FormName { get; init; }

    [PbsGameplayTag(UType.TagCategory, Create = true)]
    public IReadOnlyList<FGameplayTag> Types { get; init; } = [GameplayTags.Pokemon_Data_Types_NORMAL];

    [PbsScalar<BaseStatsConverter>]
    public IReadOnlyDictionary<FGameplayTag, int> BaseStats { get; init; } = new Dictionary<FGameplayTag, int>
    {
        [GameplayTags.Pokemon_Data_Stats_Main_HP] = 1,
        [GameplayTags.Pokemon_Data_Stats_MainBattle_ATTACK] = 1,
        [GameplayTags.Pokemon_Data_Stats_MainBattle_DEFENSE] = 1,
        [GameplayTags.Pokemon_Data_Stats_MainBattle_SPECIAL_ATTACK] = 1,
        [GameplayTags.Pokemon_Data_Stats_MainBattle_SPECIAL_DEFENSE] = 1,
        [GameplayTags.Pokemon_Data_Stats_MainBattle_SPEED] = 1
    };

    [PbsGameplayTag(UGenderRatio.TagCategory, Create = true)]
    public FGameplayTag GenderRatio { get; init; } = GameplayTags.Pokemon_Data_GenderRatios_Female50Percent;

    [PbsGameplayTag(UGrowthRate.TagCategory, Create = true)]
    public FGameplayTag GrowthRate { get; init; } = GameplayTags.Pokemon_Data_GrowthRates_Medium;

    public int BaseExp { get; init; } = 100;

    public IReadOnlyList<EvYield> EvYield { get; init; } = [];

    [PbsRange<int>(3, 255)]
    public int CatchRate { get; init; } = 255;

    [PbsRange<int>(0, 255)]
    public int BaseHappiness { get; init; } = 70;
    
    [PbsGameplayTag(UAbility.TagCategory)]
    public IReadOnlyList<FGameplayTag> Abilities { get; init; } = [];
    
    [PbsGameplayTag(UAbility.TagCategory)]
    public IReadOnlyList<FGameplayTag> HiddenAbilities { get; init; } = [];
    
    public IReadOnlyList<LevelUpMoveInfo> Moves { get; init; } = [];
    
    [PbsGameplayTag(UMove.TagCategory)]
    public IReadOnlyList<FGameplayTag> TutorMoves { get; init; } = [];
    
    [PbsGameplayTag(UMove.TagCategory)]
    public IReadOnlyList<FGameplayTag> EggMoves { get; init; } = [];
    

    [PbsGameplayTag(UEggGroup.TagCategory)]
    public IReadOnlyList<FGameplayTag> EggGroups { get; init; } = [GameplayTags.Pokemon_Data_EggGroups_Undiscovered];


    public int HatchSteps { get; init; } = 1;
    
    [PbsGameplayTag(UItem.TagCategory)]
    public FGameplayTag Incense { get; init; }
    

    [PbsGameplayTag(USpecies.TagCategory)]
    public IReadOnlyList<FName> Offspring { get; init; } = [];
    
    [PbsAllowMultiple]
    public IReadOnlyList<EvolutionConditionInfo> Evolutions { get; init; } = [];

    [PbsRange<float>(0.1f)]
    public float Height { get; init; } = 0.1f;

    [PbsRange<float>(0.1f)]
    public float Weight { get; init; } = 0.1f;

    [PbsName("Color")]
    [PbsGameplayTag(UBodyColor.TagCategory)]
    public FGameplayTag BodyColor { get; init; } = GameplayTags.Pokemon_Data_BodyColors_Red;
    
    [PbsName("Shape")]
    [PbsGameplayTag(UBodyShape.TagCategory)]
    public FGameplayTag BodyShape { get; init; } = GameplayTags.Pokemon_Data_BodyShapes_Head;
    
    [PbsGameplayTag(UHabitat.TagCategory)]
    public FGameplayTag Habitat { get; init; }
    
    public FText Category { get; init; } = "???";
    
    public FText Pokedex { get; init; } = "???";
    
    [PbsGameplayTag(UItem.TagCategory)]
    public IReadOnlyList<FGameplayTag> WildHoldItemCommon { get; init; } = [];
    
    [PbsGameplayTag(UItem.TagCategory)]
    public IReadOnlyList<FGameplayTag> WildHoldItemUncommon { get; init; } = [];
    
    [PbsGameplayTag(UItem.TagCategory)]
    public IReadOnlyList<FGameplayTag> WildHoldItemRare { get; init; } = [];
    
    [PbsRange<int>(1)]
    public int Generation { get; init; } = 1;
    
    [PbsName("Flags")]
    [PbsGameplayTag(USpecies.MetadataCategory, Create = true, Separator = "_")]
    public FGameplayTagContainer Tags { get; init; }
}
