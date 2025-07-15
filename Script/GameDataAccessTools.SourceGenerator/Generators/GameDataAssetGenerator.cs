using GameAccessTools.SourceGenerator.Attributes;
using GameAccessTools.SourceGenerator.Model;
using GameAccessTools.SourceGenerator.Properties;
using GameAccessTools.SourceGenerator.Utilities;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Retro.SourceGeneratorUtilities.Utilities.Attributes;

namespace GameAccessTools.SourceGenerator.Generators;

[Generator]
internal class GameDataAssetGenerator : IIncrementalGenerator {

  public void Initialize(IncrementalGeneratorInitializationContext context) {
    var dataEntries = context.SyntaxProvider.CreateSyntaxProvider(
        (n, _) => n is ClassDeclarationSyntax,
        (ctx, _) => {
          var classNode = (ClassDeclarationSyntax) ctx.Node;
          if (ctx.SemanticModel.GetDeclaredSymbol(classNode) is not INamedTypeSymbol syntax) {
            return null;
          }

          return syntax.HasAttribute<GameDataEntryAttribute>() ? syntax : null;
        })
      .Where(m => m is not null);
    
    context.RegisterSourceOutput(dataEntries, GenerateGameAssetData!);
  }
  
  private static void GenerateGameAssetData(SourceProductionContext context, INamedTypeSymbol classSymbol) {
    bool isValidType = true;
    if (!classSymbol.Interfaces.Any(x => x.ToDisplayString() == SourceContextNames.IGameDataEntry)) {
      context.ReportDiagnostic(Diagnostic.Create(
        new DiagnosticDescriptor(
          "GDA0001",
          "GameDataEntry must implement IGameDataEntry",
          "{0} must implement IGameDataEntry",
          "GameDataAccessTools",
          DiagnosticSeverity.Error,
          true),
        classSymbol.Locations.First(),
        classSymbol.Name
        ));
      isValidType = false;
    }

    var uclassAttributeInfo = classSymbol.GetAttributes()
      .SingleOrDefault(x => x.AttributeClass?.ToDisplayString() == SourceContextNames.UClassAttribute);
    if (uclassAttributeInfo is null) {
      context.ReportDiagnostic(Diagnostic.Create(
        new DiagnosticDescriptor(
          "GDA0002",
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
    } else if (uclassAttributeInfo.ConstructorArguments[0].Value is ulong propertyTag && (propertyTag & SourceContextNames.EditInlineNew) == 0) {
      context.ReportDiagnostic(Diagnostic.Create(
        new DiagnosticDescriptor(
          "GDA0003",
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
    while (baseType is not null) {
      if (baseType.ToDisplayString() == SourceContextNames.UObject) {
        break;
      }
      
      if (baseType.ToDisplayString() == SourceContextNames.AActor) {
        context.ReportDiagnostic(Diagnostic.Create(
          new DiagnosticDescriptor(
            "GDA0004",
            "GameDataEntry may not inherit AActor",
            "{0} cannot inherit from AActor",
            "GameDataAccessTools",
            DiagnosticSeverity.Error,
            true
          ),
          classSymbol.Locations.First(),
          classSymbol.Name
        ));
        isValidType = false;
        break;
      }
      
      baseType = baseType.BaseType;
    }
    
    if (!isValidType) {
      return;
    }
    
    var gameDataEntryInfo = classSymbol.GetAttributes().GetGameDataEntryInfos().Single();
    string generatedClassName;
    if (gameDataEntryInfo.GeneratedClassName is not null) {
      generatedClassName = gameDataEntryInfo.GeneratedClassName;
    } else {
      generatedClassName = classSymbol.Name.EndsWith("Data") ? $"U{classSymbol.Name[1..]}Asset" : $"U{classSymbol.Name[1..]}DataAsset";
    }

    var templateParams = new {
      Namespace = classSymbol.ContainingNamespace.ToDisplayString(),
      AssetClassName = generatedClassName,
      EntryName = classSymbol.Name
    };
    
    var handlebars = Handlebars.Create();
    handlebars.Configuration.TextEncoder = null;
    
    context.AddSource($"{generatedClassName[1..]}.g.cs", handlebars.Compile(SourceTemplates.GameDataAssetTemplate)(templateParams));
  }
}