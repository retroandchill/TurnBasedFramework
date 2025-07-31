using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Pokemon.Editor.Serializers.Json;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class BattleWeatherMapper
{
    public static UBattleWeather ToBattleWeather(this BattleWeatherInfo growthRateInfo, UObject? outer = null)
    {
        return growthRateInfo.ToBattleWeatherInitializer(outer);
    }
    
    public static partial BattleWeatherInfo ToBattleWeatherInfo(this UBattleWeather growthRate);
    
    private static partial BattleWeatherInitializer ToBattleWeatherInitializer(this BattleWeatherInfo growthRate, UObject? outer = null);
}