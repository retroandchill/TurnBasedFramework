using System.Diagnostics.CodeAnalysis;
using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using LanguageExt;
using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Pbs;

[UStruct]
public readonly struct FLevelUpMove
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    private readonly bool _evolutionMove;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [UMetaData("ClampMin", "1")]
    [UMetaData("UIMin", "1")]
    [UMetaData("EditCondition", $"!{nameof(_evolutionMove)}")]
    [UMetaData("EditConditionHides")]
    private readonly int _level;
    
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [UMetaData("Categories", UMove.TagCategory)]
    public FGameplayTag Move { get; init; }

    private FLevelUpMove(int level, FGameplayTag move)
    {
        _evolutionMove = false;
        _level = level;
        Move = move;
    }

    private FLevelUpMove(FGameplayTag move)
    {
        _evolutionMove = true;
        Move = move;
        _level = 0;
    }

    public static FLevelUpMove LevelUp(int level, FGameplayTag move) => new(level, move);
    
    public static FLevelUpMove Evolution(FGameplayTag move) => new(move);

    public void Match(Action<int> onLevelUp, Action onEvolution)
    {
        if (_evolutionMove)
        {
            onEvolution();
        }
        else
        {
            onLevelUp(_level);
        }
    }

    public T Match<T>(Func<int, T> onLevelUp, Func<T> onEvolution)
    {
        return _evolutionMove ? onEvolution() : onLevelUp(_level);
    }
}

[UStruct]
public readonly struct FEvolutionCondition
{
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [field: UMetaData("Categories", USpecies.TagCategory)]
    public FGameplayTag Species { get; init; }
    
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [field: UMetaData("Categories", UEvolutionMethod.TagCategory)]
    public FGameplayTag Method { get; init; }

    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere | PropertyFlags.Instanced)]
    public UEvolutionConditionData Data { get; init; }
    
    public bool IsValid => Method.IsValid && Data is not null 
                                          && GameData.EvolutionMethods.GetEntry(Method)
                                              .ConditionType.IsChildOf(Data.GetType());

    public UEvolutionConditionData GetData<T>() where T : UEvolutionConditionData
    {
        if (TryGetData<T>(out var data))
        {
            return data;
        }
        
        throw new InvalidOperationException($"Evolution condition data is not of type {typeof(T)}");
    }

    public bool TryGetData<T>([NotNullWhen(true)] out T? data) where T : UEvolutionConditionData
    {
        if (GameData.EvolutionMethods.TryGetEntry(Method, out var entry) && 
            entry.ConditionType.IsChildOf(typeof(T)) && Data is T typedData)
        {
            data = typedData;
            return true;
        }
        
        data = null;
        return false;
    }
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class USpecies : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.Species";
    public const string MetadataCategory = "Pokemon.Metadata.Species";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [UMetaData("Categories", TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public Option<FText> FormName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Basic")]
    [UMetaData("Categories", UType.TagCategory)]
    public IReadOnlyList<FGameplayTag> Types { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    [UMetaData("Categories", UType.TagCategory)]
    [ClampMin("1")]
    [UMetaData("UIMin", "1")]
    public IReadOnlyDictionary<FGameplayTag, int> BaseStats { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Basic")]
    [UMetaData("Categories", UGenderRatio.TagCategory)]
    public FGameplayTag GenderRatio { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Basic")]
    [UMetaData("Categories", UGrowthRate.TagCategory)]
    public FGameplayTag GrowthRate { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Drops")]
    [UMetaData("Categories", UType.TagCategory)]
    [ClampMin("1")]
    [UMetaData("UIMin", "1")]
    public int BaseExp { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, DisplayName = "EV Yield",  Category = "Stats")]
    [UMetaData("Categories", UType.TagCategory)]
    [ClampMin("0")]
    [UMetaData("UIMin", "0")]
    public IReadOnlyDictionary<FGameplayTag, int> EvYield { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Capture")]
    [UMetaData("Categories", UType.TagCategory)]
    [ClampMin("3")]
    [UMetaData("UIMin", "3")]
    public int CatchRate { get; init; } = 255;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Basic")]
    [UMetaData("Categories", UType.TagCategory)]
    [ClampMin("0")]
    [UMetaData("UIMin", "0")]
    public int BaseHappiness { get; init; } = 70;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Abilities")]
    [UMetaData("Categories", UAbility.TagCategory)]
    public IReadOnlyList<FGameplayTag> Abilities { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Abilities")]
    [UMetaData("Categories", UAbility.TagCategory)]
    public IReadOnlyList<FGameplayTag> HiddenAbilities { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Moves")]
    public IReadOnlyList<FLevelUpMove> Moves { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Moves")]
    [UMetaData("Categories", UMove.TagCategory)]
    public IReadOnlyList<FGameplayTag> TutorMoves { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Moves")]
    [UMetaData("Categories", UMove.TagCategory)]
    public IReadOnlyList<FGameplayTag> EggMoves { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Breeding")]
    [UMetaData("Categories", UEggGroup.TagCategory)]
    public IReadOnlyList<FGameplayTag> EggGroups { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Breeding")]
    [ClampMin("1")]
    [UMetaData("UIMin", "1")]
    public int HatchSteps { get; init; } = 1;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Breeding")]
    [UMetaData("Categories", TagCategory)]
    public IReadOnlyList<FGameplayTag> Offspring { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Evolution")]
    public IReadOnlyList<FEvolutionCondition> Evolutions { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Pokédex")]
    [ClampMin("0")]
    [UMetaData("UIMin", "0")]
    public float Height { get; init; } = 1;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Pokédex")]
    [ClampMin("0")]
    [UMetaData("UIMin", "0")]
    public float Weight { get; init; } = 1;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Pokédex")]
    [UMetaData("Categories", UBodyColor.TagCategory)]
    public FGameplayTag BodyColor { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Pokédex")]
    [UMetaData("Categories", UBodyShape.TagCategory)]
    public FGameplayTag BodyShape { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Pokédex")]
    [UMetaData("Categories", UHabitat.TagCategory)]
    public FGameplayTag Habitat { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Pokédex")]
    public FText Category { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Pokédex")]
    public FText Pokedex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Drops")]
    [UMetaData("Categories", UItem.TagCategory)]
    public IReadOnlyList<FGameplayTag> WildHoldItemCommon { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Drops")]
    [UMetaData("Categories", UItem.TagCategory)]
    public IReadOnlyList<FGameplayTag> WildHoldItemUncommon { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Drops")]
    [UMetaData("Categories", UItem.TagCategory)]
    public IReadOnlyList<FGameplayTag> WildHoldItemRare { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata")]
    [ClampMin("1")]
    [UMetaData("UIMin", "1")]
    public int Generation { get; init; } = 1;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata")]
    [UMetaData("Categories", MetadataCategory)]
    public FGameplayTagContainer Tags { get; init; }
    
}