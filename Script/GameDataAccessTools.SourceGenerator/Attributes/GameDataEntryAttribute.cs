#if GAME_DATA_ACCESS_TOOLS_GENERATOR
using RhoMicro.CodeAnalysis;
#else
#nullable enable
#endif

namespace GameAccessTools.SourceGenerator.Attributes;

#if GAME_DATA_ACCESS_TOOLS_GENERATOR
[IncludeFile]
#endif
internal class GameDataEntryAttribute : Attribute {
  /// <summary>
  /// The name of the generated class, without the "U" prefix.
  /// Defaults to &lt;ClassName>DataAsset (or &lt;ClassName>Asset
  /// if the class name already ends with data).
  /// </summary>
  public string? GeneratedClassName { get; init; }

  public string? AssetName { get; init; }
}
