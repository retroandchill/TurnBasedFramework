using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class SpeciesJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<USpecies>
{
    public override string SerializeData(IEnumerable<USpecies> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToSpeciesInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<USpecies> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<SpeciesInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToSpecies(outer));
    }
}
