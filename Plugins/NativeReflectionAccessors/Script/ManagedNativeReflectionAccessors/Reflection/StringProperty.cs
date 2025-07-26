using UnrealSharp.Core.Marshallers;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class StringProperty(IntPtr nativePtr) : UnrealProperty(nativePtr)
{
    public override Type Type => typeof(string);
    public override object? GetValue(IntPtr nativePtr)
    {
        return StringMarshaller.FromNative(IntPtr.Add(nativePtr, Offset), 0);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        StringMarshaller.ToNative(IntPtr.Add(nativePtr, Offset), 0, value is not null ? (string) value : string.Empty);
    }
}