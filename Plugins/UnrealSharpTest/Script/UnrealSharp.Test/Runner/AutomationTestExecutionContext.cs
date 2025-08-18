using System.Runtime.InteropServices;
using UnrealSharp.CoreUObject;
using UnrealSharp.Test.Interop;
using UnrealSharp.Test.Model;

namespace UnrealSharp.Test.Runner;

[StructLayout(LayoutKind.Sequential)]
public struct WeakAutomationTestReference
{
    private readonly IntPtr _ptr;
    private readonly IntPtr _referenceCounter;
}

public sealed class AutomationTestExecutionContext : ITestExecutionContext
{
    private WeakAutomationTestReference _test;
    
    public AutomationTestExecutionContext(ref WeakAutomationTestReference test)
    {
        TestPointerExporter.CallCopyWeakPtr(ref test, ref _test);
    }

    ~AutomationTestExecutionContext()
    {
        Dispose();
    }
    
    public void LogEvent(string message, EAutomationEventType type, EventLocation location)
    {
        unsafe
        {
            fixed (char* messagePtr = message)
            fixed (char* locationPtr = location.File)
            {
                AssertionExporter.CallRecordAssertion(ref _test, messagePtr, type, locationPtr, location.Line);
            }
        }
    }

    public void Dispose()
    {
        TestPointerExporter.CallReleaseWeakPtr(ref _test);
        GC.SuppressFinalize(this);
    }
}