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
    LifetimeScope = gameInstanceSubsystem.LifetimeScope.BeginLifetimeScope(UnrealScope.World, b => {
      b.RegisterInstance(World).As<UWorld>();
      b.RegisterSource(registrationSource);
    });
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
