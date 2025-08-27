using System.Collections.Immutable;
using JetBrains.Annotations;

namespace TurnBased.SourceGenerator.Model;

[UsedImplicitly]
public record ComponentInfo(string ComponentName, ImmutableArray<UPropertyInfo> Properties, ImmutableArray<UFunctionInfo> Methods);