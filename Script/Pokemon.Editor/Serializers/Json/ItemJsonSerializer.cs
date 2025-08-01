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

public sealed class ItemJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UItem>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<UItem> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToItemInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UItem> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<ItemInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToItem(outer));
    }
}
