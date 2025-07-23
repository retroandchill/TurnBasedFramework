using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.StaticVars;

namespace ManagedTurnBasedExamples.Data;

[GameDataRepositoryProvider(SettingsDisplayName = "Turn-Based Data")]
public partial class TurnBasedData {

  [SettingsCategory("PBS")]
  public static partial USkillDataRepository Skills { get; }


}
