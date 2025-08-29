using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealInject.Options;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public class TrainerTypeJsonSerializer(IConfigOptions<JsonSerializerOptions> jsonSerializerOptions) : IGameDataEntrySerializer<UTrainerType>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;
    
    public FName FormatTag => JsonConstants.FormatTag;
    public FText FormatName => JsonConstants.FormatName;
    public string FileExtensionText => JsonConstants.FileExtensionText;
    
    public string SerializeData(IEnumerable<UTrainerType> entries)
    {
        return JsonSerializer.Serialize(
            entries.Select(x => x.ToTrainerTypeInfo()),
            _jsonSerializerOptions
        );
    }

    public IEnumerable<UTrainerType> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer
            .Deserialize<TrainerTypeInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToTrainerType(outer));
    }
}