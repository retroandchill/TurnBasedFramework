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

public sealed class NatureJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UNature>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UNature> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToNatureInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UNature> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<NatureInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToNature(outer));
    }
}
