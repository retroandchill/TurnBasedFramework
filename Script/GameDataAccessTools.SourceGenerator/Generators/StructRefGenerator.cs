using System.Collections.Immutable;
using GameAccessTools.SourceGenerator.Attributes;
using GameAccessTools.SourceGenerator.Model;
using GameAccessTools.SourceGenerator.Properties;
using GameAccessTools.SourceGenerator.Utilities;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameAccessTools.SourceGenerator.Generators;

[Generator]
public class StructRefGenerator : IIncrementalGenerator {
  public void Initialize(IncrementalGeneratorInitializationContext context) {
    var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(
      predicate: (s, _) => s is StructDeclarationSyntax,
      transform: (ctx, _) => {
        var classNode = (TypeDeclarationSyntax)ctx.Node;
        if (ctx.SemanticModel.GetDeclaredSymbol(classNode) is not INamedTypeSymbol symbol) {
          return null;
        }

        var hasReferenceForAttribute = symbol.GetAttributes()
            .Any(attr => attr.AttributeClass?.ToDisplayString() == typeof(ReferenceForAttribute).FullName);

        return hasReferenceForAttribute ? symbol : null;
      })
        .Where(m => m is not null)
        .Collect();

    context.RegisterSourceOutput(syntaxProvider, (spc, source) => {
      foreach (var classSymbol in source) {
        GenerateStructRefData(classSymbol!, spc);
      }
    });
  }
  
  private static void GenerateStructRefData(INamedTypeSymbol classSymbol, SourceProductionContext context) {
    var referenceForAttribute = classSymbol.GetAttributes()
        .GetReferenceForOverviews()
        .Single();

    var properties = referenceForAttribute.Type.GetMembers()
        .OfType<IFieldSymbol>()
        .Where(x => !x.IsStatic)
        .Select(x => x.GetPropertyInfo())
        .ToImmutableArray();

    var templateParams = new {
        Namespace = classSymbol.ContainingNamespace.ToDisplayString(),
        StructName = classSymbol.Name,
        ReferenceStructName = referenceForAttribute.Type.ToDisplayString(),
        ReferenceStructNamespace = referenceForAttribute.Type.ContainingNamespace.ToDisplayString(),
        ReferenceStructEngineName = referenceForAttribute.Type.Name[1..],
        referenceForAttribute.IsReadOnly,
        Properties = properties
    };
    
    var handlebars = Handlebars.Create();
    handlebars.Configuration.TextEncoder = null;
    
    context.AddSource($"{classSymbol.Name}.g.cs", handlebars.Compile(SourceTemplates.RefStructTemplate)(templateParams));
  }
}