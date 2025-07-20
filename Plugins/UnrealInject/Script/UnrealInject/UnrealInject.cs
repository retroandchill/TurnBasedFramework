using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using UnrealSharp.Engine.Core.Modules;

namespace UnrealInject;

public class FUnrealInjectModule : IModuleInterface {

  private static FUnrealInjectModule? _instance;
  private Func<IServiceProvider>? _serviceProviderFactory;
  private ServiceCollection? _serviceCollection;

  public static FUnrealInjectModule Instance {
    get {
      if (_instance is null) {
        throw new InvalidOperationException("The UnrealInject module is not initialized.");
      }

      return _instance;
    }
  }

  public bool ShouldCreateServiceProvider => _serviceProviderFactory is not null;

  public void StartupModule() {
    _instance = this;
  }

  public void ShutdownModule() {
    _instance = null;
  }

  [PublicAPI]
  public IServiceProvider CreateServiceProvider() {
    if (_serviceProviderFactory is null) {
      throw new InvalidOperationException("The service provider has not been configured.");
    }

    return _serviceProviderFactory();
  }

  [PublicAPI]
  public FUnrealInjectModule WithServiceProvider(Func<IServiceProvider> serviceProviderFactory) {
    if (_serviceProviderFactory is not null) {
      throw new InvalidOperationException("The service provider has already been configured.");
    }

    _serviceProviderFactory = serviceProviderFactory;

    return this;
  }

  [PublicAPI]
  public FUnrealInjectModule ConfigureServices(Action<IServiceCollection> serviceCollection) {
    if (_serviceCollection is null) {
      _serviceCollection = [];
      WithServiceProvider(() => _serviceCollection.BuildServiceProvider());
    }

    serviceCollection(_serviceCollection);
    return this;
  }
}
