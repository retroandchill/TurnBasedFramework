using System.Text.Json;
using System.Text.Json.Serialization;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
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
        
        var gameplayTagString = reader.GetString()!;
        #if !PACKAGE
        var tagSource = UObject.GetDefault<UGameDataAccessToolsSettings>().NewGameplayTagsPath;
        if (!UGameplayTagHandlingUtils.TryAddGameplayTagToIni(tagSource, gameplayTagString, out var error))
        {
            throw new JsonException(error);
        }
        #endif
        return new FGameplayTag(gameplayTagString);
    }

    public override void Write(Utf8JsonWriter writer, FGameplayTag value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}