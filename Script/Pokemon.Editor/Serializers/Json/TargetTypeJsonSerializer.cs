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

public sealed class TargetTypeJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UTargetType>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UTargetType> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToTargetTypeInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UTargetType> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<TargetTypeInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToTargetType(outer));
    }
}
