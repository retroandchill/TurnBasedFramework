#if GAME_DATA_ACCESS_TOOLS_GENERATOR
using RhoMicro.CodeAnalysis;
#else
using UnrealSharp.CoreUObject;
using GameDataAccessTools.Core.DataRetrieval;
#endif

namespace GameAccessTools.SourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
#if GAME_DATA_ACCESS_TOOLS_GENERATOR
[IncludeFile]
public class AccessorForAttribute<TTarget> : Attribute;
#else
public class AccessorForAttribute<TTarget> : Attribute where TTarget : UObject, IGameDataEntry;
#endif