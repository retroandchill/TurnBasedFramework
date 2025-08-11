using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp;
using UnrealSharp.GameDataAccessTools;
using static UnrealSharp.GameDataAccessTools.UTextSerializationBlueprintLibrary;

namespace GameDataAccessTools.Core.Serialization.Json;

public class TextJsonConverter : JsonConverter<FText>
{
    public override FText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string value for FName");
        }
        
        return FromLocalizedString(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, FText value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(ToLocalizedString(value));
    }
}