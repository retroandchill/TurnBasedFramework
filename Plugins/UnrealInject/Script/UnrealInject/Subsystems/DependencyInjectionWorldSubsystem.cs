using Autofac;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject.Subsystems;

[UClass]
public class UDependencyInjectionWorldSubsystem : UCSWorldSubsystem, IUnrealServiceScope {
  public ILifetimeScope LifetimeScope { get; private set; } = null!;

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {
    var gameInstanceSubsystem = GetGameInstanceSubsystem<UDependencyInjectionGameInstanceSubsystem>();
    LifetimeScope = gameInstanceSubsystem.LifetimeScope.BeginLifetimeScope(UnrealScope.World);
  }

  protected override void Deinitialize() {
    LifetimeScope.Dispose();
  }

  protected override bool DoesSupportWorldType(ECSWorldType worldType) {
    return worldType is ECSWorldType.Game or ECSWorldType.PIE;
  }

  public object? GetService(Type serviceType) {
    return LifetimeScope.ResolveOptional(serviceType);
  }
}
