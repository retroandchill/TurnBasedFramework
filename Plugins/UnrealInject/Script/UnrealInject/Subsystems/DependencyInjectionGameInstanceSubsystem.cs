using Autofac;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject.Subsystems;

[UClass]
public class UDependencyInjectionGameInstanceSubsystem : UCSGameInstanceSubsystem, IUnrealServiceScope {

  public ILifetimeScope LifetimeScope { get; private set; } = null!;

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {
    var engineSubsystem = GetEngineSubsystem<UDependencyInjectionEngineSubsystem>();
    LifetimeScope = engineSubsystem.LifetimeScope.BeginLifetimeScope(UnrealScope.GameInstance);
  }

  protected override void Deinitialize() {
    LifetimeScope.Dispose();
  }

  public object? GetService(Type serviceType) {
    return LifetimeScope.ResolveOptional(serviceType);
  }
}
