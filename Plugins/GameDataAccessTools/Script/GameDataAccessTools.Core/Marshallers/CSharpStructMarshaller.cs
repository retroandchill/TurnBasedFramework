using System.Reflection;
using UnrealSharp.Core.Marshallers;

namespace GameDataAccessTools.Core.Marshallers;

public static class CSharpStructMarshaller<T> where T : struct
{
    private static readonly Func<IntPtr, int, T> FromNativeDelegate;
    private static readonly Action<IntPtr, int, T> ToNativeDelegate;

    static CSharpStructMarshaller()
    {
        var marshallerType = typeof(StructMarshaller<>).MakeGenericType(typeof(T));
        var fromNative = marshallerType.GetMethod("FromNative", BindingFlags.Public | BindingFlags.Static)!;
        var toNative = marshallerType.GetMethod("ToNative", BindingFlags.Public | BindingFlags.Static)!;
        
        FromNativeDelegate = (Func<IntPtr, int, T>)Delegate.CreateDelegate(typeof(Func<IntPtr, int, T>), fromNative);
        ToNativeDelegate = (Action<IntPtr, int, T>)Delegate.CreateDelegate(typeof(Action<IntPtr, int, T>), toNative);
    }
    
    public static T FromNative(IntPtr nativeBuffer, int arrayIndex)
    {
        return FromNativeDelegate(nativeBuffer, arrayIndex);
    }

    public static void ToNative(IntPtr nativeBuffer, int arrayIndex, T obj)
    {
        ToNativeDelegate(nativeBuffer, arrayIndex, obj);
    }
}