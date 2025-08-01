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

public sealed class TypeJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UType>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UType> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToTypeInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UType> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<TypeInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToType(outer));
    }
}
