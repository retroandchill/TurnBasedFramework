using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class EvolutionMethodJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UEvolutionMethod>
{
    public override string SerializeData(IEnumerable<UEvolutionMethod> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToEvolutionMethodInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UEvolutionMethod> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<EvolutionMethodInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToEvolutionMethod(outer));
    }
}
