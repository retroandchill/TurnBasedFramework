using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace UnrealSharp.Test.Runner;

public class UnrealTestRunContext(string testRunDirectory, string solutionDirectory) : IRunContext
{
    public IRunSettings? RunSettings => null;

    public bool KeepAlive => true;
    public bool InIsolation => false;
    public bool IsDataCollectionEnabled => false;
    public bool IsBeingDebugged => false;
    
    public string TestRunDirectory { get; } = testRunDirectory;
    public string SolutionDirectory { get; } = solutionDirectory;

    public ITestCaseFilterExpression? GetTestCaseFilter(IEnumerable<string>? supportedProperties, Func<string, TestProperty?> propertyProvider)
    {
        return null;
    }
}