using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;
using GameDataAccessTools.Core.DataRetrieval;
using GameDataAccessTools.Core.Serialization;
using JetBrains.Annotations;
using UnrealInject.Subsystems;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;
using UnrealSharp.Interop;

namespace GameDataAccessTools.Core.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct SerializationActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<
        IntPtr,
        UnmanagedArray*,
        void> GetSerializationActions { get; init; }

    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr, void> GetActionText { get; init; }

    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr, void> GetFileExtensionText { get; init; }

    [UsedImplicitly]
    public required delegate* unmanaged<
        IntPtr,
        IntPtr,
        IntPtr,
        NativeBool> SerializeToString { get; init; }

    [UsedImplicitly]
    public required delegate* unmanaged<
        IntPtr,
        IntPtr,
        IntPtr,
        IntPtr,
        IntPtr,
        NativeBool> DeserializeFromString { get; init; }

    public static SerializationActions Create()
    {
        return new SerializationActions
        {
            GetSerializationActions = &SerializationCallbacks.GetSerializationActions,
            GetActionText = &SerializationCallbacks.GetActionText,
            GetFileExtensionText = &SerializationCallbacks.GetFileExtensionText,
            SerializeToString = &SerializationCallbacks.SerializeToString,
            DeserializeFromString = &SerializationCallbacks.DeserializeFromString,
        };
    }
}

public static class SerializationCallbacks
{
    [UnmanagedCallersOnly]
    public static unsafe void GetSerializationActions(
        IntPtr nativeClass,
        UnmanagedArray* targetArray
    )
    {
        var classHandle = UClassExporter.CallGetDefaultFromInstance(nativeClass);

        if (classHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Invalid class handle.");
        }

        var obj = GCHandleUtilities.GetObjectFromHandlePtr<UObject>(classHandle);

        if (obj == null)
        {
            throw new InvalidOperationException("Invalid class object.");
        }

        var managedType = obj.GetType();
        var targetType = typeof(IGameDataEntrySerializer<>).MakeGenericType(managedType);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(targetType);

        var subsystem = UObject.GetEngineSubsystem<UDependencyInjectionEngineSubsystem>();

        if (
            subsystem.GetService(enumerableType) is not IEnumerable<IGameDataEntrySerializer> result
        )
        {
            throw new InvalidOperationException("No serialization actions found.");
        }

        foreach (var action in result.GroupBy(s => s.FormatTag).Select(g => g.Last()))
        {
            var handle = GCHandle.Alloc(action, GCHandleType.Normal);
            var handlePtr = GCHandle.ToIntPtr(handle);
            SerializationExporter.CallAddSerializationAction(ref *targetArray, handlePtr);
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

    [UnmanagedCallersOnly]
    public static void GetFileExtensionText(IntPtr actionHandle, IntPtr textOutput)
    {
        var handle = GCHandle.FromIntPtr(actionHandle);
        if (handle.Target is not IGameDataEntrySerializer action)
        {
            throw new InvalidOperationException("Invalid action.");
        }

        var text = action.FileExtensionText;
        StringMarshaller.ToNative(textOutput, 0, text);
    }

    [UnmanagedCallersOnly]
    public static NativeBool SerializeToString(
        IntPtr actionHandle,
        IntPtr nativeRepository,
        IntPtr stringOutput
    )
    {
        try
        {
            var dataRepositoryHandle = FCSManagerExporter.CallFindManagedObject(nativeRepository);
            var dataRepository = GCHandleUtilities.GetObjectFromHandlePtr<UGameDataRepository>(
                dataRepositoryHandle
            )!;
            var entryClass = dataRepository.GetEntryClass();
            var entryType = entryClass.DefaultObject.GetType();

            var handle = GCHandle.FromIntPtr(actionHandle);
            if (handle.Target is not IGameDataEntrySerializer)
            {
                throw new InvalidOperationException("Invalid action.");
            }

            var serializerMethod = typeof(SerializationCallbacks)
                .GetMethod(nameof(SerializeInternal), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entryType);

            var asString = (string)serializerMethod.Invoke(null, [handle.Target, dataRepository])!;
            StringMarshaller.ToNative(stringOutput, 0, asString);

            return NativeBool.True;
        }
        catch (Exception e)
        {
            StringMarshaller.ToNative(stringOutput, 0, $"{e.Message}\n{e.StackTrace}");
            return NativeBool.False;
        }
    }

    private static string SerializeInternal<TEntry>(
        IGameDataEntrySerializer<TEntry> action,
        IGameDataRepository<TEntry> repository
    )
        where TEntry : UObject, IGameDataEntry
    {
        return action.SerializeData(repository.AllEntries);
    }

    [UnmanagedCallersOnly]
    public static NativeBool DeserializeFromString(
        IntPtr actionHandle,
        IntPtr stringInput,
        IntPtr nativeRepository,
        IntPtr destinationCollection,
        IntPtr exceptionString
    )
    {
        try
        {
            var dataRepositoryHandle = FCSManagerExporter.CallFindManagedObject(nativeRepository);
            var dataRepository = GCHandleUtilities.GetObjectFromHandlePtr<UGameDataRepository>(
                dataRepositoryHandle
            )!;
            var entryClass = dataRepository.GetEntryClass();
            var entryType = entryClass.DefaultObject.GetType();

            var inputString = StringMarshaller.FromNative(stringInput, 0);

            var handle = GCHandle.FromIntPtr(actionHandle);
            if (handle.Target is not IGameDataEntrySerializer)
            {
                throw new InvalidOperationException("Invalid action.");
            }

            var serializerMethod = typeof(SerializationCallbacks)
                .GetMethod(
                    nameof(DeserializeInternal),
                    BindingFlags.NonPublic | BindingFlags.Static
                )!
                .MakeGenericMethod(entryType);

            serializerMethod.Invoke(
                null,
                [inputString, handle.Target, dataRepository, destinationCollection]
            );

            return NativeBool.True;
        }
        catch (Exception e)
        {
            var trueException =
                e is TargetInvocationException && e.InnerException is not null
                    ? e.InnerException
                    : e;
            StringMarshaller.ToNative(
                exceptionString,
                0,
                $"{trueException.Message}\n{trueException.StackTrace}"
            );
            return NativeBool.False;
        }
    }

    private static void DeserializeInternal<TEntry>(
        string inputString,
        IGameDataEntrySerializer<TEntry> action,
        IGameDataRepository<TEntry> repository,
        IntPtr destinationCollection
    )
        where TEntry : UObject, IGameDataEntry
    {
        if (repository is not UObject repositoryObject)
        {
            throw new InvalidOperationException("Invalid repository.");
        }

        var foundTags = new HashSet<FGameplayTag>();
        foreach (var entry in action.DeserializeData(inputString, repositoryObject))
        {
            var id = entry.Id;
            if (!id.IsValid)
            {
                throw new InvalidOperationException(
                    "One or more invalid entry IDs were found. Please ensure that all entries have a valid ID."
                );
            }

            if (!foundTags.Add(id))
            {
                throw new InvalidOperationException(
                    $"Duplicate entry ID '{id}' found. Please ensure that all entries have unique IDs."
                );
            }

            SerializationExporter.CallAddEntryToCollection(
                destinationCollection,
                entry.NativeObject
            );
        }
    }
}
