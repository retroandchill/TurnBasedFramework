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
    var registrationSource = new UnrealSubsystemSource<UWorldSubsystem>(collection);
    LifetimeScope = gameInstanceSubsystem.LifetimeScope.BeginLifetimeScope(UnrealScopes.LocalPlayer, b => {
      b.RegisterInstance(World).As<UWorld>();
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
