using System.Reflection;
using NUnit.Framework.Interfaces;

namespace UnrealSharp.Test.Discovery;

public class UnrealSharpTestCase(ITest test, Assembly assembly)
{
    public string FullName { get; } = test.FullName;
    public string DisplayName { get; } = test.Name;
    public Assembly TestAssembly { get; } = assembly;
    public ITest NUnitTest { get; } = test;
}