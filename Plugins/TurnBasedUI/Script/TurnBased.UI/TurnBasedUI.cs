using JetBrains.Annotations;
using TurnBased.UI.Interop;
using UnrealSharp.Engine.Core.Modules;

namespace TurnBased.UI;

[UsedImplicitly]
public class FTurnBasedUIModule : IModuleInterface
{
    public void StartupModule()
    {
        var actions = UIManagedActions.Create();
        UIManagedCallbacksExporter.CallSetActions(ref actions);
    }

    public void ShutdownModule() { }
}
