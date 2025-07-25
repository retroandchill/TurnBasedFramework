﻿using System.Collections.Immutable;
using GameAccessTools.SourceGenerator.Attributes;
using GameAccessTools.SourceGenerator.Model;
using GameAccessTools.SourceGenerator.Properties;
using GameAccessTools.SourceGenerator.Utilities;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Retro.SourceGeneratorUtilities.Utilities.Attributes;

namespace GameAccessTools.SourceGenerator.Generators;

[Generator]
internal class GameDataRepositoryGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var dataEntries = context.SyntaxProvider.CreateSyntaxProvider(
                (n, _) => n is ClassDeclarationSyntax,
                (ctx, _) =>
                {
                    var classNode = (ClassDeclarationSyntax)ctx.Node;
                    if (ModelExtensions.GetDeclaredSymbol(ctx.SemanticModel, classNode) is not INamedTypeSymbol syntax)
                    {
                        return null;
                    }

                    return syntax.HasAttribute<GameDataEntryAttribute>() ? syntax : null;
                })
            .Where(m => m is not null)
            .Collect();

        var providers = context.SyntaxProvider.CreateSyntaxProvider(
                (n, _) => n is ClassDeclarationSyntax,
                (ctx, _) =>
                {
                    var classNode = (ClassDeclarationSyntax)ctx.Node;
                    if (ModelExtensions.GetDeclaredSymbol(ctx.SemanticModel, classNode) is not INamedTypeSymbol syntax)
                    {
                        return null;
                    }

                    return syntax.HasAttribute<GameDataRepositoryProviderAttribute>() ? syntax : null;
                })
            .Where(m => m is not null)
            .Collect();

        var combinedInfo = dataEntries.Combine(providers);

        context.RegisterSourceOutput(combinedInfo, (ctx, info) => { Execute(ctx, info.Left!, info.Right!); });
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<INamedTypeSymbol> entryTypes,
        ImmutableArray<INamedTypeSymbol> providerTypes)
    {
        var foundDataEntries = entryTypes
            .Select(x => GenerateGameAssetData(context, x))
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .ToDictionary(x => x.AssetClassName);

        foreach (var providerType in providerTypes)
        {
            GenerateProviderType(context, providerType, foundDataEntries);
        }
    }

    private static GameDataRepositoryInfo? GenerateGameAssetData(SourceProductionContext context,
        INamedTypeSymbol classSymbol)
    {
        var isValidType = true;

        var uclassAttributeInfo = classSymbol.GetAttributes()
            .SingleOrDefault(x => x.AttributeClass?.ToDisplayString() == SourceContextNames.UClassAttribute);
        if (uclassAttributeInfo is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "GDA0001",
                    "GameDataEntry must be annotated with UClass",
                    "{0} must be annotated with UClass",
                    "GameDataAccessTools",
                    DiagnosticSeverity.Error,
                    true
                ),
                classSymbol.Locations.First(),
                classSymbol.Name
            ));
            isValidType = false;
        }
        else if (uclassAttributeInfo.ConstructorArguments[0].Value is ulong propertyTag &&
                 (propertyTag & SourceContextNames.EditInlineNew) == 0)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "GDA0002",
                    "GameDataEntry must have EditInlineNew flag set on UClass",
                    "{0} must have EditInlineNew flag set on UClass",
                    "GameDataAccessTools",
                    DiagnosticSeverity.Error,
                    true
                ),
                classSymbol.Locations.First(),
                classSymbol.Name
            ));
            isValidType = false;
        }

        var baseType = classSymbol.BaseType;
        while (baseType is not null)
        {
            if (baseType.ToDisplayString() == SourceContextNames.UGameDataEntry)
            {
                break;
            }

            baseType = baseType.BaseType;
        }

        if (baseType is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "GDA0003",
                    "GameDataEntry must inherit from UGameDataEntry",
                    "{0} must inherit from UGameDataEntry",
                    "GameDataAccessTools",
                    DiagnosticSeverity.Error,
                    true),
                classSymbol.Locations.First(),
                classSymbol.Name
            ));
            isValidType = false;
        }

        if (!isValidType)
        {
            return null;
        }

        var gameDataEntryInfo = classSymbol.GetAttributes().GetGameDataEntryInfos().Single();
        string generatedClassName;
        if (gameDataEntryInfo.GeneratedClassName is not null)
        {
            generatedClassName = gameDataEntryInfo.GeneratedClassName;
        }
        else
        {
            generatedClassName = classSymbol.Name.EndsWith("Data")
                ? $"U{classSymbol.Name[1..]}Repository"
                : $"U{classSymbol.Name[1..]}DataRepository";
        }

        var templateParams = new GameDataRepositoryInfo(classSymbol, generatedClassName);

        var handlebars = Handlebars.Create();
        handlebars.Configuration.TextEncoder = null;

        context.AddSource($"{generatedClassName[1..]}.g.cs",
            handlebars.Compile(SourceTemplates.GameDataRepositoryTemplate)(templateParams));
        return templateParams;
    }

    private static void GenerateProviderType(SourceProductionContext context, INamedTypeSymbol providerType,
        Dictionary<string, GameDataRepositoryInfo> foundDataEntries)
    {
        var providerInfo = providerType.GetAttributes().GetGameDataRepositoryProviderInfos()
            .Single();

        var isValidType = true;

        if (!providerType.DeclaringSyntaxReferences
                .Select(r => r.GetSyntax())
                .OfType<ClassDeclarationSyntax>()
                .SelectMany(x => x.Modifiers)
                .Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "GDA0004",
                    "Provider type must be declared as partial",
                    "{0} must be declared as partial",
                    "GameDataAccessTools",
                    DiagnosticSeverity.Error,
                    true
                ),
                providerType.Locations.First(),
                providerType.Name
            ));
            isValidType = false;
        }

        if (!isValidType)
        {
            return;
        }

        var templateParams = new
        {
            Namespace = providerType.ContainingNamespace?.ToDisplayString(),
            ClassName = providerType.Name,
            DisplayName = providerInfo.SettingsDisplayName,
            HasDisplayName = providerInfo.SettingsDisplayName is not null,
            Repositories = providerType.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.IsStatic)
                .Where(p => p.DeclaredAccessibility == Accessibility.Public)
                .Where(p => p.Type is INamedTypeSymbol namedType && IsGameDataRepository(namedType, foundDataEntries))
                .Where(p => p.GetMethod is not null && p.SetMethod is null)
                .Where(p => p.DeclaringSyntaxReferences
                    .Select(r => r.GetSyntax())
                    .OfType<PropertyDeclarationSyntax>()
                    .Any(x => x.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))))
                .Select(x => new RepositoryPropertyInfo
                {
                    Type = x.Type.BaseType is not null
                        ? x.Type.ToDisplayString()
                        : GetFormattedName(foundDataEntries, x.Type.Name),
                    Name = x.Name,
                    RepositoryClassName = x.Type.Name[1..],
                    Category = x.GetAttributes().GetSettingsCategoryInfos()
                        .SingleOrDefault().Name
                })
                .ToImmutableArray()
        };

        var handlebars = Handlebars.Create();
        handlebars.Configuration.TextEncoder = null;

        context.AddSource($"{providerType.Name}.g.cs",
            handlebars.Compile(SourceTemplates.GameDataRepositoryProviderTemplate)(templateParams));
    }

    private static bool IsGameDataRepository(INamedTypeSymbol type,
        Dictionary<string, GameDataRepositoryInfo> foundDataEntries)
    {
        if (type.BaseType?.ToDisplayString() == SourceContextNames.UGameDataRepository
            && type.Interfaces.Any(i =>
                i.IsGenericType && i.ConstructedFrom.ToDisplayString() == SourceContextNames.IGameDataRepository))
        {
            return true;
        }

        return foundDataEntries.ContainsKey(type.Name);
    }

    private static string GetFormattedName(Dictionary<string, GameDataRepositoryInfo> foundDataEntries, string name)
    {
        return foundDataEntries.TryGetValue(name, out var entry) ? $"{entry.Namespace}.{entry.AssetClassName}" : name;
    }
}