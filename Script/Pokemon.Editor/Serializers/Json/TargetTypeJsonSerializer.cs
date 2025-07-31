using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class TargetTypeJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UTargetType>
{
    public override string SerializeData(IEnumerable<UTargetType> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToTargetTypeInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UTargetType> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<TargetTypeInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToTargetType(outer));
    }
}
