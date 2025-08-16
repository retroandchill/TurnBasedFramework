using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using NUnit.VisualStudio.TestAdapter;
using UnrealSharp.Engine;

namespace UnrealSharp.Test.Runner;

public static class UnrealSharpTestExecutor
{
    public static async Task RunTestInProcess(TestCase testCase)
    {
        // Get the test executor from the ExecutorUri
        var executorType = GetExecutorType(testCase.ExecutorUri);
        var executor = (ITestExecutor)Activator.CreateInstance(executorType)!;
    
        // Create run context
        var runContext = new UnrealTestRunContext(
            Path.GetDirectoryName(testCase.Source)!,
            SystemLibrary.ProjectDirectory
        );

        // Create the framework handle (to receive results)
        var frameworkHandle = new InProcessTestFrameworkHandle();
    
        // Run the test
        executor.RunTests([testCase], runContext, frameworkHandle);
    
        // Wait for completion
        await frameworkHandle.CompletionTask;
    }
    
    private static Type GetExecutorType(Uri executorUri)
    {
        return typeof(NUnit3TestExecutor);
    }
}