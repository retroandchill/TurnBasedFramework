using System.Text.Json.Nodes;
using GameDataAccessTools.Core.Serialization;
using LanguageExt;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class SpeciesMapper
{
    public static USpecies ToSpecies(this SpeciesInfo speciesInfo, UObject? outer = null)
    {
        return speciesInfo.ToSpeciesInitializer(outer);
    }

    public static partial SpeciesInfo ToSpeciesInfo(this USpecies species);
    
    [MapperIgnoreTarget(nameof(SpeciesInitializer.Evolutions))]
    [MapProperty(nameof(SpeciesInfo.Evolutions), nameof(SpeciesInitializer.EvolutionConditionInitializers), Use = nameof(ToEvolutionConditionInitializers))]
    private static partial SpeciesInitializer ToSpeciesInitializer(this SpeciesInfo species, UObject? outer = null);

    private static LevelUpMoveInfo ToLevelUpMoveInfo(this FLevelUpMove move)
    {
        return move.Match(level => new LevelUpMoveInfo(move.Move, level), () => new LevelUpMoveInfo(move.Move));
    }
    
    private static FLevelUpMove ToLevelUpMove(this LevelUpMoveInfo move)
    {
        return move.Level.HasValue
            ? FLevelUpMove.LevelUp(move.Level.Value, move.Move)
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

        return new EvolutionConditionInfo(condition.Species, condition.Method, conditionData, data);
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
            Species = condition.Species,
            Method = condition.Method,
            Data = condition.Data?.DeserializeObjectFromJson(null, condition.DataType)
        };
    }
}
