using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class GrowthRateJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions)  : GameDataEntryJsonSerializerBase<UGrowthRate>
{
    public override string SerializeData(IEnumerable<UGrowthRate> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToGrowthRateInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UGrowthRate> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<GrowthRateInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToGrowthRate(outer));
    }
}