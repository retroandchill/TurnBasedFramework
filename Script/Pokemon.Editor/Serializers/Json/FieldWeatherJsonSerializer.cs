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

public sealed class FieldWeatherJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UFieldWeather>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UFieldWeather> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToFieldWeatherInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UFieldWeather> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<FieldWeatherInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToFieldWeather(outer));
    }
}
