using System.Reflection;
using NUnit.Engine;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TestFilter = NUnit.Framework.Internal.TestFilter;

namespace UnrealSharp.Test.Runner.NUnit;

public class NUnitTestRunner : IUnrealSharpTestRunner
{
    private readonly NUnitTestAssemblyRunner _runner = new(new DefaultTestAssemblyBuilder());
    private readonly Dictionary<string, ITest> _tests = new();

    public NUnitTestRunner(FName assemblyName, Assembly assembly)
    {
        // Load the test assembly
        _runner.Load(assembly, new Dictionary<string, object>());

        if (_runner.LoadedTest is not TestSuite suite) return;
            
        foreach (var test in GetTestCasesRecursive(suite))
        {
            _tests.Add(test.FullName, test);
        }
    }

    public IEnumerable<string> TestCases => _tests.Keys;

    public Task RunTest(string testCase)
    {
        if (!_tests.TryGetValue(testCase, out var test))
        {
            LogUnrealSharpTest.LogError($"Test {testCase} not found");
            return Task.CompletedTask;
        }

        var testFilterBuilder = new TestFilterBuilder();
        testFilterBuilder.AddTest(test.FullName);
        var testFilter = testFilterBuilder.GetFilter();
        
        var listener = new UnrealSharpTestListener();
        _runner.RunAsync(listener, TestFilter.FromXml(testFilter.Text));
        return listener.Completion;
    }
    
    private static IEnumerable<ITest> GetTestCasesRecursive(TestSuite suite)
    {
        foreach (var test in suite.Tests)
        {
            if (test is TestSuite nestedSuite)
            {
                foreach (var nestedTest in GetTestCasesRecursive(nestedSuite))
                {
                    yield return nestedTest;
                }
            }
            else if (!test.IsSuite)
            {
                yield return test;
            }
        }
    }
}