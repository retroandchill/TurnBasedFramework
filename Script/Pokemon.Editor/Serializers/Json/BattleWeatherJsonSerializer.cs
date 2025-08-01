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

public sealed class BattleWeatherJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions)  : GameDataEntryJsonSerializerBase<UBattleWeather>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;
    
    public override string SerializeData(IEnumerable<UBattleWeather> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBattleWeatherInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UBattleWeather> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BattleWeatherInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToBattleWeather(outer));
    }
}