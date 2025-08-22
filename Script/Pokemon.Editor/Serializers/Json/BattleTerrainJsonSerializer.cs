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

public sealed class BattleTerrainJsonSerializer(
    IConfigOptions<JsonSerializerOptions> jsonSerializerOptions
) : IGameDataEntrySerializer<UBattleTerrain>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public FName FormatTag => JsonConstants.FormatTag;
    public FText FormatName => JsonConstants.FormatName;
    public string FileExtensionText => JsonConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UBattleTerrain> entries)
    {
        return JsonSerializer.Serialize(
            entries.Select(x => x.ToBattleTerrainInfo()),
            _jsonSerializerOptions
        );
    }

    public IEnumerable<UBattleTerrain> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer
            .Deserialize<BattleTerrainInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToBattleTerrain(outer));
    }
}
