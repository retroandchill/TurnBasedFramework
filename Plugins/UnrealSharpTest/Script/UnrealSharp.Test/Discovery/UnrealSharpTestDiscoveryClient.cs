using System.Collections.Immutable;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NUnit.VisualStudio.TestAdapter;
using UnrealSharp.Core;
using UnrealSharp.Test.Interop;

namespace UnrealSharp.Test.Discovery;

public static class UnrealSharpTestDiscoveryClient
{
    private static readonly List<ITestDiscoverer> _discoverers = [
        new NUnit3TestDiscoverer()
    ];
    
    public static IEnumerable<TestCase> DiscoverTests(IReadOnlyList<string> assemblyPaths)
    {
        var discoveryContext = new DiscoveryContext();
        var testCases = new UnrealSharpTestDiscoverySink();

        foreach (var discoverer in _discoverers)
        {
            try
            {
                discoverer.DiscoverTests(assemblyPaths, discoveryContext, UnrealSharpTestMessageLogger.Instance,
                    testCases);
            }
            catch (Exception e)
            {
                LogUnrealSharpTest.LogError($"Failed to discover tests: {e}");
            }
        }

        return testCases.TestCases;
    }

}