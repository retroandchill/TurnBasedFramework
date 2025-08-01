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

public sealed class BodyShapeJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UBodyShape>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UBodyShape> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBodyShapeInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UBodyShape> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BodyShapeInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToBodyShape(outer));
    }
}
