using System.Text.Json;
using System.Text.Json.Nodes;
using GameDataAccessTools.Core.DataRetrieval;
using GameDataAccessTools.Core.Serialization.Marshallers;
using GameDataAccessTools.Core.Interop;
using GameDataAccessTools.Core.Serialization.Native;
using Microsoft.Extensions.Options;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;
using FJsonObjectConverterExporter = GameDataAccessTools.Core.Interop.FJsonObjectConverterExporter;

namespace GameDataAccessTools.Core.Serialization;

public abstract class GameDataEntryJsonSerializerBase<TEntry> : IGameDataEntrySerializer<TEntry>
    where TEntry : UObject, IGameDataEntry
{
    public FName FormatTag => "JSON";
    public FText FormatName => "JSON";
    public string FileExtensionText => "JSON file |*.json|";
    
    public abstract string SerializeData(IEnumerable<TEntry> entries);

    public abstract IEnumerable<TEntry> DeserializeData(string source, UObject outer);
}

public sealed class GameDataEntryJsonSerializer<TEntry>(IOptions<JsonSerializerOptions> jsonSerializerOptions) 
    : GameDataEntryJsonSerializerBase<TEntry> where TEntry : UObject, IGameDataEntry
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions.Value;

    public override string SerializeData(IEnumerable<TEntry> entries)
    {
        var jsonArray = new JsonArray();
        foreach (var entry in entries)
        {
            var nativeValue = new NativeJsonValue();
            using var nativeJsonReleaser = new NativeJsonValueReleaser(ref nativeValue);
            if (!FJsonObjectConverterExporter.CallSerializeObjectToJson(entry.NativeObject, ref nativeValue).ToManagedBool())
            {
                throw new JsonException("Unable to serialize entry to JSON.");
            }
            
            var node = JsonNodeMarshaller.FromNative(ref nativeValue);
            jsonArray.Add(node);
        }
        
        return JsonSerializer.Serialize(jsonArray, _jsonSerializerOptions);
    }

    public override IEnumerable<TEntry> DeserializeData(string source, UObject outer)
    {
        var jsonArray = JsonSerializer.Deserialize<JsonArray>(source, _jsonSerializerOptions)!;
        foreach (var entry in jsonArray)
        {
            if (entry is not JsonObject)
            {
                throw new JsonException($"Unable to deserialize entry '{entry}' as a JSON object.");
            }

            var nativeValue = new NativeJsonValue();
            JsonNodeMarshaller.ToNative(ref nativeValue, entry);
            var newEntry = UObject.NewObject<TEntry>(outer);

            var textData = new FTextData();
            using (new TextDataReleaser(ref textData)) 
            {
                FTextExporter.CallCreateEmptyText(ref textData);
                if (!FJsonObjectConverterExporter.CallDeserializeJsonToObject(ref nativeValue, newEntry.NativeObject,
                        ref textData).ToManagedBool())
                {
                    throw new JsonException(ConvertTextDataToString(ref textData));
                }
            }
            
            yield return newEntry;
        }
    }

    private static unsafe string ConvertTextDataToString(ref FTextData textData)
    {
        return new string(FTextExporter.CallToString(ref textData));
    }
}