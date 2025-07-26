using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public class ClassProperty(IntPtr nativePtr) : UnrealProperty(nativePtr)
{

    public override Type Type { get; } = TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetObjectClass(nativePtr));
    
    public UClass ObjectClass
    {
        get
        {
            var nativeClass = PropertyMetadataExporter.CallGetMetaClass(NativePtr);
            var handle = FCSManagerExporter.CallFindManagedObject(nativeClass);
            return GCHandleUtilities.GetObjectFromHandlePtr<UClass>(handle)!;
        }
    }

    public override object? GetValue(IntPtr nativePtr)
    {
        return typeof(SubclassOfMarshaller<>).MakeGenericType(Type)
            .GetMethod("FromNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0]);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        typeof(SubclassOfMarshaller<>).MakeGenericType(Type)
            .GetMethod("ToNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0, value]);
    }
}