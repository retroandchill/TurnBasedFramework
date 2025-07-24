using Microsoft.Extensions.DependencyInjection;
using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject.Subsystems;

[UClass]
public sealed class UDependencyInjectionGameInstanceSubsystem : UCSGameInstanceSubsystem, IServiceProvider, IServiceScope
{
    private IServiceScope _serviceScope = null!;
    
    public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

    protected override void Initialize(FSubsystemCollectionBaseRef collection)
    {
        var engineSubsystem = GetEngineSubsystem<UDependencyInjectionEngineSubsystem>();
        _serviceScope = engineSubsystem.CreateScope();
    }

    protected override void Deinitialize()
    {
        if (_serviceScope is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public object? GetService(Type serviceType)
    {
        return _serviceScope.ServiceProvider.GetService(serviceType);
    }
}