using Autofac;
using UnrealInject;
using UnrealSharp.Attributes;
using UnrealSharp.Engine.Core.Modules;

namespace ManagedTurnBasedExamples;

public class FManagedTurnBasedExamples : IModuleInterface {
  public void StartupModule() {
    FUnrealInjectModule.Instance.ContainerBuilder.RegisterType<SampleService>()
        .As<ISampleService>()
        .InstancePerMatchingLifetimeScope(UnrealScope.GameInstance);
  }

  public void ShutdownModule() {
  }
}
