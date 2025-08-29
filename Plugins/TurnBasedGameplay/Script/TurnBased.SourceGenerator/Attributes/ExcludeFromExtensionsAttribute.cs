#if TURN_BASED_GAMEPLAY_GENERATOR
using RhoMicro.CodeAnalysis;
#endif

namespace TurnBased.SourceGenerator.Attributes;

#if TURN_BASED_GAMEPLAY_GENERATOR
[IncludeFile]
#endif
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
internal class ExcludeFromExtensionsAttribute : Attribute;