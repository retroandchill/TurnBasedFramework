using Autofac;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject.Subsystems;

[UClass]
public class UDependencyInjectionLocalPlayerSubsystem : UCSLocalPlayerSubsystem, IUnrealServiceScope {

  public ILifetimeScope LifetimeScope { get; private set; } = null!;

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {
    var gameInstanceSubsystem = GetGameInstanceSubsystem<UDependencyInjectionGameInstanceSubsystem>();
    LifetimeScope = gameInstanceSubsystem.LifetimeScope.BeginLifetimeScope(UnrealScope.LocalPlayer);
  }

  protected override void Deinitialize() {
    LifetimeScope.Dispose();
  }

  public object? GetService(Type serviceType) {
    return LifetimeScope.ResolveOptional(serviceType);
  }
}
