using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject;

[UClass]
[UsedImplicitly]
public class UDependencyInjectionSubsystem : UCSGameInstanceSubsystem, IServiceProvider, IServiceScopeFactory {
  private IServiceProvider _serviceProvider = null!;

  public bool SupportsScopes => _serviceProvider is IServiceScopeFactory;

  protected override void Initialize(FSubsystemCollectionBaseRef collection) {
    _serviceProvider = FUnrealInjectModule.Instance.CreateServiceProvider();
  }

  protected override void Deinitialize() {
    if (_serviceProvider is IDisposable disposable) {
      disposable.Dispose();
    }
  }

  protected override bool ShouldCreateSubsystem() {
    return FUnrealInjectModule.Instance.ShouldCreateServiceProvider;
  }

  public object? GetService(Type serviceType) {
    if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceScopeFactory)) {
      return this;
    }

    return _serviceProvider.GetService(serviceType);
  }

  public IServiceScope CreateScope() {
    if (_serviceProvider is not IServiceScopeFactory scopeFactory) {
      throw new InvalidOperationException("The service provider does not support scopes.");
    }

    return scopeFactory.CreateScope();
  }
}
