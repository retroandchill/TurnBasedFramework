using GameAccessTools.SourceGenerator.Attributes;
using Microsoft.CodeAnalysis;
using Retro.SourceGeneratorUtilities.Utilities.Attributes;

namespace GameAccessTools.SourceGenerator.Model;

[AttributeInfoType<ReferenceForAttribute>]
public record struct ReferenceForOverview(ITypeSymbol Type) {
  
  public bool IsReadOnly { get; init; }
  
}