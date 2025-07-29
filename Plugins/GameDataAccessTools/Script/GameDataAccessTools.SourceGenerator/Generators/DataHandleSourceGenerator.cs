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
public class DataHandleSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (s, _) => s is StructDeclarationSyntax or RecordDeclarationSyntax,
                transform: (ctx, _) =>
                {
                    var classNode = (TypeDeclarationSyntax)ctx.Node;
                    if (ctx.SemanticModel.GetDeclaredSymbol(classNode) is not INamedTypeSymbol
                        {
                            IsValueType: true
                        } symbol)
                    {
                        return null;
                    }

                    var hasReferenceForAttribute = symbol.HasAttribute(typeof(DataHandleAttribute));

                    return hasReferenceForAttribute ? symbol : null;
                })
            .Where(m => m is not null);

        context.RegisterSourceOutput(syntaxProvider, GenerateDataHandleType!);
    }

    private static void GenerateDataHandleType(SourceProductionContext context, INamedTypeSymbol classSymbol)
    {
        var dataHandleInterface = classSymbol.AllInterfaces
            .SingleOrDefault(a => a.IsGenericType && a.TypeParameters.Length == 2
                                                  && a.ContainingNamespace.ToDisplayString() ==
                                                  "ManagedGameDataAccessTools.DataRetrieval"
                                                  && a.MetadataName == "IDataHandle`2");

        if (dataHandleInterface is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "GDA1001",
                    "Data handle does not implement IDataHandle",
                    "{0} does not implement IDataHandle",
                    "GameDataAccessTools",
                    DiagnosticSeverity.Error,
                    true
                ),
                classSymbol.Locations.First(),
                classSymbol.Name
            ));
            return;
        }

        var propertyType = dataHandleInterface.TypeArguments[0];
        var constructor = classSymbol.Constructors
            .Where(c => c.DeclaredAccessibility == Accessibility.Public &&
                        c.Parameters.Length > 0 &&
                        c.Parameters[0].Type.Equals(propertyType, SymbolEqualityComparer.Default) &&
                        (c.Parameters.Length == 1 ||
                         c.Parameters.Skip(1).All(p => p.HasExplicitDefaultValue)))
            .OrderBy(c => c.Parameters.Length)
            .FirstOrDefault();

        if (constructor is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "GDA1002",
                    "Data entry must have a public constructor that takes the key type",
                    "{0} must have a public constructor that takes a single parameter of type {1}",
                    "GameDataAccessTools",
                    DiagnosticSeverity.Error,
                    true
                ),
                classSymbol.Locations.First(),
                classSymbol.Name,
                propertyType.ToDisplayString()
            ));
            return;
        }

        var dataHandleInfo = classSymbol.GetAttributes().GetDataHandleInfos().Single();
        var templateParams = new DataHandleParams(classSymbol, propertyType)
        {
            PluralName = dataHandleInfo.PluralName!
        };
        
        var handlebars = Handlebars.Create();
        handlebars.Configuration.TextEncoder = null;

        context.AddSource($"{classSymbol.Name}.g.cs",
            handlebars.Compile(SourceTemplates.DataHandleTemplate)(templateParams));
    }
}