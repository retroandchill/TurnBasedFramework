using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnrealSharp.Core;

namespace TurnBased.UI.Interop;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct UIManagedActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, float, void> CallFloatDelegate { get; init; }
    
    public static UIManagedActions Create()
    {
        return new UIManagedActions
        {
            CallFloatDelegate = &UIManagedCallbacks.CallFloatDelegate,
        };
    }
}

public static class UIManagedCallbacks
{
    [UnmanagedCallersOnly]
    public static void CallFloatDelegate(IntPtr delegatePtr, float value)
    {
        var action = GCHandleUtilities.GetObjectFromHandlePtr<Action<float>>(delegatePtr);
        if (action is null)
        {
            throw new InvalidOperationException("Invalid delegate.");
        }
        
        action(value);
    }
}