using Autofac;
using JetBrains.Annotations;
using UnrealSharp.Engine.Core.Modules;

namespace UnrealInject;

public class FUnrealInjectModule : IModuleInterface {

  private static FUnrealInjectModule? _instance;
  public ContainerBuilder ContainerBuilder { get; private set; } = null!;

  public static FUnrealInjectModule Instance {
    get {
      if (_instance is null) {
        throw new InvalidOperationException("The UnrealInject module is not initialized.");
      }

      return _instance;
    }
  }

  public void StartupModule() {
    _instance = this;
  }

  public void ShutdownModule() {
    _instance = null;
  }
}
