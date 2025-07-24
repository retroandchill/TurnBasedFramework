using ManagedGameDataAccessToolsEditor.Interop;
using ManagedGameDataAccessToolsEditor.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnrealInject;
using UnrealSharp.Engine.Core.Modules;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessToolsEditor;

public class FManagedGameDataAccessToolsEditor : IModuleInterface
{
    public void StartupModule()
    {
        var serializationActions = SerializationActions.Create();
        SerializationExporter.CallAssignSerializationActions(ref serializationActions);
        
        FUnrealInjectModule.Instance.Services.AddSingleton(typeof(IGameDataEntrySerializer<>), typeof(GameDataEntryJsonSerializer<>));
    }

    public void ShutdownModule()
    {
        FUnrealInjectModule.Instance.Services.RemoveAll(typeof(IGameDataEntrySerializer<>));
    }
}