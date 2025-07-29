using GameAccessTools.SourceGenerator.Attributes;
using Retro.SourceGeneratorUtilities.Utilities.Attributes;

namespace GameAccessTools.SourceGenerator.Model;

[AttributeInfoType<DataHandleAttribute>]
public record struct DataHandleInfo(string? PluralName);