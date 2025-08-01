﻿using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Microsoft.Extensions.Options;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class GrowthRateJsonSerializer(IOptions<JsonSerializerOptions> jsonSerializerOptions)  : GameDataEntryJsonSerializerBase<UGrowthRate>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;
    
    public override string SerializeData(IEnumerable<UGrowthRate> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToGrowthRateInfo()), _jsonSerializerOptions);
    }

    public override IEnumerable<UGrowthRate> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<GrowthRateInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToGrowthRate(outer));
    }
}