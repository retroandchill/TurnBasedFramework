using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp.GameplayTags;

namespace GameDataAccessTools.Core.Serialization.Json;

public class GameplayTagJsonConverter : JsonConverter<FGameplayTag>
{
    public override FGameplayTag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string value for FName");
        }
        
        return new FGameplayTag(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, FGameplayTag value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}