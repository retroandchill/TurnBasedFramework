using UnrealSharp;
using UnrealSharp.Binds;

namespace ManagedNativeReflectionAccessors.Interop;

[NativeCallbacks]
public static unsafe partial class PropertyMetadataExporter
{
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> GetName;
    private static readonly delegate* unmanaged<IntPtr, FName> GetFName;
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> GetDisplayName;
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> GetToolTip;
    private static readonly delegate* unmanaged<IntPtr, bool> IsNativeBool;
    private static readonly delegate* unmanaged<IntPtr, byte> GetFieldMask;
    private static readonly delegate* unmanaged<IntPtr, IntPtr> GetEnum;
    private static readonly delegate* unmanaged<IntPtr, IntPtr> GetObjectClass;
    private static readonly delegate* unmanaged<IntPtr, IntPtr> GetMetaClass;
    private static readonly delegate* unmanaged<IntPtr, IntPtr> GetInterfaceClass;
}