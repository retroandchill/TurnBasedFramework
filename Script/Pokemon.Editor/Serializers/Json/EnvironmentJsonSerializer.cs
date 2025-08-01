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

public sealed class EnvironmentJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UEnvironment>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UEnvironment> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToEnvironmentInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UEnvironment> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<EnvironmentInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToEnvironment(outer));
    }
}
