using TurnBased.UI.Actions;
using UnrealSharp;
using UnrealSharp.Binds;
using UnrealSharp.CommonUI;
using UnrealSharp.Core;

namespace TurnBased.UI.Interop;

[NativeCallbacks]
public static unsafe partial class BindUIActionArgsExporter
{
    private static readonly delegate* unmanaged<IntPtr> GetExemptInputTypesProperty;
    private static readonly delegate* unmanaged<ref FBindUIActionArgs, FUIActionTag, IntPtr, void> ConstructFromActionTag;
    private static readonly delegate* unmanaged<ref FBindUIActionArgs, IntPtr, FName, IntPtr, void> ConstructFromRowHandle;
    private static readonly delegate* unmanaged<ref FBindUIActionArgs, IntPtr, IntPtr, void> ConstructFromInputAction;
    private static readonly delegate* unmanaged<ref FBindUIActionArgs, void> Destruct;
    private static readonly delegate* unmanaged<ref NativeDelegate, IntPtr, void> BindOnHoldActionProgressed;
    private static readonly delegate* unmanaged<ref NativeDelegate, IntPtr, void> BindOnHoldActionPressed;
    private static readonly delegate* unmanaged<ref NativeDelegate, IntPtr, void> BindOnHoldActionReleased;
    private static readonly delegate* unmanaged<ref FBindUIActionArgs, FName> GetActionName;
    private static readonly delegate* unmanaged<ref FBindUIActionArgs, NativeBool> ActionHasHoldMappings;
}