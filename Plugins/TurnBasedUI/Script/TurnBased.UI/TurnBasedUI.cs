using JetBrains.Annotations;
using UnrealSharp.Engine.Core.Modules;
using UnrealSharp.Log;

namespace TurnBased.UI;

[CustomLog]
public static partial class LogTurnBasedUI;

[UsedImplicitly]
public class FTurnBasedUIModule : IModuleInterface
{
    public void StartupModule()
    {
    }

    public void ShutdownModule() { }
}
