﻿using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace GameDataAccessTools.Core.Interop;

[NativeCallbacks]
public static unsafe partial class SerializationExporter
{
    private static readonly delegate* unmanaged<ref SerializationActions, void> AssignSerializationActions;
    private static readonly delegate* unmanaged<ref UnmanagedArray, IntPtr, void> AddSerializationAction;
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> AddEntryToCollection;
}