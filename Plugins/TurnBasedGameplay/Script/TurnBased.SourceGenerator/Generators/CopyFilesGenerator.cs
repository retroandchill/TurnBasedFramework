using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis.Generated;

namespace TurnBased.SourceGenerator.Generators;

[Generator]
internal class CopyFilesGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncludedFileSources.RegisterToContext(context);
    }
}
