using NUnit.Framework.Internal;
using UnrealSharp.Attributes;
using UnrealSharp.FunctionalTesting;
using UnrealSharp.Test.Runner;

namespace UnrealSharp.Test.Functional;

[UClass]
public class AAsyncFunctionalTest : AFunctionalTest
{
    protected sealed override void StartTest()
    {
        _ = RunTestInternal();
    }

    private async Task RunTestInternal()
    {
        using var nunitContext = new TestExecutionContext.IsolatedContext();
        var testResult = TestExecutionContext.CurrentContext.CurrentResult;
        
        try
        {
            await RunTest();
        }
        catch (Exception e)
        {
            testResult.RecordException(e);
        }
        
        UnrealSharpTestExecutor.LogTestResult(testResult);
    }

    protected virtual ValueTask RunTest()
    {
        return ValueTask.CompletedTask;
    }
}