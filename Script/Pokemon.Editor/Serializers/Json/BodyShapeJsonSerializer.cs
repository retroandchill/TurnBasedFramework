using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Microsoft.Extensions.Options;
using Pokemon.Data.Core;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealInject.Options;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class BodyShapeJsonSerializer(IConfigOptions<JsonSerializerOptions> jsonSerializerOptions) : IGameDataEntrySerializer<UBodyShape>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;
    
    public FName FormatTag => JsonConstants.FormatTag;
    public FText FormatName => JsonConstants.FormatName;
    public string FileExtensionText => JsonConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UBodyShape> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToBodyShapeInfo()), _jsonSerializerOptions);
    }

    public IEnumerable<UBodyShape> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<BodyShapeInfo[]>(source, _jsonSerializerOptions)!
            .Select(x => x.ToBodyShape(outer));
    }
}
