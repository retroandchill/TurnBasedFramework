using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Microsoft.Extensions.Options;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using Retro.ReadOnlyParams.Annotations;
using UnrealInject.Options;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class SpeciesJsonSerializer(
    IConfigOptions<JsonSerializerOptions> jsonSerializerOptions
) : IGameDataEntrySerializer<USpecies>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public FName FormatTag => JsonConstants.FormatTag;
    public FText FormatName => JsonConstants.FormatName;
    public string FileExtensionText => JsonConstants.FileExtensionText;

    public string SerializeData(IEnumerable<USpecies> entries)
    {
        return JsonSerializer.Serialize(
            entries.Select(x => x.ToSpeciesInfo()),
            _jsonSerializerOptions
        );
    }

    public IEnumerable<USpecies> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer
            .Deserialize<SpeciesInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToSpecies(outer));
    }
}
