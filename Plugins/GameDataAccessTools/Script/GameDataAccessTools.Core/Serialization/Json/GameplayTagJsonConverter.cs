using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp.GameplayTags;
using static GameDataAccessTools.Core.Serialization.SerializationExtensions;

namespace GameDataAccessTools.Core.Serialization.Json;

public class GameplayTagJsonConverter : JsonConverter<FGameplayTag>
{
    public override FGameplayTag Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string value for FGameplayTag");
        }

        var gameplayTagString = reader.GetString()!;
        try
        {
            return GetOrCreateGameplayTag(gameplayTagString);
        }
        catch (InvalidOperationException e)
        {
            throw new JsonException($"Failed to parse gameplay tag {gameplayTagString}", e);
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        FGameplayTag value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value.ToString());
    }
}
