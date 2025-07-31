using GameDataAccessTools.Core.Serialization;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pokemon.Data.Core;
using Pokemon.Editor.Serializers.Json;
using UnrealInject;
using UnrealSharp.Engine.Core.Modules;

namespace Pokemon.Editor;

[UsedImplicitly]
public class FPokemonEditor : IModuleInterface
{
    public void StartupModule()
    {
        FUnrealInjectModule.Instance.ConfigureServices(services => services
            .AddSingleton<IGameDataEntrySerializer<UGrowthRate>, GrowthRateJsonSerializer>());
    }

    public void ShutdownModule()
    {
        FUnrealInjectModule.Instance.ConfigureServices(services => services
            .RemoveAll<IGameDataEntrySerializer<UGrowthRate>>());
    }
}