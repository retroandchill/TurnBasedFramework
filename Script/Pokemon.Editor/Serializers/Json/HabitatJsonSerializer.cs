using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class HabitatJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UHabitat>
{
    public override string SerializeData(IEnumerable<UHabitat> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToHabitatInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UHabitat> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<HabitatInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToHabitat(outer));
    }
}
