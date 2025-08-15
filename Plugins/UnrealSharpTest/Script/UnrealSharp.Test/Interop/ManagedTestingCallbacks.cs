using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.Plugins;
using UnrealSharp.Test.Discovery;

namespace UnrealSharp.Test.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ManagedTestingActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, void> GetManagedTests { get; init; }
    
    [UsedImplicitly]
    public required delegate* unmanaged<IntPtr, IntPtr, void> GetFullyQualifiedName { get; init; }
    
    public static ManagedTestingActions Create()
    {
        return new ManagedTestingActions
        {
            GetManagedTests = &ManagedTestingCallbacks.GetManagedTests,
            GetFullyQualifiedName = &ManagedTestingCallbacks.GetFullQualifiedName
        };
    }
}

public static class ManagedTestingCallbacks
{
    [UnmanagedCallersOnly]
    public static void GetManagedTests(IntPtr outMap)
    {
        foreach (var testCase in FUnrealSharpTest.Instance.Discoverer.DiscoverTests(PluginLoader.LoadedPlugins
                     .Select(x => x.LoadContext)
                     .Where(x => x is not null)!))
        {
            var testCaseHandle = GCHandle.Alloc(testCase);
            var testCasePtr = GCHandle.ToIntPtr(testCaseHandle);
            ManagedTestingExporter.CallAddTest(outMap, testCasePtr);
        }
    }

    [UnmanagedCallersOnly]
    public static void GetFullQualifiedName(IntPtr managedPtr, IntPtr stringPtr)
    {
        var testCaseHandle = GCHandle.FromIntPtr(managedPtr);
        var testCase = (UnrealSharpTestCase) testCaseHandle.Target!;
        StringMarshaller.ToNative(stringPtr, 0, testCase.FullName);
    }
}