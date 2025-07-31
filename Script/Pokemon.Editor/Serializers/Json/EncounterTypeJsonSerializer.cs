using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class EncounterTypeJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UEncounterType>
{
    public override string SerializeData(IEnumerable<UEncounterType> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToEncounterTypeInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UEncounterType> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<EncounterTypeInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToEncounterType(outer));
    }
}
