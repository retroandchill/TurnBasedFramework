using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BodyShapeJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UBodyShape>
{
    public override string SerializeData(IEnumerable<UBodyShape> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBodyShapeInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UBodyShape> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BodyShapeInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToBodyShape(outer));
    }
}
