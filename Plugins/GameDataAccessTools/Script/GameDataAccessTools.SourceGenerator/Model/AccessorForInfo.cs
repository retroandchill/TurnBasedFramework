using GameAccessTools.SourceGenerator.Attributes;
using Microsoft.CodeAnalysis;
using Retro.SourceGeneratorUtilities.Utilities.Attributes;

namespace GameAccessTools.SourceGenerator.Model;

[AttributeInfoType(typeof(AccessorForAttribute<>))]
public record struct AccessorForInfo(ITypeSymbol TargetType);