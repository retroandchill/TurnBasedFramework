using System.Collections;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using GameAccessTools.SourceGenerator.Attributes;
using GameAccessTools.SourceGenerator.Model;
using GameAccessTools.SourceGenerator.Properties;
using GameAccessTools.SourceGenerator.Utilities;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameAccessTools.SourceGenerator.Generators;

[Generator]
public class ObjectPropertyAccessorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (s, _) => s is ClassDeclarationSyntax or StructDeclarationSyntax or RecordDeclarationSyntax,
                transform: (ctx, _) =>
                {
                    var classNode = (TypeDeclarationSyntax)ctx.Node;
                    if (ctx.SemanticModel.GetDeclaredSymbol(classNode) is not INamedTypeSymbol symbol)
                    {
                        return null;
                    }

                    var hasReferenceForAttribute = symbol.GetAttributes()
                        .Any(attr => attr.AttributeClass?.MetadataName == typeof(AccessorForAttribute<>).Name);

                    return hasReferenceForAttribute ? symbol : null;
                })
            .Where(m => m is not null)
            .Collect();

        context.RegisterSourceOutput(syntaxProvider, (spc, source) =>
        {
            foreach (var classSymbol in source)
            {
                GeneratePropertyAccessorType(classSymbol!, spc);
            }
        });
    }

    private static void GeneratePropertyAccessorType(INamedTypeSymbol classSymbol, SourceProductionContext context)
    {
        var referenceForAttribute = classSymbol.GetAttributes()
            .GetAccessorForInfos()
            .Single();
        
        List<ITypeSymbol> allTypes = [referenceForAttribute.TargetType];
        var baseType = referenceForAttribute.TargetType.BaseType;
        while (baseType is not null)
        {
            allTypes.Add(baseType);
            if (baseType.ToDisplayString() == SourceContextNames.UGameDataEntry)
            {
                break;
            }
            
            baseType = baseType.BaseType;
        }
        
        var properties = ((IEnumerable<ITypeSymbol>) allTypes).Reverse()
            .SelectMany(x => x.GetMembers())
            .OfType<IPropertySymbol>()
            .Where(x => !x.IsStatic)
            .Where(x => x.ContainingType?.ToDisplayString() == SourceContextNames.UGameDataEntry ||
                        x.GetAttributes()
                            .Any(y =>
                                y.AttributeClass?.ToDisplayString() == SourceContextNames.UPropertyAttribute))
            .Select(x => x.GetPropertyInfo())
            .ToImmutableArray();

        var templateParams = new
        {
            Namespace = classSymbol.ContainingNamespace.ToDisplayString(),
            ClassName = classSymbol.Name,
            ObjectType = GetClassType(classSymbol),
            ReferenceClassName = referenceForAttribute.TargetType.ToDisplayString(),
            ReferenceClassNamespace = referenceForAttribute.TargetType.ContainingNamespace.ToDisplayString(),
            ReferenceClassEngineName = referenceForAttribute.TargetType.Name[1..],
            Properties = properties
        };

        var handlebars = Handlebars.Create();
        handlebars.Configuration.TextEncoder = null;

        context.AddSource($"{classSymbol.Name}.g.cs",
            handlebars.Compile(SourceTemplates.GameDataEntryAccessorTemplate)(templateParams));
    }

    private static string GetClassType(INamedTypeSymbol classSymbol)
    {
        if (classSymbol.TypeKind == TypeKind.Struct)
        {
            return classSymbol.IsRecord ? "record struct" : "struct";
        }

        return classSymbol.IsRecord ? "record" : "class";
    }
}