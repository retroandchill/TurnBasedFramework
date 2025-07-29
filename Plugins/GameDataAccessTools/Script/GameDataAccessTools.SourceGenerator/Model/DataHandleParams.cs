using GameAccessTools.SourceGenerator.Utilities;
using Microsoft.CodeAnalysis;

namespace GameAccessTools.SourceGenerator.Model;

public record DataHandleParams(INamedTypeSymbol HandleSymbol, ITypeSymbol PropertyType)
{
    public string Namespace => HandleSymbol.ContainingNamespace.ToDisplayString();
    
    public string ClassName => HandleSymbol.Name;
    
    public bool IsUStruct => HandleSymbol.GetAttributes()
        .Any(a => a.AttributeClass?.ToDisplayString() == SourceContextNames.UStructAttribute);
    
    public bool IsRecord => HandleSymbol.IsRecord;

    public string EngineName => GetEngineName(ClassName);

    public string PropertyTypeEngineName => GetEngineName(PropertyType.Name);

    private readonly string? _pluralName;
    public string PluralName
    {
        get => _pluralName ?? $"{EngineName}s";
        init => _pluralName = value;
    }

    private static string GetEngineName(string name)
    {
        return name[0] is 'U' or 'A' or 'F' or 'T' or 'I' && name.Length > 1 &&
               name[1] is >= 'A' and <= 'Z'
            ? name[1..]
            : name;
    }
}