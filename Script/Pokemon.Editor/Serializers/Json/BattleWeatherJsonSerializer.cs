using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BattleWeatherJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions)  : GameDataEntryJsonSerializerBase<UBattleWeather>
{
    public override string SerializeData(IEnumerable<UBattleWeather> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBattleWeatherInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UBattleWeather> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BattleWeatherInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToBattleWeather(outer));
    }
}