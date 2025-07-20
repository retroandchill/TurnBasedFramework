using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject;

[UClass]
[UsedImplicitly]
public class UDependencyInjectionWorldSubsystem : UCSWorldSubsystem, IServiceProvider, IServiceScope {
  private IServiceScope _scope = null!;

  public IServiceProvider ServiceProvider => this;

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {
    var dependencyInjectionSubsystem = collection.InitializeRequiredSubsystem<UDependencyInjectionSubsystem>();
    _scope = dependencyInjectionSubsystem.CreateScope();
  }

  protected override void Deinitialize() {
    if (_scope is IDisposable disposable) {
      disposable.Dispose();
    }
  }

  protected override bool ShouldCreateSubsystem() {
    var dependencyInjectionSubsystem = GetGameInstanceSubsystem<UDependencyInjectionSubsystem>();
    return dependencyInjectionSubsystem.SupportsScopes;
  }

  public object? GetService(Type serviceType) {
    if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceScopeFactory)) {
      return this;
    }

    return _scope.ServiceProvider.GetService(serviceType);
  }
}
