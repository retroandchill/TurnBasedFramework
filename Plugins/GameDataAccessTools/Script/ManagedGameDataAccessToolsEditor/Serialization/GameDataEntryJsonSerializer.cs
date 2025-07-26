using System.Text.Json;
using System.Text.Json.Nodes;
using ManagedGameDataAccessToolsEditor.Serialization;
using ManagedGameDataAccessToolsEditor.Serialization.Marshallers;
using ManagedGameDataAccessToolsEditor.Serialization.Native;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.Interop;

namespace ManagedGameDataAccessToolsEditor.Interop;

public class GameDataEntryJsonSerializer<TEntry>([ReadOnly] JsonSerializerOptions jsonSerializerOptions) 
    : IGameDataEntrySerializer<TEntry> where TEntry : UGameDataEntry
{
    public FText FormatName => "JSON";
    public string FileExtensionText => "JSON file (*.json)";

    public string SerializeData(IEnumerable<TEntry> entries)
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
        
        return JsonSerializer.Serialize(jsonArray, jsonSerializerOptions);
    }

    public IEnumerable<TEntry> DeserializeData(string source, UObject outer)
    {
        var jsonArray = JsonSerializer.Deserialize<JsonObject>(source, jsonSerializerOptions)!;
        foreach (var (key, value) in jsonArray)
        {
            if (value is not JsonObject)
            {
                throw new JsonException($"Unable to deserialize entry {key} (value = '{value}') as a JSON object.");
            }

            var nativeValue = new NativeJsonValue();
            JsonNodeMarshaller.ToNative(ref nativeValue, value);
            var newEntry = UObject.NewObject<TEntry>(outer);

            var textData = new FTextData();
            using (new TextDataReleaser(ref textData)) 
            {
                FTextExporter.CallCreateEmptyText(ref textData);
                if (!FJsonObjectConverterExporter.CallDeserializeJsonToObject(ref nativeValue, newEntry.NativeObject,
                        ref textData).ToManagedBool())
                {
                    unsafe
                    {
                        throw new JsonException(ConvertTextDataToString(ref textData));
                    }
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