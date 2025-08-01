﻿using GameDataAccessTools.Core.Serialization.Native;
using UnrealSharp;
using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace GameDataAccessTools.Core.Interop;

[NativeCallbacks]
public static unsafe partial class FJsonObjectConverterExporter
{
    private static readonly delegate* unmanaged<IntPtr, ref NativeJsonValue, NativeBool> SerializeObjectToJson;
    private static readonly delegate* unmanaged<ref NativeJsonValue, IntPtr, ref FTextData, NativeBool> DeserializeJsonToObject;
}