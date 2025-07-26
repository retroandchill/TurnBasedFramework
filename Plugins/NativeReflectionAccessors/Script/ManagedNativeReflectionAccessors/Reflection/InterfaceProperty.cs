using System.Reflection;
using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp.CoreUObject;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class InterfaceProperty : UnrealProperty
{
    
    private readonly OpaqueStaticMarshaller _staticMarshaller;
    public InterfaceProperty(IntPtr nativePtr) : base(nativePtr)
    {
        Type = TypeUtils.RetrieveManagedInterface(PropertyMetadataExporter.CallGetWrappedType(nativePtr));
        var marshallerType = typeof(ScriptInterfaceMarshaller<>).MakeGenericType(Type);
        _staticMarshaller = new OpaqueStaticMarshaller(marshallerType);
    }

    public override Type Type { get; }
    public override object? GetValue(IntPtr nativePtr)
    {
        return typeof(ScriptInterfaceMarshaller<>).MakeGenericType(Type)
            .GetMethod("FromNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0]);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        typeof(ScriptInterfaceMarshaller<>).MakeGenericType(Type)
            .GetMethod("ToNative")!
            .Invoke(null, [IntPtr.Add(nativePtr, Offset), 0, value]);
    }
}