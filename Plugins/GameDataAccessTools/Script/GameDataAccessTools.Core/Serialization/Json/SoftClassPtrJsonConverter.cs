using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;

namespace GameDataAccessTools.Core.Serialization.Json;

public class SoftClassPtrJsonConverter<T> : JsonConverter<TSoftClassPtr<T>> where T : UObject
{
    public override TSoftClassPtr<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return new TSoftClassPtr<T>();
            case JsonTokenType.String:
            {
                var pathString = reader.GetString()!;
                return pathString.GetSoftClassPtrFromPath().Cast<T>();
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

    public override void Write(Utf8JsonWriter writer, TSoftClassPtr<T> value, JsonSerializerOptions options)
    {
        if (value.IsNull)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}