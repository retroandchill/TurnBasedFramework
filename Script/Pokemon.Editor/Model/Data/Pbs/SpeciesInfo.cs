using System.Text.Json.Nodes;
using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record struct LevelUpMoveInfo(FGameplayTag Move, int? Level = null);

public record struct EvolutionConditionInfo(FGameplayTag Species, FGameplayTag Method, 
                                            TSubclassOf<UEvolutionConditionData> DataType, 
                                            JsonObject? Data);

public record SpeciesInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    
    public FText? FormName { get; init; }
    
    public required IReadOnlyList<FGameplayTag> Types { get; init; }
    
    public required IReadOnlyDictionary<FGameplayTag, int> BaseStats { get; init; }
    
    public FGameplayTag GenderRatio { get; init; }
    
    public FGameplayTag GrowthRate { get; init; }
    

    public int BaseExp { get; init; }


    public IReadOnlyDictionary<FGameplayTag, int> EvYield { get; init; } = new Dictionary<FGameplayTag, int>();


    public int CatchRate { get; init; } = 255;


    public int BaseHappiness { get; init; } = 70;
    

    public IReadOnlyList<FGameplayTag> Abilities { get; init; } = [];
    

    public IReadOnlyList<FGameplayTag> HiddenAbilities { get; init; } = [];
    

    public IReadOnlyList<LevelUpMoveInfo> Moves { get; init; } = [];
    

    public IReadOnlyList<FGameplayTag> TutorMoves { get; init; } = [];
    

    public IReadOnlyList<FGameplayTag> EggMoves { get; init; } = [];
    

    public IReadOnlyList<FGameplayTag> EggGroups { get; init; } = [];


    public int HatchSteps { get; init; } = 1;
    

    public IReadOnlyList<FGameplayTag> Offspring { get; init; } = [];
    
    public IReadOnlyList<EvolutionConditionInfo> Evolutions { get; init; } = [];


    public float Height { get; init; } = 1;


    public float Weight { get; init; } = 1;
    

    public FGameplayTag BodyColor { get; init; }
    

    public FGameplayTag BodyShape { get; init; }
    

    public FGameplayTag Habitat { get; init; }
    
    public FText Category { get; init; } = FText.None;
    
    public FText Pokedex { get; init; } = FText.None;
    

    public IReadOnlyList<FGameplayTag> WildHoldItemCommon { get; init; } = [];
    

    public IReadOnlyList<FGameplayTag> WildHoldItemUncommon { get; init; } = [];
    

    public IReadOnlyList<FGameplayTag> WildHoldItemRare { get; init; } = [];
    

    public int Generation { get; init; } = 1;
    
    public FGameplayTagContainer Tags { get; init; }
}
