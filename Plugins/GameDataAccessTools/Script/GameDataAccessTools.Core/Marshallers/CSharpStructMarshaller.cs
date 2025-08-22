using System.Reflection;
using UnrealSharp;
using UnrealSharp.Core.Attributes;
using UnrealSharp.Core.Marshallers;

namespace GameDataAccessTools.Core.Marshallers;

public static class CSharpStructMarshaller<T>
    where T : struct
{
    private static readonly Func<IntPtr, int, T> FromNativeDelegate;
    private static readonly Action<IntPtr, int, T> ToNativeDelegate;

    static CSharpStructMarshaller()
    {
        Type marshallerType;
        if (typeof(T).GetCustomAttribute<BlittableTypeAttribute>() is not null)
        {
            marshallerType = typeof(BlittableMarshaller<>).MakeGenericType(typeof(T));
        }
        else if (
            typeof(T)
                .GetInterfaces()
                .Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(MarshalledStruct<>)
                )
        )
        {
            marshallerType = typeof(StructMarshaller<>).MakeGenericType(typeof(T));
        }
        else
        {
            marshallerType = typeof(T).Assembly.GetType($"{typeof(T).FullName}Marshaller")!;
        }
        var fromNative = marshallerType.GetMethod(
            "FromNative",
            BindingFlags.Public | BindingFlags.Static
        )!;
        var toNative = marshallerType.GetMethod(
            "ToNative",
            BindingFlags.Public | BindingFlags.Static
        )!;

        FromNativeDelegate =
            (Func<IntPtr, int, T>)Delegate.CreateDelegate(typeof(Func<IntPtr, int, T>), fromNative);
        ToNativeDelegate =
            (Action<IntPtr, int, T>)
                Delegate.CreateDelegate(typeof(Action<IntPtr, int, T>), toNative);
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
