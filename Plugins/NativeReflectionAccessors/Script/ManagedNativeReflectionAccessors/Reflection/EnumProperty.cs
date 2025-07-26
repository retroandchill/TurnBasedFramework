using ManagedNativeReflectionAccessors.Interop;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class EnumProperty<T>(IntPtr nativePtr) : UnrealProperty(nativePtr) where T : unmanaged, Enum
{
    public override Type Type => typeof(T);
    
    public UEnum Enum
    {
        get
        {
            var nativeENum = PropertyMetadataExporter.CallGetWrappedType(NativePtr);
            var handle = FCSManagerExporter.CallFindManagedObject(nativeENum);
            return GCHandleUtilities.GetObjectFromHandlePtr<UEnum>(handle)!;
        }
    }

    public override object? GetValue(IntPtr nativePtr)
    {
        return EnumMarshaller<T>.FromNative(IntPtr.Add(nativePtr, Offset), 0);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        EnumMarshaller<T>.ToNative(IntPtr.Add(nativePtr, Offset), 0, (T) value);
    }
}