using GameAccessTools.SourceGenerator.Attributes;
using Retro.SourceGeneratorUtilities.Utilities.Attributes;

namespace GameAccessTools.SourceGenerator.Model;

[AttributeInfoType<GameDataEntryAttribute>]
public record struct GameDataEntryInfo
{
    public string? GeneratedClassName { get; init; }
}