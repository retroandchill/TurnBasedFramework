using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class StatusEffectJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UStatusEffect>
{
    public override string SerializeData(IEnumerable<UStatusEffect> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToStatusEffectInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UStatusEffect> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<StatusEffectInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToStatusEffect(outer));
    }
}
