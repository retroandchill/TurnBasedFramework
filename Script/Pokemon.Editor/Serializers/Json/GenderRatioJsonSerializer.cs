using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class GenderRatioJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UGenderRatio>
{
    public override string SerializeData(IEnumerable<UGenderRatio> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToGenderRatioInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UGenderRatio> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<GenderRatioInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToGenderRatio(outer));
    }
}
