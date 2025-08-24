using TurnBased.Core.Interop;
using UnrealSharp.Engine.Core.Modules;

namespace TurnBased.Core;

public class FTurnBasedCoreModule : IModuleInterface
{
    public void StartupModule()
    {
        var actions = TurnBasedManagedActions.Create();
        TurnBasedCallbacksExporter.CallSetActions(ref actions);
    }

    public void ShutdownModule()
    {

    }
}