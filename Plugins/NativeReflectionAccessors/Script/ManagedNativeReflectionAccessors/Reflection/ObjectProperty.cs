using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class ObjectProperty(IntPtr nativePtr) : UnrealProperty(nativePtr)
{
    public override Type Type { get; } = TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetObjectClass(nativePtr));


    public UClass ObjectClass
    {
        get
        {
            var nativeClass = PropertyMetadataExporter.CallGetObjectClass(NativePtr);
            var handle = FCSManagerExporter.CallFindManagedObject(nativeClass);
            return GCHandleUtilities.GetObjectFromHandlePtr<UClass>(handle)!;
        }
    }

    public override object? GetValue(IntPtr nativePtr)
    {
        return typeof(ObjectMarshaller<>).MakeGenericType(Type)
            .GetMethod("FromNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0]);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        typeof(ObjectMarshaller<>).MakeGenericType(Type)
            .GetMethod("ToNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0, value]);
    }
}