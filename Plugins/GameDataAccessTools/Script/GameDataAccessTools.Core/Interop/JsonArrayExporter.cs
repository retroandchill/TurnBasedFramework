using GameDataAccessTools.Core.Serialization.Native;
using UnrealSharp.Binds;
using UnrealSharp.Core;

namespace GameDataAccessTools.Core.Interop;

[NativeCallbacks]
public static unsafe partial class JsonArrayExporter
{
    private static readonly delegate* unmanaged<ref UnmanagedArray, int, ref NativeJsonValue*, void> GetAtIndex;
    private static readonly delegate* unmanaged<ref UnmanagedArray, ref NativeJsonValue, void> AddToArray;
}