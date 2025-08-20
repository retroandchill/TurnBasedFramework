using UnrealSharp.Binds;
using UnrealSharp.CoreUObject;
using UnrealSharp.Test.Runner;

namespace UnrealSharp.Test.Interop;

[NativeCallbacks]
public static unsafe partial class AutomationTestExporter
{
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> EnqueueLatentCommand;
    private static readonly delegate* unmanaged<char*, EAutomationEventType, void> LogEvent;
}