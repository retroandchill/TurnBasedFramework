using UnrealSharp.Binds;

namespace TurnBased.UI.Interop;

[NativeCallbacks]
public static unsafe partial class UIManagedCallbacksExporter
{
    private static readonly delegate* unmanaged<ref UIManagedActions, void> SetActions;
}