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
    var registrationSource = new UnrealSubsystemSource<ULocalPlayerSubsystem>(collection);
    LifetimeScope = gameInstanceSubsystem.LifetimeScope.BeginLifetimeScope(UnrealScopes.World, b => {
      var localPlayer = LocalPlayer;
      b.RegisterInstance(localPlayer).As<ULocalPlayer>();
      b.RegisterSource(registrationSource);
    });
  }

  protected override void Deinitialize() {
    LifetimeScope.Dispose();
  }

  public object? GetService(Type serviceType) {
    return LifetimeScope.ResolveOptional(serviceType);
  }
}
