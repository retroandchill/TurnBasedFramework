using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp;

namespace GameDataAccessTools.Core.Serialization.Json;

public class NameJsonConverter : JsonConverter<FName>
{
    public override FName Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string value for FName");
        }

        return reader.GetString()!;
    }

    public override void Write(Utf8JsonWriter writer, FName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
