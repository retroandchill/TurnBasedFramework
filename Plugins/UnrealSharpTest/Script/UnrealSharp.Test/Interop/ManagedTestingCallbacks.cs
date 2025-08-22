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
    public required delegate* unmanaged<IntPtr, UnmanagedArray*, UnmanagedArray*, void> GetTests { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr, FName, NativeBool> RunTest { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, NativeBool> CheckTaskComplete { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<void> ClearTestClassInstances { get; init; }

    public static ManagedTestingActions Create()
    {
        return new ManagedTestingActions
        {
            CollectTestCases = &ManagedTestingCallbacks.CollectTestCases,
            GetTests = &ManagedTestingCallbacks.GetTests,
            RunTest = &ManagedTestingCallbacks.RunTest,
            CheckTaskComplete = &ManagedTestingCallbacks.CheckTaskComplete,
            ClearTestClassInstances = &ManagedTestingCallbacks.ClearTestClassInstances
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
    public static void GetTests(IntPtr managedTestCasePtr, UnmanagedArray* beautifiedNames, UnmanagedArray* testParameters)
    {
        var managedTestCase = GCHandleUtilities.GetObjectFromHandlePtr<UnrealTestMethod>(managedTestCasePtr);
        if (managedTestCase is null)
        {
            LogUnrealSharpTest.LogError("Failed to get tests");
            return;
        }

        if (managedTestCase.TestCases.Count == 0)
        {
            fixed (char* testNamePtr = managedTestCase.FullyQualifiedName)
            {
                AutomationTestExporter.CallAddTestCase(FName.None, testNamePtr, ref *beautifiedNames, ref *testParameters);
            }
        }

        var testIndex = 1;
        var digitsCount = (int) Math.Log10(managedTestCase.TestCases.Count) + 1;
        foreach (var (testCommand, testCase) in managedTestCase.TestCases)
        {
            var index = testIndex.ToString().PadLeft(digitsCount, '0');
            fixed (char* displayNamePtr = $"#{index}: {testCase.GetDisplayName()}")
            {
                AutomationTestExporter.CallAddTestCase(testCommand, displayNamePtr, ref *beautifiedNames, ref *testParameters);;
            }
            testIndex++;
        }
    }
    
    [UnmanagedCallersOnly]
    public static NativeBool RunTest(IntPtr nativeTest, IntPtr managedTestCasePtr, FName testName)
    {
        var testCase = GCHandleUtilities.GetObjectFromHandlePtr<UnrealTestMethod>(managedTestCasePtr);
        if (testCase is null)
        {
            return NativeBool.False;
        }
        
        return UnrealSharpTestExecutor.RunTestInProcess(new AutomationTestRef(nativeTest), testCase, testName).ToNativeBool();
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
    
    [UnmanagedCallersOnly]
    public static void ClearTestClassInstances()
    {
        UnrealSharpTestExecutor.ClearTestClassInstances();
    }
}