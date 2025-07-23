using GameAccessTools.SourceGenerator.Attributes;
using Microsoft.CodeAnalysis;
using Retro.SourceGeneratorUtilities.Utilities.Attributes;

namespace GameAccessTools.SourceGenerator.Model;

[AttributeInfoType<GameDataRepositoryProviderAttribute>]
public record struct GameDataRepositoryProviderInfo {

  public string? SettingsDisplayName { get; init; }
}
