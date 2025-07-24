using Microsoft.Extensions.DependencyInjection;
using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject.Subsystems;

[UClass]
public class UDependencyInjectionEngineSubsystem : UCSEngineSubsystem, IServiceProvider, IServiceScopeFactory
{
    private IServiceProvider _serviceProvider = null!;

    protected override void Initialize(FSubsystemCollectionBaseRef collection)
    {
        _serviceProvider = FUnrealInjectModule.Instance.BuildServiceProvider();
    }

    protected override void Deinitialize()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
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