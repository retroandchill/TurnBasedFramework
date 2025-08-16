using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using JetBrains.Annotations;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.Test.Discovery;
using UnrealSharp.Test.Mappers;
using UnrealSharp.UnrealSharpTest;

namespace UnrealSharp.Test.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ManagedTestingActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, int, UnmanagedArray*, void> CollectTestCases { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<FName, IntPtr, IntPtr> StartTest { get; init; }
    
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
            FManagedTestCaseMarshaller.ToNative((IntPtr)nativeStruct, 0, unrealStruct);
            ManagedTestingExporter.CallAddTestCase(ref *outputArrayPtr, (IntPtr)nativeStruct);
        }
    }

    [UnmanagedCallersOnly]
    public static IntPtr StartTest(FName assemblyName, IntPtr testNamePtr)
    {
        if (!FUnrealSharpTestModule.Instance.TryGetRunner(assemblyName, out var runner)) return IntPtr.Zero;
        
        var testName = StringMarshaller.FromNative(testNamePtr, 0);
        try
        {
            var testTask = runner.RunTest(testName);
            var taskHandle = GCHandle.Alloc(testTask);
            return GCHandle.ToIntPtr(taskHandle);
        }
        catch (Exception e)
        {
            LogUnrealSharpTest.LogError($"Failed to run test {testName}: {e}");
            return IntPtr.Zero;
        } 
    }
    
    [UnmanagedCallersOnly]
    public static NativeBool CheckTaskComplete(IntPtr taskHandlePtr)
    {
        var task = GCHandleUtilities.GetObjectFromHandlePtr<Task>(taskHandlePtr);
        ArgumentNullException.ThrowIfNull(task);
        if (!task.IsCompleted) return NativeBool.False;
        
        task.Dispose();
        return NativeBool.True;

    }
}