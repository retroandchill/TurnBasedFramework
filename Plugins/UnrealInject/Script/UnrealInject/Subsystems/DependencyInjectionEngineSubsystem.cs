using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject.Subsystems;

[UClass]
[UsedImplicitly]
public class UDependencyInjectionEngineSubsystem
    : UCSEngineSubsystem,
        IServiceProvider,
        IServiceScopeFactory
{
    private IServiceProvider _serviceProvider = null!;

    public event Action<IServiceProvider>? OnServiceProviderRebuilt;

    protected override void Initialize(FSubsystemCollectionBaseRef collection)
    {
        FUnrealInjectModule.Instance.OnServiceProviderRebuilt += RebuildServiceProvider;

        _serviceProvider = FUnrealInjectModule.Instance.BuildServiceProvider();
    }

    protected override void Deinitialize()
    {
        FUnrealInjectModule.Instance.OnServiceProviderRebuilt -= RebuildServiceProvider;

        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void RebuildServiceProvider(IServiceProvider provider)
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
        _serviceProvider = provider;
        OnServiceProviderRebuilt?.Invoke(_serviceProvider);
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }

    public IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }

    public void RebuildServiceProvider()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
        _serviceProvider = FUnrealInjectModule.Instance.BuildServiceProvider();
    }
}
