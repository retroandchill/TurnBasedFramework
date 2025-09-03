using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pokemon.Core.Executor.Display;
using Pokemon.UI.Async;
using UnrealInject;
using UnrealSharp.Engine.Core.Modules;

namespace Pokemon.UI;

public class FPokemonUIModule : IModuleInterface
{
    public void StartupModule()
    {
        FUnrealInjectModule.Instance.ConfigureServices(services =>
            services.AddScoped<IDisplayService, DefaultDisplayService>());
    }

    public void ShutdownModule()
    {
        FUnrealInjectModule.Instance.ConfigureServices(services =>
            services.RemoveAll<IDisplayService>());
    }
}