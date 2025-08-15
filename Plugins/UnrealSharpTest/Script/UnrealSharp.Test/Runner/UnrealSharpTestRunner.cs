using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Engine;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;

namespace UnrealSharp.Test.Runner;

public class UnrealSharpTestRunner
{
    private readonly NUnitTestAssemblyRunner _runner;
    private readonly Assembly _assembly;
    
    public UnrealSharpTestRunner(NUnitTestAssemblyRunner runner, Assembly assembly)
    {
        _runner = runner;
        _assembly = assembly;
    }


    public TestResult RunTest(ITest test)
    {
        var testFilter = new TestFilter(test.Id);

    }
}