using GameAccessTools.SourceGenerator.Attributes;
using PokemonData.Data.Core;

namespace PokemonData;

[GameDataRepositoryProvider(SettingsDisplayName = "Game Data")]
public partial class GameData
{
    [SettingsCategory("Core")]
    public static partial UGrowthRateDataRepository GrowthRates { get; }
}