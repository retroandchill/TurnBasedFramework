using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace UnrealSharp.Test.Discovery;

public class UnrealSharpTestDiscoverySink : ITestCaseDiscoverySink
{
    private readonly List<TestCase> _testCases = [];
    public IEnumerable<TestCase> TestCases => _testCases;
    
    public void SendTestCase(TestCase discoveredTest)
    {
        _testCases.Add(discoveredTest);
    }
}