using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp;

namespace GameDataAccessTools.Core.Serialization.Json;

public class SubclassOfJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsConstructedGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(TSubclassOf<>);
    }

    public override JsonConverter? CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var converterType = typeof(SubclassOfJsonConverter<>).MakeGenericType(
            typeToConvert.GenericTypeArguments[0]
        );
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}
