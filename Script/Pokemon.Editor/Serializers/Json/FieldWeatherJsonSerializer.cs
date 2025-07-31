using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class FieldWeatherJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UFieldWeather>
{
    public override string SerializeData(IEnumerable<UFieldWeather> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToFieldWeatherInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UFieldWeather> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<FieldWeatherInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToFieldWeather(outer));
    }
}
