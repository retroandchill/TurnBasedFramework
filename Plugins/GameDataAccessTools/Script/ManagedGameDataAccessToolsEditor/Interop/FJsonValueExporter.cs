using ManagedGameDataAccessToolsEditor.Serialization.Native;
using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace ManagedGameDataAccessToolsEditor.Interop;

[NativeCallbacks]
public static unsafe partial class FJsonValueExporter
{
    private static readonly delegate* unmanaged<ref NativeJsonValue, void> CreateJsonNull;
    private static readonly delegate* unmanaged<ref NativeJsonValue, NativeBool, void> CreateJsonBool;
    private static readonly delegate* unmanaged<ref NativeJsonValue, double, void> CreateJsonNumber;
    private static readonly delegate* unmanaged<ref NativeJsonValue, string, void> CreateJsonString;
    private static readonly delegate* unmanaged<ref NativeJsonValue, ref UnmanagedArray, void> CreateJsonArray;
    private static readonly delegate* unmanaged<ref NativeJsonValue, ref NativeJsonValue, void> CreateJsonObject;
    
    private static readonly delegate* unmanaged<ref NativeJsonValue, EJson> GetJsonType;
    private static readonly delegate* unmanaged<ref NativeJsonValue, void> DestroyJsonValue;
    
    private static readonly delegate* unmanaged<ref NativeJsonValue, NativeBool> GetJsonBool;
    private static readonly delegate* unmanaged<ref NativeJsonValue, double> GetJsonNumber;
    private static readonly delegate* unmanaged<ref NativeJsonValue, char*> GetJsonString;
    private static readonly delegate* unmanaged<ref NativeJsonValue, ref UnmanagedArray*, void> GetJsonArray;
    private static readonly delegate* unmanaged<ref NativeJsonValue, ref NativeJsonValue*, void> GetJsonObject;
}