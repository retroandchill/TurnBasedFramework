using UnrealSharp.Core.Marshallers;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class BlittableProperty<T>(IntPtr nativePtr) : UnrealProperty(nativePtr) where T : unmanaged
{
    public override Type Type => typeof(T);
    public override object? GetValue(IntPtr nativePtr)
    {
        return BlittableMarshaller<T>.FromNative(IntPtr.Add(nativePtr, Offset), 0);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        BlittableMarshaller<T>.ToNative(IntPtr.Add(nativePtr, Offset), 0, (T) value);
    }
}