using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class StatJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UStat>
{
    public override string SerializeData(IEnumerable<UStat> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToStatInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UStat> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<StatInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToStat(outer));
    }
}
