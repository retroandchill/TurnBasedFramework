using ManagedGameDataAccessToolsEditor.Serialization;
using UnrealSharp;
using UnrealSharp.Binds;

namespace ManagedGameDataAccessToolsEditor.Interop;

[NativeCallbacks]
public static unsafe partial class SerializationExporter
{
    private static readonly delegate* unmanaged<ref SerializationActions, void> AssignSerializationActions;
    private static readonly delegate* unmanaged<IntPtr, IntPtr, void> OnSerializationAction;
}