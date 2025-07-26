using System.Reflection;

namespace ManagedNativeReflectionAccessors.Reflection;

public abstract class GenericStaticMarshalledProperty : UnrealProperty
{
    private readonly MethodInfo _fromNativeMethod;
    private readonly MethodInfo _toNativeMethod;

    public sealed override Type Type { get; }
    
    protected GenericStaticMarshalledProperty(IntPtr nativePtr, Type managedType, Type marshallerType, Type marshallerParameter) : base(nativePtr)
    {
        Type = managedType.IsGenericTypeDefinition ? managedType.MakeGenericType(marshallerParameter) : managedType;
        var concreteMarshallerType = marshallerType.MakeGenericType(marshallerParameter);
        _fromNativeMethod = concreteMarshallerType.GetMethod("FromNative")!;
        _toNativeMethod = concreteMarshallerType.GetMethod("ToNative")!;
    }

    protected GenericStaticMarshalledProperty(IntPtr nativePtr, Type managedType, Type marshallerType) : this(nativePtr,
        managedType, marshallerType, managedType)
    {
        
    }
    
    public sealed override object? GetValue(IntPtr nativePtr)
    {
        return _fromNativeMethod.Invoke(null, [IntPtr.Add(nativePtr, Offset), 0]);
    }

    public sealed override void SetValue(IntPtr nativePtr, object? value)
    {
        _toNativeMethod.Invoke(null, [IntPtr.Add(nativePtr, Offset), 0, value]);
    }
}