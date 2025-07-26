using ManagedGameDataAccessToolsEditor.Serialization;
using ManagedGameDataAccessToolsEditor.Serialization.Native;
using UnrealSharp;
using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace ManagedGameDataAccessToolsEditor.Interop;

[NativeCallbacks]
public static unsafe partial class SerializationExporter
{
    private static readonly delegate* unmanaged<ref SerializationActions, void> AssignSerializationActions;
    private static readonly delegate* unmanaged<ref UnmanagedArray, IntPtr, void> AddSerializationAction;
}