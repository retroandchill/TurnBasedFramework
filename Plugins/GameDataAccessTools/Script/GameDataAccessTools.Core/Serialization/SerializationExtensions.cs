using System.Text.Json;
using System.Text.Json.Nodes;
using GameDataAccessTools.Core.Interop;
using GameDataAccessTools.Core.Serialization.Marshallers;
using GameDataAccessTools.Core.Serialization.Native;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace GameDataAccessTools.Core.Serialization;

public static class SerializationExtensions
{
    public static IServiceCollection ConfigureJsonSerialization(this IServiceCollection services,
                                                                Action<JsonSerializerOptions> configure)
    {
        services.TryAddSingleton(typeof(IOptions<>), typeof(OptionsManager<>));
        services.TryAddSingleton(_ => new JsonSerializerOptions());
        services.Configure(configure);
        return services;
    }

    public static JsonObject SerializeObjectToJson(this UObject obj)
    {
        var nativeValue = new NativeJsonValue();
        using var nativeJsonReleaser = new NativeJsonValueReleaser(ref nativeValue);
        if (!FJsonObjectConverterExporter.CallSerializeObjectToJson(obj.NativeObject, ref nativeValue).ToManagedBool())
        {
            throw new JsonException("Unable to serialize entry to JSON.");
        }
            
        var result = JsonNodeMarshaller.FromNative(ref nativeValue);

        if (result is not JsonObject jsonObject)
        {
            throw new JsonException($"Unable to serialize entry to JSON. Expected JsonObject, got {result?.GetType().Name ?? "???"}.");
        }
        
        return jsonObject;
    }

    public static T DeserializeObjectFromJson<T>(this JsonObject json, UObject? outer = null, TSubclassOf<T> classType = default) where T : UObject
    {
        var nativeValue = new NativeJsonValue();
        JsonNodeMarshaller.ToNative(ref nativeValue, json);
        var newEntry = UObject.NewObject(outer, classType);

        var textData = new FTextData();
        using var textDataReleaser = new TextDataReleaser(ref textData);
        FTextExporter.CallCreateEmptyText(ref textData);
        if (!FJsonObjectConverterExporter.CallDeserializeJsonToObject(ref nativeValue, newEntry.NativeObject,
                ref textData).ToManagedBool())
        {
            throw new JsonException(ConvertTextDataToString(ref textData));
        }

        return newEntry;
    }
    
    private static unsafe string ConvertTextDataToString(ref FTextData textData)
    {
        return new string(FTextExporter.CallToString(ref textData));
    }
}