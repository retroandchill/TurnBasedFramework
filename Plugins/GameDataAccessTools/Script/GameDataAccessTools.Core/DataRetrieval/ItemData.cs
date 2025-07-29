using System.Diagnostics.CodeAnalysis;
using GameAccessTools.SourceGenerator.Attributes;
using JetBrains.Annotations;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.GameDataAccessTools;

namespace GameDataAccessTools.Core.DataRetrieval;

// Example data entry class
[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
[UsedImplicitly]
public class UItemData : UGameDataEntry
{
    [UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadOnly)]
    public string Name { get; }

    [UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadOnly)]
    public int Value { get; }

    [UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadOnly)]
    public UTexture2D Icon { get; }
}