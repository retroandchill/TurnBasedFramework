using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Pokemon.Editor.Serializers.Json;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    PreferParameterlessConstructors = false
)]
public static partial class BattleTerrainMapper
{
    public static UBattleTerrain ToBattleTerrain(
        this BattleTerrainInfo growthRateInfo,
        UObject? outer = null
    )
    {
        return growthRateInfo.ToBattleTerrainInitializer(outer);
    }

    public static partial BattleTerrainInfo ToBattleTerrainInfo(this UBattleTerrain growthRate);

    private static partial BattleTerrainInitializer ToBattleTerrainInitializer(
        this BattleTerrainInfo growthRate,
        UObject? outer = null
    );
}
