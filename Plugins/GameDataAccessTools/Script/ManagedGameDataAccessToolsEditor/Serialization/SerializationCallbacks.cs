using System.Collections;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using ManagedGameDataAccessToolsEditor.Interop;
using UnrealInject.Subsystems;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.Interop;

namespace ManagedGameDataAccessToolsEditor.Serialization;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct SerializationActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr, void> ForEachSerializationAction { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr, void> GetActionText { get; init; }
    
    public static SerializationActions Create()
    {
        return new SerializationActions
        {
            ForEachSerializationAction = &SerializationCallbacks.ForEachSerializationAction,
            GetActionText = &SerializationCallbacks.GetActionText
        };
    }
}

public static class SerializationCallbacks
{
    [UnmanagedCallersOnly]
    public static void ForEachSerializationAction(IntPtr nativeClass, IntPtr delegatePointer)
    {
        var classHandle = UClassExporter.CallGetDefaultFromInstance(nativeClass);

        if (classHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Invalid class handle.");
        }

        var obj = GCHandleUtilities.GetObjectFromHandlePtr<UGameDataEntry>(classHandle);
        
        if (obj == null)
        {
            throw new InvalidOperationException("Invalid class object.");
        }

        var managedType = obj.GetType();
        var targetType = typeof(IGameDataEntrySerializer<>).MakeGenericType(managedType);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(targetType);
        
        var subsystem = UObject.GetEngineSubsystem<UDependencyInjectionEngineSubsystem>();

        if (subsystem.GetService(enumerableType) is not IEnumerable<IGameDataEntrySerializer> result)
        {
            throw new InvalidOperationException("No serialization actions found.");
        }

        foreach (var action in result)
        {
            var handle = GCHandle.Alloc(action, GCHandleType.Normal);
            var handlePtr = GCHandle.ToIntPtr(handle);
            SerializationExporter.CallOnSerializationAction(delegatePointer, handlePtr);
        }
    }

    [UnmanagedCallersOnly]
    public static void GetActionText(IntPtr actionHandle, IntPtr textOutput)
    {
        var handle = GCHandle.FromIntPtr(actionHandle);
        if (handle.Target is not IGameDataEntrySerializer action)
        {
            throw new InvalidOperationException("Invalid action.");
        }

        var text = action.FormatName;
        TextMarshaller.ToNative(textOutput, 0, text);
    }
}