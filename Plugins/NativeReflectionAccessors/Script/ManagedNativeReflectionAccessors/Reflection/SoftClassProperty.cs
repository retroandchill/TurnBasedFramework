using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public class SoftClassProperty : UnrealProperty
{
    private readonly Type _objectType;
    private readonly OpaqueStaticMarshaller _marshaller;

    public SoftClassProperty(IntPtr nativePtr) : base(nativePtr)
    {
        _objectType = TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetObjectClass(nativePtr));
        Type = typeof(TSoftClassPtr<>).MakeGenericType(_objectType);
        _marshaller = new OpaqueStaticMarshaller(typeof(SoftClassMarshaller<>).MakeGenericType(_objectType));
    }

    public override Type Type { get; }
    
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
        
        return _marshaller.FromNative(IntPtr.Add(nativePtr, Offset), 0);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        typeof(SoftClassMarshaller<>).MakeGenericType(_objectType)
            .GetMethod("ToNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0, value]);
    }
}