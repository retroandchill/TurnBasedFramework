using UnrealSharp.Binds;

namespace TurnBased.Core.Interop;

[NativeCallbacks]
public static unsafe partial class TurnBasedCallbacksExporter
{
    private static readonly delegate *unmanaged<ref TurnBasedManagedActions, void> SetActions;
}