using GameAccessTools.SourceGenerator.Attributes;
using JetBrains.Annotations;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.GameDataAccessTools;

namespace ManagedTurnBasedExamples.Data;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
[UsedImplicitly]
public class USkillData : UGameDataEntry
{
    [UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadOnly)]
    public string Name { get; internal set; }

    [UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadOnly)]
    public int Value { get; internal set; }

    [UProperty(PropertyFlags.EditAnywhere | PropertyFlags.BlueprintReadOnly)]
    public UTexture2D Icon { get; internal set; }
}