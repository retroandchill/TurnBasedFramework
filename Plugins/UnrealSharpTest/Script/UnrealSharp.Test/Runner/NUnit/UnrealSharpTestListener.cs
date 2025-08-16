using NUnit.Framework.Interfaces;

namespace UnrealSharp.Test.Runner.NUnit;

public class UnrealSharpTestListener : ITestListener
{
    private readonly TaskCompletionSource _completionSource = new();
    
    public Task Completion => _completionSource.Task;
    
    public void TestStarted(ITest test)
    {
        // Nothing needed on start
    }

    public void TestFinished(ITestResult result)
    {
        switch (result.ResultState.Status)
        {
            case TestStatus.Inconclusive:
            case TestStatus.Skipped:
            case TestStatus.Passed:
                break;
            case TestStatus.Warning:
                LogUnrealSharpTest.LogWarning(result.Message);
                break;
            case TestStatus.Failed:
                LogUnrealSharpTest.LogError(result.Message);
                break;
            default:
                throw new InvalidOperationException();
        }
        _completionSource.SetResult();
    }

    public void TestOutput(TestOutput output)
    {
        LogUnrealSharpTest.Log(output.Text);
    }

    public void SendMessage(TestMessage message)
    {
        LogUnrealSharpTest.Log(message.Message);
    }
}