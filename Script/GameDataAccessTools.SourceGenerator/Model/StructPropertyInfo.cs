using Microsoft.CodeAnalysis;

namespace GameAccessTools.SourceGenerator.Model;

public readonly record struct MarshallerInfo(string Name, string? ChildMarshallerType = null) {
  public bool IsInstanced => ChildMarshallerType is not null;
}

public record StructPropertyInfo(ITypeSymbol Type, string Name) {
  public required MarshallerInfo MarshallerInfo { get; init; }
  
  public bool MarshallerInstanced => MarshallerInfo.IsInstanced;
}