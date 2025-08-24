using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnrealSharp.Core;

namespace UnrealSharp.TurnBasedCore;

internal readonly partial struct FManagedInitializerDelegate
{
    [UsedImplicitly]
    private readonly IntPtr _managedDelegate;

    public FManagedInitializerDelegate(Action<IntPtr>? managedDelegate)
    {
        if (managedDelegate is null)
        {
            _managedDelegate = IntPtr.Zero;
            return;
        }

        var delegateHandle = GCHandleUtilities.AllocateWeakPointer(managedDelegate);
        _managedDelegate = GCHandle.ToIntPtr(delegateHandle);
    }
}