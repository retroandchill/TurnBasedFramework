#if GAME_DATA_ACCESS_TOOLS_GENERATOR
using RhoMicro.CodeAnalysis;
#endif

namespace GameAccessTools.SourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
#if GAME_DATA_ACCESS_TOOLS_GENERATOR
[IncludeFile]
#endif
internal class DataHandleAttribute(string? pluralName = null) : Attribute
{
    public string? PluralName { get; } = pluralName;
}