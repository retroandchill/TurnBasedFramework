using GameAccessTools.SourceGenerator.Utilities;
using Microsoft.CodeAnalysis;

namespace GameAccessTools.SourceGenerator.Model;

public record DataHandleParams(string ClassName, string PropertyType)
{
    public required string Namespace { get; init; }
    
    public required bool IsUStruct { get; init; }
    
    public required bool IsRecord { get; init; }
    
    public required bool IsComparable { get; init; }

    public string EngineName => GetEngineName(ClassName);

    public string PropertyTypeEngineName => GetEngineName(PropertyType);

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