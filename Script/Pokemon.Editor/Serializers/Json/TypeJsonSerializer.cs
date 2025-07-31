using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class TypeJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UType>
{
    public override string SerializeData(IEnumerable<UType> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToTypeInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UType> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<TypeInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToType(outer));
    }
}
