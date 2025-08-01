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

public sealed class BodyColorJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions)  : GameDataEntryJsonSerializerBase<UBodyColor>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UBodyColor> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBodyColorInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UBodyColor> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BodyColorInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToBodyColor(outer));
    }
}