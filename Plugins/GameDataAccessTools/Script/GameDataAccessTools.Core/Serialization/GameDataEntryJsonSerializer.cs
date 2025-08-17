using System.Text.Json;
using System.Text.Json.Nodes;
using GameDataAccessTools.Core.DataRetrieval;
using GameDataAccessTools.Core.Serialization.Marshallers;
using GameDataAccessTools.Core.Interop;
using GameDataAccessTools.Core.Serialization.Native;
using Microsoft.Extensions.Options;
using Retro.ReadOnlyParams.Annotations;
using UnrealInject.Options;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;
using FJsonObjectConverterExporter = GameDataAccessTools.Core.Interop.FJsonObjectConverterExporter;

namespace GameDataAccessTools.Core.Serialization;

public static class JsonConstants
{
    public static readonly FName FormatTag = "JSON";
    public static readonly FText FormatName = "JSON";
    public const string FileExtensionText = "JSON file |*.json|";
}

public sealed class GameDataEntryJsonSerializer<TEntry>(IConfigOptions<JsonSerializerOptions> jsonSerializerOptions) 
    : IGameDataEntrySerializer<TEntry> where TEntry : UObject, IGameDataEntry
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public FName FormatTag => JsonConstants.FormatTag;
    public FText FormatName => JsonConstants.FormatName;
    public string FileExtensionText => JsonConstants.FileExtensionText;
    
    public string SerializeData(IEnumerable<TEntry> entries)
    {
        var jsonArray = new JsonArray();
        foreach (var entry in entries)
        {
            jsonArray.Add(entry.SerializeObjectToJson());
        }
        
        return JsonSerializer.Serialize(jsonArray, _jsonSerializerOptions);
    }

    public IEnumerable<TEntry> DeserializeData(string source, UObject outer)
    {
        var jsonArray = JsonSerializer.Deserialize<JsonArray>(source, _jsonSerializerOptions)!;
        foreach (var entry in jsonArray)
        {
            if (entry is not JsonObject jsonObject)
            {
                throw new JsonException($"Unable to deserialize entry '{entry}' as a JSON object.");
            }

            yield return jsonObject.DeserializeObjectFromJson<TEntry>(outer);
        }
    }
}