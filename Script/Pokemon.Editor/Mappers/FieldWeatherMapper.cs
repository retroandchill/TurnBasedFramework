using LanguageExt;
using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class FieldWeatherMapper
{
    public static UFieldWeather ToFieldWeather(this FieldWeatherInfo fieldWeatherInfo, UObject? outer = null)
    {
        return fieldWeatherInfo.ToFieldWeatherInitializer(outer);
    }

    public static partial FieldWeatherInfo ToFieldWeatherInfo(this UFieldWeather fieldWeather);

    private static partial FieldWeatherInitializer ToFieldWeatherInitializer(this FieldWeatherInfo fieldWeather, UObject? outer = null);
    
    private static FGameplayTag? ToNullableGameplayTag(this Option<FGameplayTag> value) => value.ToNullable();

    private static Option<FGameplayTag> ToGameplayTagOption(this FGameplayTag? value) => value.ToOption();
}
