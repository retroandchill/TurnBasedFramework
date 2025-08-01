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

public sealed class EvolutionMethodJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UEvolutionMethod>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UEvolutionMethod> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToEvolutionMethodInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UEvolutionMethod> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<EvolutionMethodInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToEvolutionMethod(outer));
    }
}
