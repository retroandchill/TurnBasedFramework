﻿using GameDataAccessTools.Core.Serialization.Native;
using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace GameDataAccessTools.Core.Interop;

[NativeCallbacks]
public static unsafe partial class FJsonObjectExporter
{
    private static readonly delegate* unmanaged<ref NativeJsonValue, void> CreateJsonObject;
    private static readonly delegate* unmanaged<ref NativeJsonValue, string, ref NativeJsonValue, void> SetField;
    
    private static readonly delegate* unmanaged<ref NativeJsonValue, ref MapIterator, void> CreateJsonIterator;
    private static readonly delegate* unmanaged<ref MapIterator, NativeBool> AdvanceJsonIterator;
    private static readonly delegate* unmanaged<ref MapIterator, NativeBool> IsValidJsonIterator;
    private static readonly delegate* unmanaged<ref MapIterator, ref IntPtr, ref NativeJsonValue*, void> GetJsonIteratorValues;
}