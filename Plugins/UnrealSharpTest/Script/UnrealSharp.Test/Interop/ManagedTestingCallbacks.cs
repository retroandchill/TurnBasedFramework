using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using JetBrains.Annotations;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.Test.Discovery;
using UnrealSharp.Test.Mappers;
using UnrealSharp.Test.Runner;
using UnrealSharp.UnrealSharpTest;

namespace UnrealSharp.Test.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ManagedTestingActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, int, UnmanagedArray*, void> CollectTestCases { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr> StartTest { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, NativeBool> CheckTaskComplete { get; init; }

    public static ManagedTestingActions Create()
    {
        return new ManagedTestingActions
        {
            CollectTestCases = &ManagedTestingCallbacks.CollectTestCases,
            StartTest = &ManagedTestingCallbacks.StartTest,
            CheckTaskComplete = &ManagedTestingCallbacks.CheckTaskComplete
        };
    }
}

public static unsafe class ManagedTestingCallbacks
{
    [UnmanagedCallersOnly]
    public static void CollectTestCases(IntPtr assemblyNamesPtr, int assemblyNamesLength, UnmanagedArray* outputArrayPtr)
    {
        var testCases = Enumerable.Range(0, assemblyNamesLength)
            .Select(i => StringMarshaller.FromNative(assemblyNamesPtr, i))
            .ToArray();
        
        var nativeStruct = stackalloc byte[FManagedTestCaseMarshaller.GetNativeDataSize()];
        foreach (var unrealStruct in UnrealSharpTestDiscoveryClient.DiscoverTests(testCases)
                     .Select(x => x.ToManagedTestCase()))
        {
            unrealStruct.ToNative((IntPtr)nativeStruct);
            ManagedTestingExporter.CallAddTestCase(ref *outputArrayPtr, (IntPtr)nativeStruct);
        }
    }

    [UnmanagedCallersOnly]
    public static IntPtr StartTest(IntPtr nativeStruct)
    {
        var managedStruct = FManagedTestCase.FromNative(nativeStruct);
        var testCase = managedStruct.ToTestCase();
        var testTask = UnrealSharpTestExecutor.RunTestInProcess(testCase);
        var taskHandle = GCHandle.Alloc(testTask);
        return GCHandle.ToIntPtr(taskHandle);
    }
    
    [UnmanagedCallersOnly]
    public static NativeBool CheckTaskComplete(IntPtr taskHandlePtr)
    {
        var task = GCHandleUtilities.GetObjectFromHandlePtr<Task>(taskHandlePtr);
        ArgumentNullException.ThrowIfNull(task);
        if (!task.IsCompleted) return NativeBool.False;

        if (task.IsFaulted)
        {
            LogUnrealSharpTest.LogError(task.Exception!.ToString());
        }
        task.Dispose();
        return NativeBool.True;

    }
}