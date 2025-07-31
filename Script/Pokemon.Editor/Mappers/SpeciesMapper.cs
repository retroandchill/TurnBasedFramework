using LanguageExt;
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
}
