using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp.Engine.Core.Modules;

namespace UnrealInject;

public sealed class FUnrealInjectModule : IModuleInterface
{
    private static FUnrealInjectModule? _instance;

    private readonly ServiceCollection _serviceCollection = [];
    
    public IServiceCollection Services => _serviceCollection;

    private IServiceProviderFactory<object> _serviceProviderFactory =
        new ServiceProviderFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());

    public static FUnrealInjectModule Instance
    {
        get
        {
            if (_instance is null)
            {
                throw new InvalidOperationException("The UnrealInject module is not initialized.");
            }

            return _instance;
        }
    }

    public void StartupModule()
    {
        _instance = this;
    }

    public void ShutdownModule()
    {
        _instance = null;
    }

    public FUnrealInjectModule UseServiceProviderFactory<TBuilder>(IServiceProviderFactory<TBuilder> factory)
        where TBuilder : notnull
    {
        _serviceProviderFactory = new ServiceProviderFactoryAdapter<TBuilder>(factory);
        return this;
    }

    internal IServiceProvider BuildServiceProvider()
    {
        var containerBuilder = _serviceProviderFactory.CreateBuilder(_serviceCollection);
        return _serviceProviderFactory.CreateServiceProvider(containerBuilder);
    }

    private sealed class ServiceProviderFactoryAdapter<TBuilder>([ReadOnly] IServiceProviderFactory<TBuilder> factory)
        : IServiceProviderFactory<object> where TBuilder : notnull
    {
        public object CreateBuilder(IServiceCollection services)
        {
            return factory.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(object containerBuilder)
        {
            return factory.CreateServiceProvider((TBuilder)containerBuilder);
        }
    }
}