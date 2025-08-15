using NUnit.Framework.Interfaces;
using Retro.ReadOnlyParams.Annotations;

namespace UnrealSharp.Test.Runner;

public class UnrealSharpTestListener([ReadOnly] IntPtr latentTestAction) : ITestListener
{
    
    public void TestStarted(ITest test)
    {
        // We don't need to do anything here
    }

    public void TestFinished(ITestResult result)
    {
        switch (result.ResultState.Status)
        {
            case TestStatus.Inconclusive:
                break;
            case TestStatus.Skipped:
                break;
            case TestStatus.Passed:
                break;
            case TestStatus.Warning:
                break;
            case TestStatus.Failed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void TestOutput(TestOutput output)
    {
        throw new NotImplementedException();
    }

    public void SendMessage(TestMessage message)
    {
        throw new NotImplementedException();
    }
}