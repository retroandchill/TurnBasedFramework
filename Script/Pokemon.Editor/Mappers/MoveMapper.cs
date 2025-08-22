using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    PreferParameterlessConstructors = false
)]
public static partial class MoveMapper
{
    private const int AlwaysHitsAccuracy = 0;
    private const int NoDamagePower = 0;
    private const int VariableDamagePower = 1;
    private const int GuaranteedEffectChance = 0;

    public static UMove ToMove(this MoveInfo moveInfo, UObject? outer = null)
    {
        return moveInfo.ToMoveInitializer(outer);
    }

    [MapPropertyFromSource(nameof(MoveInfo.Power), Use = nameof(MapPower))]
    [MapPropertyFromSource(nameof(MoveInfo.Accuracy), Use = nameof(MapAccuracy))]
    [MapPropertyFromSource(nameof(MoveInfo.EffectChance), Use = nameof(MapEffectChance))]
    public static partial MoveInfo ToMoveInfo(this UMove move);

    private static int MapPower(this UMove move)
    {
        return move.DamageType switch
        {
            EDamageType.NoDamage => NoDamagePower,
            EDamageType.FixedPower => move.Power,
            EDamageType.VariablePower => VariableDamagePower,
            _ => throw new InvalidOperationException(),
        };
    }

    private static int MapAccuracy(this UMove move)
    {
        return move.AlwaysHits ? AlwaysHitsAccuracy : move.Accuracy;
    }

    private static int MapEffectChance(this UMove move)
    {
        return move.GuaranteedEffect ? GuaranteedEffectChance : move.EffectChance;
    }

    [MapPropertyFromSource(nameof(MoveInitializer.DamageType), Use = nameof(MapDamageType))]
    [MapPropertyFromSource(nameof(MoveInitializer.AlwaysHits), Use = nameof(MapAlwaysHits))]
    [MapPropertyFromSource(
        nameof(MoveInitializer.GuaranteedEffect),
        Use = nameof(MapGuaranteedEffect)
    )]
    private static partial MoveInitializer ToMoveInitializer(
        this MoveInfo move,
        UObject? outer = null
    );

    private static EDamageType MapDamageType(this MoveInfo moveInfo)
    {
        return moveInfo.Power switch
        {
            NoDamagePower => EDamageType.NoDamage,
            VariableDamagePower => EDamageType.VariablePower,
            _ => EDamageType.FixedPower,
        };
    }

    private static bool MapAlwaysHits(this MoveInfo moveInfo)
    {
        return moveInfo.Accuracy == AlwaysHitsAccuracy;
    }

    private static bool MapGuaranteedEffect(this MoveInfo moveInfo)
    {
        return moveInfo.EffectChance == GuaranteedEffectChance;
    }
}
