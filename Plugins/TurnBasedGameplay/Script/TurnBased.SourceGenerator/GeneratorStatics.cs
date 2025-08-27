using Microsoft.CodeAnalysis;

namespace TurnBased.SourceGenerator;

public static class GeneratorStatics
{
    public const string TurnBasedUnit = "UnrealSharp.TurnBasedCore.UTurnBasedUnit";
    public const string TurnBasedUnitComponent = "UnrealSharp.TurnBasedCore.UTurnBasedUnitComponent";
    
    public const string UPropertyAttribute = "UnrealSharp.Attributes.UPropertyAttribute";
    public const string UFunctionAttribute = "UnrealSharp.Attributes.UFunctionAttribute";
    public static bool IsTurnBaseUnitOrComponent(this ITypeSymbol symbol)
    {
        for (var baseType = symbol; baseType != null; baseType = baseType.BaseType)
        {
            if (baseType.ToDisplayString() is TurnBasedUnit or TurnBasedUnitComponent)
                return true;
        }

        return false;
    }
    
    public static bool IsTurnBasedUnit(this ITypeSymbol symbol)
    {
        for (var baseType = symbol; baseType != null; baseType = baseType.BaseType)
        {
            if (baseType.ToDisplayString() is TurnBasedUnit)
                return true;
        }

        return false;
    }

    public static bool IsTurnBasedUnitComponent(this ITypeSymbol symbol)
    {
        for (var baseType = symbol; baseType != null; baseType = baseType.BaseType)
        {
            if (baseType.ToDisplayString() is TurnBasedUnitComponent)
                return true;
        }
        
        return false;   
    }
}