using Autofac;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject.Subsystems;

[UClass]
public class UDependencyInjectionEngineSubsystem : UCSEngineSubsystem, IUnrealServiceScope {
  private IContainer _container = null!;
  public ILifetimeScope LifetimeScope => _container;

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {
    var containerBuilder = FUnrealInjectModule.Instance.ContainerBuilder;
    containerBuilder.RegisterSource(new UnrealSubsystemSource<UEngineSubsystem>(collection));
    _container = containerBuilder.Build();
  }

  protected override void Deinitialize() {
    _container.Dispose();
  }

  public object? GetService(Type serviceType) {
    return _container.ResolveOptional(serviceType);
  }


}
