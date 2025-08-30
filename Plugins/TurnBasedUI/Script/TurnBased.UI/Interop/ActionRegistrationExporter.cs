using TurnBased.UI.Actions;
using UnrealSharp.Binds;

namespace TurnBased.UI.Interop;

[NativeCallbacks]
public static unsafe partial class ActionRegistrationExporter
{
    private static readonly delegate* unmanaged<IntPtr, out IntPtr, out int, void> GetActionBindings;
    private static readonly delegate* unmanaged<IntPtr, ref FBindUIActionArgs, IntPtr, void> RegisterActionBinding;
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> AddActionBinding;
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> RemoveActionBinding;
    private static readonly delegate* unmanaged<IntPtr, void> DestructHandle;
}