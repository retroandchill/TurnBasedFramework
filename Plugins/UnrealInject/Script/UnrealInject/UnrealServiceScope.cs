using Autofac;

namespace UnrealInject;

public enum UnrealScopes {
  GameInstance,
  World,
  LocalPlayer
}

public interface IUnrealServiceScope : IServiceProvider {
  ILifetimeScope LifetimeScope { get; }
}
