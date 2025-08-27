using System.Collections.Immutable;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace TurnBased.SourceGenerator.Model;

public readonly record struct AccessorInfo(bool IsUFunction, ImmutableArray<AttributeInfo> Attributes);

[UsedImplicitly]
public record UPropertyInfo(ITypeSymbol Type, string Name, AccessorInfo? Getter, AccessorInfo? Setter, string? DisplayName, string? Category)
{
    [UsedImplicitly]
    public bool HasGetter => Getter.HasValue;
    
    [UsedImplicitly]
    public bool HasSetter => Setter.HasValue;
    
    [UsedImplicitly]
    public bool GetterIsUFunction => Getter?.IsUFunction ?? false;
    
    [UsedImplicitly]
    public ImmutableArray<AttributeInfo> GetterAttributes => Getter?.Attributes ?? [];
    
    [UsedImplicitly]
    public bool SetterIsUFunction => Setter?.IsUFunction ?? false;
    
    [UsedImplicitly]
    public ImmutableArray<AttributeInfo> SetterAttributes => Setter?.Attributes ?? [];
    
    [UsedImplicitly]
    public bool HasDisplayName = DisplayName is not null;
    
    [UsedImplicitly]
    public bool HasCategory = Category is not null;
}