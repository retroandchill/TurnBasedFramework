using UnrealInject.Subsystems;
using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace ManagedTurnBasedExamples;

[UClass]
public class USampleWorldSubsystem : UCSWorldSubsystem {

  private ISampleService _sampleService;

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {
    var worldSubsystem = collection.InitializeDependency<UDependencyInjectionWorldSubsystem>();
    _sampleService = (ISampleService) worldSubsystem.GetService(typeof(ISampleService));
  }

  protected override bool DoesSupportWorldType(ECSWorldType worldType) {
    return worldType is ECSWorldType.Game or ECSWorldType.PIE;
  }
}
