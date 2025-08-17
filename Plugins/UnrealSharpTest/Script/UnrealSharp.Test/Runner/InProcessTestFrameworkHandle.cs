using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace UnrealSharp.Test.Runner;

public class InProcessTestFrameworkHandle : IFrameworkHandle
{
    private readonly TaskCompletionSource _completionSource = new();
    public Task CompletionTask => _completionSource.Task;

    public bool EnableShutdownAfterTestRun { get; set; }
    
    public void SendMessage(TestMessageLevel testMessageLevel, string message)
    {
        switch (testMessageLevel)
        {
            case TestMessageLevel.Informational:
                LogUnrealSharpTest.Log(message);
                break;
            case TestMessageLevel.Warning:
                LogUnrealSharpTest.LogWarning(message);
                break;
            case TestMessageLevel.Error:
                LogUnrealSharpTest.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testMessageLevel), testMessageLevel, null);
        }
    }

    public void RecordResult(TestResult testResult)
    {
        if (testResult.Outcome == TestOutcome.Failed)
        {
            LogUnrealSharpTest.LogError($"{testResult.ErrorMessage}\n{testResult.ErrorStackTrace}");
        }
        
        _completionSource.SetResult();
    }

    public void RecordStart(TestCase testCase)
    {
        LogUnrealSharpTest.Log($"Starting test: {testCase.FullyQualifiedName}");
    }

    public void RecordEnd(TestCase testCase, TestOutcome outcome)
    {
        LogUnrealSharpTest.Log($"Finished test: {testCase.FullyQualifiedName}");
    }

    public void RecordAttachments(IList<AttachmentSet> attachmentSets)
    {
        throw new InvalidOperationException("Cannot run tests in-process");
    }

    public int LaunchProcessWithDebuggerAttached(string filePath, string? workingDirectory, string? arguments,
                                                 IDictionary<string, string?>? environmentVariables)
    {
        throw new InvalidOperationException("Cannot run tests in-process");
    }
}