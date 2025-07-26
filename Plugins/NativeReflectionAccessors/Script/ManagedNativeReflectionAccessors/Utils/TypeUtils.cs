using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Utils;

public static class TypeUtils
{
    public static Type RetrieveManagedType(IntPtr nativeClass)
    {
        var classHandle = UClassExporter.CallGetDefaultFromInstance(nativeClass);

        if (classHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Invalid class handle.");
        }

        var obj = GCHandleUtilities.GetObjectFromHandlePtr<object>(classHandle);
        
        if (obj == null)
        {
            throw new InvalidOperationException("Invalid class object.");
        }

        return obj.GetType();
    }
}