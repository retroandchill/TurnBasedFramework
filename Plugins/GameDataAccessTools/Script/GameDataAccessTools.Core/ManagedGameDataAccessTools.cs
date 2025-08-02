using System.Text.Json;
using System.Text.Json.Serialization;
using GameDataAccessTools.Core.Interop;
using GameDataAccessTools.Core.Serialization;
using GameDataAccessTools.Core.Serialization.Json;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
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
            services.ConfigureJsonSerialization(options =>
            {
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.PropertyNameCaseInsensitive = true;
                options.AllowTrailingCommas = true;
                options.WriteIndented = true;
                
                options.Converters.Add(new JsonStringEnumConverter());
                options.Converters.Add(new NameJsonConverter());
                options.Converters.Add(new TextJsonConverter());
                options.Converters.Add(new GameplayTagJsonConverter());
                options.Converters.Add(new SubclassOfJsonConverterFactory());
                options.Converters.Add(new SoftObjectPtrJsonConverterFactory());
                options.Converters.Add(new SoftClassPtrJsonConverterFactory());
            });

            services.AddSingleton(typeof(IGameDataEntrySerializer<>), typeof(GameDataEntryJsonSerializer<>));
        });
    }

    public void ShutdownModule()
    {
    }
}