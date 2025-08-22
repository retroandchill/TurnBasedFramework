using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using static UnrealSharp.GameDataAccessTools.UTextSerializationBlueprintLibrary;

namespace GameDataAccessTools.Core.Serialization.Json;

public class SubclassOfJsonConverter<T> : JsonConverter<TSubclassOf<T>>
    where T : UObject
{
    public override TSubclassOf<T> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return new TSubclassOf<T>();
            case JsonTokenType.String:
            {
                var pathString = reader.GetString()!;
                var path = GetClassFromPath(pathString);
                if (!path.Valid)
                {
                    throw new JsonException($"Invalid path {pathString}");
                }

                return path.As<T>();
            }
            case JsonTokenType.None:
            case JsonTokenType.StartObject:
            case JsonTokenType.EndObject:
            case JsonTokenType.StartArray:
            case JsonTokenType.EndArray:
            case JsonTokenType.PropertyName:
            case JsonTokenType.Comment:
            case JsonTokenType.Number:
            case JsonTokenType.True:
            case JsonTokenType.False:
            default:
                throw new JsonException("Expected string value for TSubclassOf<T>");
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        TSubclassOf<T> value,
        JsonSerializerOptions options
    )
    {
        if (value.Valid)
        {
            writer.WriteStringValue(value.ToString());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
