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

public sealed class EggGroupJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UEggGroup>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UEggGroup> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToEggGroupInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UEggGroup> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<EggGroupInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToEggGroup(outer));
    }
}
