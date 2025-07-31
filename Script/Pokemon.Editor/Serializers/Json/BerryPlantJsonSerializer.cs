using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BerryPlantJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UBerryPlant>
{
    public override string SerializeData(IEnumerable<UBerryPlant> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBerryPlantInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UBerryPlant> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BerryPlantInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToBerryPlant(outer));
    }
}
