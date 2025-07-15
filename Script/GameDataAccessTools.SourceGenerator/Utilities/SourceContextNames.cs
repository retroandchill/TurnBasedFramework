namespace GameAccessTools.SourceGenerator.Utilities;

public static class SourceContextNames {
  public const string RootNamespace = "ManagedGameDataAccessTools";
  
  public const string DataRetrievalNamespace = $"{RootNamespace}.DataRetrieval";
  
  public const string IGameDataEntry = $"{DataRetrievalNamespace}.IGameDataEntry";

  public const string UObject = "UnrealSharp.CoreUObject.UObject";
  
  public const string AActor = "UnrealSharp.Engine.AActor";
  
  public const string UClassAttribute = "UnrealSharp.Attributes.UClassAttribute";

  public const ulong EditInlineNew = 0x00001000u;
}