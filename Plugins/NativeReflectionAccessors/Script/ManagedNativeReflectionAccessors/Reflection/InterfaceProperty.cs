using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class InterfaceProperty(IntPtr nativePtr) : UnrealProperty(nativePtr)
{
    public override Type Type { get; } = TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetInterfaceClass(nativePtr));
    public override object? GetValue(IntPtr nativePtr)
    {
        throw new NotImplementedException();
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        throw new NotImplementedException();
    }
}