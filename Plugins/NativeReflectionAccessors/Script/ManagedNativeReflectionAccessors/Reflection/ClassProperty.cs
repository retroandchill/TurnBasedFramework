using System.Reflection;
using ManagedNativeReflectionAccessors.Interop;
using ManagedNativeReflectionAccessors.Utils;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Interop;

namespace ManagedNativeReflectionAccessors.Reflection;

public sealed class ClassProperty(IntPtr nativePtr) : GenericStaticMarshalledProperty(nativePtr,
    typeof(TSubclassOf<>), typeof(SubclassOfMarshaller<>), 
    TypeUtils.RetrieveManagedType(PropertyMetadataExporter.CallGetWrappedType(nativePtr)));