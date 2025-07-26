using UnrealSharp;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class TextProperty(IntPtr nativePtr) : UnrealProperty(nativePtr)
{
    public override Type Type => typeof(FText);
    public override object? GetValue(IntPtr nativePtr)
    {
        return TextMarshaller.FromNative(IntPtr.Add(nativePtr, Offset), 0);
    }

    public override void SetValue(IntPtr nativePtr, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        TextMarshaller.ToNative(IntPtr.Add(nativePtr, Offset), 0, (FText) value);
    }
}