using System.Text.Json;
using System.Text.Json.Serialization;
using GameDataAccessTools.Core.Serialization.Json;
using UnrealInject.Options;

namespace GameDataAccessTools.Core.Serialization;

public class BaseJsonConfigurer : IOptionsConfiguration<JsonSerializerOptions>
{
    public void Configure(JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.PropertyNameCaseInsensitive = true;
        options.AllowTrailingCommas = true;
        options.WriteIndented = true;

        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new NameJsonConverter());
        options.Converters.Add(new TextJsonConverter());
        options.Converters.Add(new GameplayTagJsonConverter());
        options.Converters.Add(new SubclassOfJsonConverterFactory());
        options.Converters.Add(new SoftObjectPtrJsonConverterFactory());
        options.Converters.Add(new SoftClassPtrJsonConverterFactory());
    }
}