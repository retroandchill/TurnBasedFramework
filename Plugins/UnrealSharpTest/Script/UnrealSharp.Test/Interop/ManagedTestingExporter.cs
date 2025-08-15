using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace UnrealSharp.Test.Interop;

[NativeCallbacks]
public static unsafe partial class ManagedTestingExporter
{
    private static readonly delegate* unmanaged<ref ManagedTestingActions, void> SetManagedActions;
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> AddTest;
}