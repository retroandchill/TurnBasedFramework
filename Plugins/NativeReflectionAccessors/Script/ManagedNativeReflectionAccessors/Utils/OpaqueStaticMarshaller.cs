using System.Reflection;

namespace ManagedNativeReflectionAccessors.Utils;

public readonly struct OpaqueStaticMarshaller(Type type)
{
    private readonly MethodInfo _fromNativeMethod = type.GetMethod("FromNative")!;
    private readonly MethodInfo _toNativeMethod = type.GetMethod("ToNative")!;

    public object? FromNative(IntPtr nativePtr, int offset)
    {
        return _fromNativeMethod.Invoke(null, [IntPtr.Add(nativePtr, offset), 0]);
    }

    public void ToNative(IntPtr nativePtr, int offset, object? value)
    {
        _toNativeMethod.Invoke(null, [IntPtr.Add(nativePtr, offset), 0, value]);
    }
    
}