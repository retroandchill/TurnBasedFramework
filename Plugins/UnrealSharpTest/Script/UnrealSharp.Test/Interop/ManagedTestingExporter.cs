using UnrealSharp.Binds;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;

namespace UnrealSharp.Test.Interop;

[NativeCallbacks]
public static unsafe partial class ManagedTestingExporter
{
    private static readonly delegate* unmanaged<ref ManagedTestingActions, void> SetManagedActions;
    private static readonly delegate* unmanaged<FName, IntPtr> FindUserAssembly;
    private static readonly delegate* unmanaged<ref UnmanagedArray, IntPtr, IntPtr, void> AddTestCase;
}