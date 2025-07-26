using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class WeakObjectProperty(IntPtr nativePtr) : UnrealProperty(nativePtr)
{
    public override Type Type { get; } =
        typeof(TWeakObjectPtr<>).MakeGenericType(
            TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetWrappedType(nativePtr)));


    public UClass ObjectClass
    {
        get
        {
            var nativeClass = PropertyMetadataExporter.CallGetWrappedType(NativePtr);
            var handle = FCSManagerExporter.CallFindManagedObject(nativeClass);
            return GCHandleUtilities.GetObjectFromHandlePtr<UClass>(handle)!;
        }
    }

    public override object? GetValue(IntPtr nativePtr)
    {
        return typeof(BlittableMarshaller<>).MakeGenericType(Type)
            .GetMethod("FromNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0]);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        typeof(BlittableMarshaller<>).MakeGenericType(Type)
            .GetMethod("ToNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0, value]);
    }
}