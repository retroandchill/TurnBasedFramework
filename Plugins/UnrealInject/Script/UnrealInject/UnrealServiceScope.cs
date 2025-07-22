using Autofac;

namespace UnrealInject;

public enum UnrealScope {
  GameInstance,
  World,
  LocalPlayer
}

public interface IUnrealServiceScope : IServiceProvider {
  ILifetimeScope LifetimeScope { get; }
}
