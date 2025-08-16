using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;

namespace UnrealSharp.Test.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ManagedTestingActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<FName, IntPtr, UnmanagedArray*, void> LoadAssemblyTests { get; init; }

    [UsedImplicitly]
    public required delegate* unmanaged<FName, void> UnloadAssemblyTests { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<FName, IntPtr, IntPtr> StartTest { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, NativeBool> CheckTaskComplete { get; init; }

    public static ManagedTestingActions Create()
    {
        return new ManagedTestingActions
        {
            LoadAssemblyTests = &ManagedTestingCallbacks.LoadAssemblyTests,
            UnloadAssemblyTests = &ManagedTestingCallbacks.UnloadAssemblyTests,
            StartTest = &ManagedTestingCallbacks.StartTest,
            CheckTaskComplete = &ManagedTestingCallbacks.CheckTaskComplete
        };
    }
}

public static unsafe class ManagedTestingCallbacks
{
    [UnmanagedCallersOnly]
    public static void LoadAssemblyTests(FName assemblyName, IntPtr assemblyPtr, UnmanagedArray* outArray)
    {
        var assembly = GCHandleUtilities.GetObjectFromHandlePtr<Assembly>(assemblyPtr);
        ArgumentNullException.ThrowIfNull(assembly);
        var runner = FUnrealSharpTestModule.Instance.RegisterRunner(assemblyName, assembly);
        if (runner is null) return;
        
        foreach (var fullName in runner.TestCases)
        {
            fixed (char* nativeString = fullName)
            {
                // The native side will move the unmanaged array, removing the need to clear the string
                ManagedTestingExporter.CallAddTest(ref *outArray, nativeString);
            }
        }
    }

    [UnmanagedCallersOnly]
    public static void UnloadAssemblyTests(FName assemblyName)
    {
        FUnrealSharpTestModule.Instance.UnregisterRunner(assemblyName);
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