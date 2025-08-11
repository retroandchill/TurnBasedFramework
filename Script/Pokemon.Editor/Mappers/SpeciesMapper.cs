using System.Collections.Immutable;
using System.Text.Json.Nodes;
using GameDataAccessTools.Core.Serialization;
using LanguageExt;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class SpeciesMapper
{
    private const int EvolutionLearnLevel = 0;
    
    public static USpecies ToSpecies(this SpeciesInfo speciesInfo, UObject? outer = null)
    {
        return speciesInfo.ToSpeciesInitializer(outer);
    }

    [MapProperty(nameof(USpecies.EvYield), nameof(SpeciesInfo.EvYield), Use = nameof(MapEvYieldDictionary))]
    public static partial SpeciesInfo ToSpeciesInfo(this USpecies species);
    
    [MapperIgnoreTarget(nameof(SpeciesInitializer.Evolutions))]
    [MapProperty(nameof(SpeciesInfo.Evolutions), nameof(SpeciesInitializer.EvolutionConditionInitializers), Use = nameof(ToEvolutionConditionInitializers))]
    [MapProperty(nameof(SpeciesInfo.EvYield), nameof(SpeciesInitializer.EvYield), Use = nameof(MapEvYieldList))]
    private static partial SpeciesInitializer ToSpeciesInitializer(this SpeciesInfo species, UObject? outer = null);

    private static LevelUpMoveInfo ToLevelUpMoveInfo(this FLevelUpMove move)
    {
        return move.Match(level => new LevelUpMoveInfo(level, move.Move), () => new LevelUpMoveInfo(EvolutionLearnLevel, move.Move));
    }
    
    private static FLevelUpMove ToLevelUpMove(this LevelUpMoveInfo move)
    {
        return move.Level == EvolutionLearnLevel
            ? FLevelUpMove.LevelUp(move.Level, move.Move)
            : FLevelUpMove.Evolution(move.Move);
    }
    
    private static FText? ToNullableText(this Option<FText> value) => value.Match<FText?>(v => v, () => null);

    private static Option<FText> ToTextOption(this FText? value) => value ?? Option<FText>.None;

    private static EvolutionConditionInfo ToEvolutionConditionInfo(this FEvolutionCondition condition)
    {
        TSubclassOf<UEvolutionConditionData> conditionData;
        JsonObject? data;
        if (condition.Data is not null)
        {
            conditionData = condition.Data.Class;
            data = condition.Data.SerializeObjectToJson();
        }
        else
        {
            conditionData = default;
            data = null;
        }

        return new EvolutionConditionInfo(condition.Species.TagName, condition.Method, conditionData, data);
    }

    private static IEnumerable<Func<UObject, FEvolutionCondition>> ToEvolutionConditionInitializers(
        this IReadOnlyList<EvolutionConditionInfo> conditions)
    {
        return conditions.Select(condition => (Func<UObject, FEvolutionCondition>)(y => condition.ToEvolutionCondition(y)));
    }

    private static FEvolutionCondition ToEvolutionCondition(this EvolutionConditionInfo condition, UObject? outer = null)
    {
        return new FEvolutionCondition
        {
            Species = new FGameplayTag(condition.Species),
            Method = condition.Method,
            Data = condition.Data?.DeserializeObjectFromJson(outer, condition.DataType)
        };
    }

    private static IReadOnlyDictionary<FGameplayTag, int> MapEvYieldList(IReadOnlyList<EvYield> evYields)
    {
        return evYields.ToDictionary(x => x.Stat, x => x.Amount);
    }
    
    private static IReadOnlyList<EvYield> MapEvYieldDictionary(IReadOnlyDictionary<FGameplayTag, int> evYields)
    {
        return evYields.Select(x => new EvYield(x.Key, x.Value)).ToImmutableList();
    }
    
    private static FName GameplayTagToName(FGameplayTag tag)
    {
        return tag.TagName;
    }
}
