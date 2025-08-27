using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TurnBased.SourceGenerator.Model;
using TurnBased.SourceGenerator.Properties;

namespace TurnBased.SourceGenerator.Generators;

[Generator]
public class TurnBasedUnitExtensionGenerator : IIncrementalGenerator
{
    private readonly Dictionary<ITypeSymbol, ComponentInfo> _components = new(SymbolEqualityComparer.Default);
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        _components.Clear();
        var classesProvider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (s, _) => s is ClassDeclarationSyntax,
            transform: (ctx, _) =>
            {
                var classNode = (ClassDeclarationSyntax)ctx.Node;
                if (ctx.SemanticModel.GetDeclaredSymbol(classNode) is not INamedTypeSymbol classSymbol)
                {
                    return null;
                }
                
                return classSymbol.IsTurnBaseUnitOrComponent() ? classSymbol : null;
            })
            .Where(m => m is not null);
        
     context.RegisterSourceOutput(classesProvider, Execute!);   
    }
    
    private void Execute(SourceProductionContext context, INamedTypeSymbol classSymbol)
    {
        if (classSymbol.IsTurnBasedUnit())
        {
            CreateTurnBasedUnitExtension(context, classSymbol);
        }
        else if (classSymbol.IsTurnBasedUnitComponent())
        {
            CreateTurnBasedUnitComponentExtension(context, classSymbol);
        }
        else
        {
            throw new InvalidOperationException("Invalid class symbol.");
        }
    } 
    
    private void CreateTurnBasedUnitExtension(SourceProductionContext context, INamedTypeSymbol classSymbol)
    {
        var templateParams = new
        {
            Namespace = classSymbol.ContainingNamespace.ToDisplayString(),
            ClassName = classSymbol.Name,
            EngineName = classSymbol.Name[1..],
            Components = classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.Type.IsTurnBasedUnitComponent())
                .Select(GetComponentInfo)
                .ToImmutableArray()
        };
        
        var handlebars = Handlebars.Create();
        handlebars.Configuration.TextEncoder = null;

        context.AddSource(
            $"{templateParams.EngineName}Extensions.g.cs",
            handlebars.Compile(SourceTemplates.TurnBasedUnitExtensionsTemplate)(templateParams)
        );
    }

    private void CreateTurnBasedUnitComponentExtension(SourceProductionContext context,
                                                              INamedTypeSymbol classSymbol)
    {
        
    }

    private ComponentInfo GetComponentInfo(IPropertySymbol propertySymbol)
    {
        if (_components.TryGetValue(propertySymbol.Type, out var componentInfo))
        {
            return componentInfo;
        }

        var info = new ComponentInfo(propertySymbol.Name,
            [
                ..propertySymbol.Type.GetMembers()
                    .OfType<IPropertySymbol>()
                    .Where(IsAccessibleProperty)
                    .Select(GetPropertyInfo)
            ],
            [
                ..propertySymbol.Type.GetMembers()
                    .OfType<IMethodSymbol>()
                    .Where(IsAccessibleMethod)
                    .Select(GetFunctionInfo)
            ]
        );
        
        _components.Add(propertySymbol.Type, info);
        return info;
    }

    private static bool IsAccessibleProperty(IPropertySymbol propertySymbol)
    {
        if (propertySymbol.IsStatic || propertySymbol.DeclaredAccessibility != Accessibility.Public) return false;

        if (propertySymbol.GetAttributes()
            .Any(a => a.AttributeClass?.ToDisplayString() == GeneratorStatics.UPropertyAttribute))
        {
            return true;
        }
        
        return GetAccessors(propertySymbol)
            .Any(m => m.DeclaredAccessibility == Accessibility.Public && m.GetAttributes()
                .Any(a => a.AttributeClass?.ToDisplayString() == GeneratorStatics.UFunctionAttribute));
    }

    private static IEnumerable<IMethodSymbol> GetAccessors(IPropertySymbol propertySymbol)
    {
        if (propertySymbol.GetMethod is not null) yield return propertySymbol.GetMethod;
        if (propertySymbol.SetMethod is not null) yield return propertySymbol.SetMethod;
    }

    private static UPropertyInfo GetPropertyInfo(IPropertySymbol propertySymbol)
    {
        var upropertyAttribute = propertySymbol.GetAttributes()
            .SingleOrDefault(a => a.AttributeClass?.ToDisplayString() == GeneratorStatics.UPropertyAttribute);

        string? displayName = null;
        string? category = null;
        if (upropertyAttribute is not null)
        {
            var namedArguments = upropertyAttribute.NamedArguments
                .ToDictionary(a => a.Key, a => a.Value.Value);
            
            displayName = namedArguments.TryGetValue("DisplayName", out var displayNameValue) ? displayNameValue as string : null;
            category = namedArguments.TryGetValue("Category", out var categoryValue) ? categoryValue as string : null;
        }
        
        return new UPropertyInfo(propertySymbol.Type, propertySymbol.Name, GetAccessorInfo(propertySymbol.GetMethod),
            GetAccessorInfo(propertySymbol.SetMethod), displayName, category);
    }

    private static AccessorInfo? GetAccessorInfo(IMethodSymbol? methodSymbol)
    {
        if (methodSymbol is not { DeclaredAccessibility: Accessibility.Public } ) return null;
        
        var isUFunction = methodSymbol.GetAttributes()
            .Any(a => a.AttributeClass?.ToDisplayString() == GeneratorStatics.UFunctionAttribute);
        return new AccessorInfo(isUFunction, GetMethodAttributes(methodSymbol));
    }

    private static ImmutableArray<AttributeInfo> GetMethodAttributes(IMethodSymbol methodSymbol)
    {
        return [
            ..methodSymbol.GetAttributes()
                .Select(a => a.ApplicationSyntaxReference?.GetSyntax())
                .OfType<AttributeSyntax>()
                .Select(a => new AttributeInfo(a))
        ];
    }

    private static bool IsAccessibleMethod(IMethodSymbol methodSymbol)
    {
        return methodSymbol is { DeclaredAccessibility: Accessibility.Public, AssociatedSymbol: not IPropertySymbol } 
               && methodSymbol.GetAttributes()
                   .Any(a => a.AttributeClass?.ToDisplayString() == GeneratorStatics.UFunctionAttribute);
    }

    private static UFunctionInfo GetFunctionInfo(IMethodSymbol methodSymbol)
    {
        return new UFunctionInfo(methodSymbol.Name, methodSymbol.ReturnType,
            GetMethodAttributes(methodSymbol), [
                ..methodSymbol.Parameters
                    .Select((p, i) =>
                        new UParamInfo(p.Type, p.Name, GetDefaultValue(p), i == methodSymbol.Parameters.Length - 1))
            ]);
    }
    
    private static string? GetDefaultValue(IParameterSymbol parameterSymbol)
    {
        return parameterSymbol.DeclaringSyntaxReferences
            .Select(s => s.GetSyntax())
            .OfType<ParameterSyntax>()
            .Select(pax => pax.Default?.Value.ToString())
            .FirstOrDefault();
    }
}