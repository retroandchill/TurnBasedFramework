using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BodyColorJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions)  : GameDataEntryJsonSerializerBase<UBodyColor>
{
    public override string SerializeData(IEnumerable<UBodyColor> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBodyColorInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UBodyColor> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BodyColorInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToBodyColor(outer));
    }
}