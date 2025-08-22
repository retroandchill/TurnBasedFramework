using GameAccessTools.SourceGenerator.Attributes;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;

namespace Pokemon.Data;

[GameDataRepositoryProvider(SettingsDisplayName = "Game Data")]
public partial class GameData
{
    [SettingsCategory("Core")]
    public static partial UGrowthRateDataRepository GrowthRates { get; }

    [SettingsCategory("Core")]
    public static partial UGenderRatioDataRepository GenderRatios { get; }

    [SettingsCategory("Core")]
    public static partial UEggGroupDataRepository EggGroups { get; }

    [SettingsCategory("Core")]
    public static partial UBodyShapeDataRepository BodyShapes { get; }

    [SettingsCategory("Core")]
    public static partial UBodyColorDataRepository BodyColors { get; }

    [SettingsCategory("Core")]
    public static partial UHabitatDataRepository Habitats { get; }

    [SettingsCategory("Core")]
    public static partial UEvolutionMethodDataRepository EvolutionMethods { get; }

    [SettingsCategory("Core")]
    public static partial UStatDataRepository Stats { get; }

    [SettingsCategory("Core")]
    public static partial UNatureDataRepository Natures { get; }

    [SettingsCategory("Core")]
    public static partial UStatusEffectDataRepository StatusEffects { get; }

    [SettingsCategory("Core")]
    public static partial UFieldWeatherDataRepository FieldWeathers { get; }

    [SettingsCategory("Core")]
    public static partial UEncounterTypeDataRepository EncounterTypes { get; }

    [SettingsCategory("Core")]
    public static partial UEnvironmentDataRepository Environments { get; }

    [SettingsCategory("Core")]
    public static partial UBattleWeatherDataRepository BattleWeathers { get; }

    [SettingsCategory("Core")]
    public static partial UBattleTerrainDataRepository BattleTerrains { get; }

    [SettingsCategory("Core")]
    public static partial UTargetTypeDataRepository TargetTypes { get; }

    [SettingsCategory("Pbs")]
    public static partial UTypeDataRepository Types { get; }

    [SettingsCategory("Pbs")]
    public static partial UAbilityDataRepository Abilities { get; }

    [SettingsCategory("Pbs")]
    public static partial UMoveDataRepository Moves { get; }

    [SettingsCategory("Pbs")]
    public static partial UItemDataRepository Items { get; }

    [SettingsCategory("Pbs")]
    public static partial UBerryPlantDataRepository BerryPlants { get; }

    [SettingsCategory("Pbs")]
    public static partial USpeciesDataRepository Species { get; }
}
