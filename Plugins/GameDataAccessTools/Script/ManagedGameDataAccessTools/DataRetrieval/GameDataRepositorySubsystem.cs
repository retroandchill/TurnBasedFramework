using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.UnrealSharpCore;

namespace ManagedGameDataAccessTools.DataRetrieval;

[UClass]
public class UGameDataRepositorySubsystem : UCSGameInstanceSubsystem {

  [UProperty]
  private TArray<UGameDataRepository> Repositories { get; }

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {

  }

  protected override void Deinitialize() {
    Repositories.Clear();
  }

}
