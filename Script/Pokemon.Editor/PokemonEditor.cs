using GameDataAccessTools.Core.Serialization;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Interop;
using Pokemon.Editor.Serializers.Json;
using Pokemon.Editor.Serializers.Pbs.Serializers;
using UnrealInject;
using UnrealSharp.Engine.Core.Modules;

namespace Pokemon.Editor;

[UsedImplicitly]
public class FPokemonEditor : IModuleInterface
{
    public void StartupModule()
    {
        var actions = PokemonManagedActions.Create();
        PokemonActionsExporter.CallSetActions(ref actions);
        
        FUnrealInjectModule.Instance.ConfigureServices(services => services
            .AddSingleton<IGameDataEntrySerializer<UAbility>, AbilityJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UBattleTerrain>, BattleTerrainJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UBattleWeather>, BattleWeatherJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UBerryPlant>, BerryPlantJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UBodyColor>, BodyColorJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UBodyShape>, BodyShapeJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UEggGroup>, EggGroupJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UEncounterType>, EncounterTypeJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UEvolutionMethod>, EvolutionMethodJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UEnvironment>, EnvironmentJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UFieldWeather>, FieldWeatherJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UGenderRatio>, GenderRatioJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UGrowthRate>, GrowthRateJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UHabitat>, HabitatJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UItem>, ItemJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UMove>, MoveJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UNature>, NatureJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<USpecies>, SpeciesJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UStat>, StatJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UStatusEffect>, StatusEffectJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UTargetType>, TargetTypeJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UType>, TypeJsonSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UType>, TypePbsSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UAbility>, AbilityPbsSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UMove>, MovePbsSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UItem>, ItemPbsSerializer>()
            .AddSingleton<IGameDataEntrySerializer<UBerryPlant>, BerryPlantPbsSerializer>());
    }

    public void ShutdownModule()
    {
        FUnrealInjectModule.Instance.ConfigureServices(services => services
            .RemoveAll<IGameDataEntrySerializer<UAbility>>()
            .RemoveAll<IGameDataEntrySerializer<UBattleTerrain>>()
            .RemoveAll<IGameDataEntrySerializer<UBattleWeather>>()
            .RemoveAll<IGameDataEntrySerializer<UBerryPlant>>()
            .RemoveAll<IGameDataEntrySerializer<UBodyColor>>()
            .RemoveAll<IGameDataEntrySerializer<UBodyShape>>()
            .RemoveAll<IGameDataEntrySerializer<UEggGroup>>()
            .RemoveAll<IGameDataEntrySerializer<UEncounterType>>()
            .RemoveAll<IGameDataEntrySerializer<UEvolutionMethod>>()
            .RemoveAll<IGameDataEntrySerializer<UEnvironment>>()
            .RemoveAll<IGameDataEntrySerializer<UFieldWeather>>()
            .RemoveAll<IGameDataEntrySerializer<UGenderRatio>>()
            .RemoveAll<IGameDataEntrySerializer<UGrowthRate>>()
            .RemoveAll<IGameDataEntrySerializer<UHabitat>>()
            .RemoveAll<IGameDataEntrySerializer<UItem>>()
            .RemoveAll<IGameDataEntrySerializer<UMove>>()
            .RemoveAll<IGameDataEntrySerializer<UNature>>()
            .RemoveAll<IGameDataEntrySerializer<USpecies>>()
            .RemoveAll<IGameDataEntrySerializer<UStat>>()
            .RemoveAll<IGameDataEntrySerializer<UStatusEffect>>()
            .RemoveAll<IGameDataEntrySerializer<UTargetType>>()
            .RemoveAll<IGameDataEntrySerializer<UType>>());
    }
}