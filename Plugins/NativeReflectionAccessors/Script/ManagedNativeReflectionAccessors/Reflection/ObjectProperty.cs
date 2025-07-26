using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class ObjectProperty(IntPtr nativePtr) : GenericStaticMarshalledProperty(nativePtr, TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetWrappedType(nativePtr)), typeof(ObjectMarshaller<>))
{
    public UClass ObjectClass
    {
        get
        {
            var nativeClass = PropertyMetadataExporter.CallGetWrappedType(NativePtr);
            var handle = FCSManagerExporter.CallFindManagedObject(nativeClass);
            return GCHandleUtilities.GetObjectFromHandlePtr<UClass>(handle)!;
        }
    }
}