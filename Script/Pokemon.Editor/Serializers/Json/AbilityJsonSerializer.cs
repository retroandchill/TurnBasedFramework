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

public sealed class AbilityJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UAbility>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;
    
    public override string SerializeData(IEnumerable<UAbility> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToAbilityInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UAbility> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<AbilityInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToAbility(outer));
    }
}
