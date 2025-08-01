using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Microsoft.Extensions.Options;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class StatJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UStat>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UStat> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToStatInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UStat> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<StatInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToStat(outer));
    }
}
