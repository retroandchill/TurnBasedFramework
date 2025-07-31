using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class EggGroupJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UEggGroup>
{
    public override string SerializeData(IEnumerable<UEggGroup> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToEggGroupInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UEggGroup> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<EggGroupInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToEggGroup(outer));
    }
}
