using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BattleTerrainJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions)  : GameDataEntryJsonSerializerBase<UBattleTerrain>
{
    public override string SerializeData(IEnumerable<UBattleTerrain> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBattleTerrainInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UBattleTerrain> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BattleTerrainInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToBattleTerrain(outer));
    }
}