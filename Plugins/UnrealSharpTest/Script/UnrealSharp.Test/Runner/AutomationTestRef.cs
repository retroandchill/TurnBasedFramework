using System.Runtime.InteropServices;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp.Test.Interop;

namespace UnrealSharp.Test.Runner;

public readonly ref struct AutomationTestRef([ReadOnly] IntPtr nativeTest)
{
    public void EnqueueNativeTask(Task task)
    {
        var taskHandle = GCHandle.Alloc(task);
        AutomationTestExporter.CallEnqueueLatentCommand(nativeTest, GCHandle.ToIntPtr(taskHandle));
    }
}