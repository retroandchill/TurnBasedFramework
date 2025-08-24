using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnrealSharp.Core;

namespace TurnBased.Core.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TurnBasedManagedActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr, void> OnUnitInitialized;

    public static TurnBasedManagedActions Create()
    {
        return new TurnBasedManagedActions()
        {
            OnUnitInitialized = &TurnBasedManagedCallbacks.OnUnitInitialized
        };
    }
}

public static class TurnBasedManagedCallbacks
{
    [UnmanagedCallersOnly]
    public static void OnUnitInitialized(IntPtr delegatePtr, IntPtr unitPtr)
    {
        var foundDelegate = GCHandleUtilities.GetObjectFromHandlePtr<Action<IntPtr>>(delegatePtr);
        foundDelegate?.Invoke(unitPtr);
    }
}