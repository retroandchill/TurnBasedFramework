using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject;

[UClass]
[UsedImplicitly]
public class UDependencyInjectionWorldSubsystem : UCSWorldSubsystem, IServiceScope {
  private IServiceScope _scope = null!;

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

  public IServiceProvider ServiceProvider => _scope.ServiceProvider;
}
