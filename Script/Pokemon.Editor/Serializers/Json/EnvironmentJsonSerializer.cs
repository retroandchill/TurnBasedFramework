using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class EnvironmentJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UEnvironment>
{
    public override string SerializeData(IEnumerable<UEnvironment> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToEnvironmentInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UEnvironment> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<EnvironmentInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToEnvironment(outer));
    }
}
