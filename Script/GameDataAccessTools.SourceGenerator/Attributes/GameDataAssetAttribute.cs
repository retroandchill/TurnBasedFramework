#if GAME_DATA_ACCESS_TOOLS_GENERATOR
using RhoMicro.CodeAnalysis;
#else
using ManagedGameDataAccessTools.DataRetrieval;
using UnrealSharp.CoreUObject;
#endif

namespace GameAccessTools.SourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class)]
#if GAME_DATA_ACCESS_TOOLS_GENERATOR
[IncludeFile]
internal class GameDataAssetAttribute<T> : Attribute;
#else
internal class GameDataAssetAttribute<T> : Attribute where T : UObject, IGameDataEntry;
#endif