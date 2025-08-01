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

public sealed class GenderRatioJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UGenderRatio>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UGenderRatio> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToGenderRatioInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UGenderRatio> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<GenderRatioInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToGenderRatio(outer));
    }
}
