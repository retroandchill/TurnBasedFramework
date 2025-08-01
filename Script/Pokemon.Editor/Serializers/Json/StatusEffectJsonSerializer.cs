using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Microsoft.Extensions.Options;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class StatusEffectJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UStatusEffect>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UStatusEffect> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToStatusEffectInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UStatusEffect> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<StatusEffectInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToStatusEffect(outer));
    }
}
