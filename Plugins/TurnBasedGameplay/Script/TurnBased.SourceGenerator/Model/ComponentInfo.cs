using System.Collections.Immutable;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace TurnBased.SourceGenerator.Model;

[UsedImplicitly]
public record ComponentInfo(
    ITypeSymbol ComponentType,
    string ComponentName,
    ImmutableArray<UPropertyInfo> Properties,
    ImmutableArray<UFunctionInfo> Methods,
    bool IsLast = false
);
