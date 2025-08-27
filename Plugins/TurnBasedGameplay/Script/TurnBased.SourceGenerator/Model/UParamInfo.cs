using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace TurnBased.SourceGenerator.Model;

[UsedImplicitly]
public record UParamInfo(ITypeSymbol Type, string Name, string? DefaultValue = null, bool IsLast = false)
{
    [UsedImplicitly]
    public bool HasDefaultValue => DefaultValue is not null;
}