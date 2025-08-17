using System.Text.Json;
using System.Text.Json.Serialization;
using GameDataAccessTools.Core.Interop;
using GameDataAccessTools.Core.Serialization;
using GameDataAccessTools.Core.Serialization.Json;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnrealInject;
using UnrealSharp.Engine.Core.Modules;

namespace GameDataAccessTools.Core;

[UsedImplicitly]
public class FManagedGameDataAccessTools : IModuleInterface
{
    public void StartupModule()
    {
        var serializationActions = SerializationActions.Create();
        SerializationExporter.CallAssignSerializationActions(ref serializationActions);

        FUnrealInjectModule.Instance.ConfigureServices(services =>
        {
            services.ConfigureJsonSerialization()
                .AddSingleton(typeof(IGameDataEntrySerializer<>), typeof(GameDataEntryJsonSerializer<>));
        });
    }

    public void ShutdownModule()
    {
        FUnrealInjectModule.Instance.ConfigureServices(services =>
        {
            services.RemoveJsonSerialization()
                .RemoveAll(typeof(IGameDataEntrySerializer<>));
        });
    }
}