using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnrealSharp.Core;
using UnrealSharp.Test.Discovery;
using UnrealSharp.Test.Mappers;
using UnrealSharp.Test.Model;
using UnrealSharp.Test.Runner;
using UnrealSharp.UnrealSharpTest;

namespace UnrealSharp.Test.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ManagedTestingActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, int, UnmanagedArray*, void> CollectTestCases { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<WeakAutomationTestReference*, IntPtr, IntPtr> StartTest { get; init; }
    
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
        var testCases = new ReadOnlySpan<FName>((FName*) assemblyNamesPtr, assemblyNamesLength);
        
        var nativeStruct = stackalloc byte[FManagedTestCaseMarshaller.GetNativeDataSize()];
        foreach (var testCase in UnrealSharpTestDiscoveryClient.DiscoverTests(testCases))
        {
            var unrealStruct = testCase.ToManagedTestCase();
            unrealStruct.ToNative((IntPtr)nativeStruct);
            
            var testCaseHandle = GCHandle.Alloc(testCase);
            var testCaseHandlePtr = GCHandle.ToIntPtr(testCaseHandle);
            
            ManagedTestingExporter.CallAddTestCase(ref *outputArrayPtr, (IntPtr)nativeStruct, testCaseHandlePtr);
        }
    }

    [UnmanagedCallersOnly]
    public static IntPtr StartTest(WeakAutomationTestReference* nativeTest, IntPtr managedTestCasePtr)
    {
        var testCase = GCHandleUtilities.GetObjectFromHandlePtr<UnrealTestCase>(managedTestCasePtr);
        if (testCase is null)
        {
            return IntPtr.Zero;
        }
        
        var testTask = UnrealSharpTestExecutor.RunTestInProcess(ref *nativeTest, testCase);
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