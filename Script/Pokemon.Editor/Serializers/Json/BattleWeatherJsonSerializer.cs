using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Microsoft.Extensions.Options;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealInject.Options;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BattleWeatherJsonSerializer(
    IConfigOptions<JsonSerializerOptions> jsonSerializerOptions
) : IGameDataEntrySerializer<UBattleWeather>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public FName FormatTag => JsonConstants.FormatTag;
    public FText FormatName => JsonConstants.FormatName;
    public string FileExtensionText => JsonConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UBattleWeather> entries)
    {
        return JsonSerializer.Serialize(
            entries.Select(x => x.ToBattleWeatherInfo()),
            _jsonSerializerOptions
        );
    }

    public IEnumerable<UBattleWeather> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer
            .Deserialize<BattleWeatherInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToBattleWeather(outer));
    }
}
