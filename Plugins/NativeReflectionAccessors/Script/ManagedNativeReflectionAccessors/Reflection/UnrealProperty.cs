using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public abstract class UnrealProperty(IntPtr nativePtr)
{
    protected IntPtr NativePtr { get; } = nativePtr;
    public int Size { get; } = FPropertyExporter.CallGetSize(nativePtr);
    public int Offset { get; } = FPropertyExporter.CallGetPropertyOffset(nativePtr);
    public int ArrayDim { get; } = FPropertyExporter.CallGetArrayDim(nativePtr);

    public static UnrealProperty FromNativePtr(IntPtr nativePtr)
    {
        throw new NotImplementedException();
    }

    public string Name
    {
        get
        {
            unsafe
            {
                using var stringHandle = new StringDisposer();
                PropertyMetadataExporter.CallGetName(NativePtr, (IntPtr) (&stringHandle.NativeArray));
                return StringMarshaller.FromNative((IntPtr)(&stringHandle.NativeArray), ArrayDim);
            }
        }
    }

    public FName FName => PropertyMetadataExporter.CallGetFName(NativePtr);

    public FText DisplayName
    {
        get
        {
            unsafe
            {
                var textData = new FTextData();
                FTextExporter.CallCreateEmptyText(ref textData);
                PropertyMetadataExporter.CallGetDisplayName(NativePtr, (IntPtr) (&textData));
                return TextMarshaller.FromNative((IntPtr)(&textData), ArrayDim);
            }
        }
    }

    public FText Tooltip
    {
        get
        {
            unsafe
            {
                var textData = new FTextData();
                FTextExporter.CallCreateEmptyText(ref textData);
                PropertyMetadataExporter.CallGetToolTip(NativePtr, (IntPtr) (&textData));
                return TextMarshaller.FromNative((IntPtr)(&textData), ArrayDim);
            }
        }
    }

    public abstract Type Type { get; }

    public object GetValue(UObject obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return GetValue(obj.NativeObject);
    }

    public object GetValue<T>(T obj) where T : struct
    {
        throw new NotImplementedException();
    }
    
    public abstract object? GetValue(IntPtr nativePtr);
    
    public void SetValue(UObject obj, object value)
    {
        ArgumentNullException.ThrowIfNull(obj);
        SetValue(obj.NativeObject, value);
    }

    public void SetValue<T>(T obj, object value) where T : struct
    {
        throw new NotImplementedException();
    }

    public abstract void SetValue(IntPtr nativePtr, object? value);
}