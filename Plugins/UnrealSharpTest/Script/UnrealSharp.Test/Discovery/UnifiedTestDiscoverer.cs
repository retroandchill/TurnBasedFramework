using System.Reflection;
using System.Runtime.Loader;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnrealSharp.Test.Runner;

namespace UnrealSharp.Test.Discovery;

public class UnifiedTestDiscoverer
{
    private readonly Dictionary<Assembly, NUnitTestAssemblyRunner> _runners = new();

    
    public IEnumerable<UnrealSharpTestCase> DiscoverTests(IEnumerable<AssemblyLoadContext> loadContexts)
    {
        var testCases = new List<UnrealSharpTestCase>();
        
        foreach (var assembly in loadContexts.SelectMany(x => x.Assemblies))
        {
            // Skip assemblies that don't contain tests
            if (assembly.GetReferencedAssemblies().All(x => x.Name?.Contains("nunit.framework", StringComparison.CurrentCultureIgnoreCase) != true))
            {
                continue;
            }

            try
            {
                // Load the test assembly
                var runner = new NUnitTestAssemblyRunner(new DynamicAssemblyBuilder());
                runner.Load(assembly, new Dictionary<string, object>());
                _runners[assembly] = runner;
                
                if (runner.LoadedTest is TestSuite suite)
                {
                    testCases.AddRange(GetTestCasesRecursive(suite).Select(test => new UnrealSharpTestCase(test, assembly)));
                }



            }
            catch (Exception ex)
            {
                // Log any errors but continue processing other assemblies
                LogUnrealSharpTest.LogError($"Error discovering tests in assembly {assembly.FullName}: {ex.Message}");
            }
        }

        return testCases;

    }
    
    private IEnumerable<ITest> GetTestCasesRecursive(TestSuite suite)
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

    public void RunTest(UnrealSharpTestCase testCase, ITestListener listener)
    {
        if (_runners.TryGetValue(testCase.TestAssembly, out var runner))
        {
            runner.RunAsync(listener, TestFilter.Empty);
        }
    }

}