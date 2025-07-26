using ManagedNativeReflectionAccessors.Interop;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class BoolProperty(IntPtr nativePtr) : UnrealProperty(nativePtr)
{
    public bool IsNativeBool => PropertyMetadataExporter.CallIsNativeBool(NativePtr);
    
    public byte FieldMask => PropertyMetadataExporter.CallGetFieldMask(NativePtr);
    
    public override Type Type => typeof(bool);

    public override object? GetValue(IntPtr nativePtr)
    {
        var offset = IntPtr.Add(nativePtr, Offset);
        return IsNativeBool ? BoolMarshaller.FromNative(offset, 0) : BitfieldBoolMarshaller.FromNative(offset, FieldMask);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var boolValue = (bool) value;
        var offset = IntPtr.Add(nativePtr, Offset);
        if (IsNativeBool)
        {
            BoolMarshaller.ToNative(offset, 0, boolValue);
        }
        else
        {
            BitfieldBoolMarshaller.ToNative(offset, FieldMask, boolValue);
        }
    }
}