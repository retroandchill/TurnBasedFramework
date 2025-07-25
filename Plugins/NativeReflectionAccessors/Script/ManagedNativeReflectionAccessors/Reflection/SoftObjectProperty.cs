﻿using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class SoftObjectProperty(IntPtr nativePtr) : GenericStaticMarshalledProperty(nativePtr,
    typeof(TSubclassOf<>), typeof(SubclassOfMarshaller<>), 
    TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetWrappedType(nativePtr)))
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