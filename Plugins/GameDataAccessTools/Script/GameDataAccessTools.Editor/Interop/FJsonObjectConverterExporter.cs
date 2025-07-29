using ManagedGameDataAccessToolsEditor.Serialization.Native;
using UnrealSharp;
using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace ManagedGameDataAccessToolsEditor.Interop;

[NativeCallbacks]
public static unsafe partial class FJsonObjectConverterExporter
{
    private static readonly delegate* unmanaged<IntPtr, ref NativeJsonValue, NativeBool> SerializeObjectToJson;
    private static readonly delegate* unmanaged<ref NativeJsonValue, IntPtr, ref FTextData, NativeBool> DeserializeJsonToObject;
}