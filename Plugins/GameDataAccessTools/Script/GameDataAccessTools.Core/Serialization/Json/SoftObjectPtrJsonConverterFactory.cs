using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp;

namespace GameDataAccessTools.Core.Serialization.Json;

public class SoftObjectPtrJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsConstructedGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(TSoftObjectPtr<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(SoftObjectPtrJsonConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}