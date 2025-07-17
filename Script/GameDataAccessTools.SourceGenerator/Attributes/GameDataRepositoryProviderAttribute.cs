#if GAME_DATA_ACCESS_TOOLS_GENERATOR
using RhoMicro.CodeAnalysis;
#else
#nullable enable
using UnrealSharp.DeveloperSettings;
#endif

namespace GameAccessTools.SourceGenerator.Attributes;

#if GAME_DATA_ACCESS_TOOLS_GENERATOR
[IncludeFile]
public class GameDataRepositoryProviderAttribute<TSettings> : Attribute {
#else
public class GameDataRepositoryProviderAttribute<TSettings> : Attribute where TSettings : UDeveloperSettings {
#endif
  /// <summary>
  /// The name of the generated class, without the "U" prefix.
  /// Defaults to &lt;ClassName>DataAsset (or &lt;ClassName>Asset
  /// if the class name already ends with data).
  /// </summary>
  public string? GeneratedClassName { get; init; }

  public string? RepositoryConfigCategory { get; init; }
}
