using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Microsoft.Extensions.Options;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BerryPlantJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UBerryPlant>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UBerryPlant> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBerryPlantInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UBerryPlant> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BerryPlantInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToBerryPlant(outer));
    }
}
