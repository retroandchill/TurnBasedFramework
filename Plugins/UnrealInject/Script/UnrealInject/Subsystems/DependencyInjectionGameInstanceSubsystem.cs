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
    var registrationSource = new UnrealSubsystemSource<UGameInstanceSubsystem>(collection);
    LifetimeScope = engineSubsystem.LifetimeScope.BeginLifetimeScope(UnrealScopes.GameInstance, b => {
      b.RegisterInstance(GameInstance).As<UGameInstance>();
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
