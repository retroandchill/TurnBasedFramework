using System.Reflection;

namespace UnrealSharp.Test.Runner.NUnit;

public class NUnitTestRunnerFactory : IUnrealSharpTestRunnerFactory
{
    public bool IsTestAssembly(Assembly assembly)
    {
        return assembly.GetReferencedAssemblies().Any(x => x.Name == "nunit.framework");
    }

    public IUnrealSharpTestRunner CreateRunner(FName name, Assembly assembly)
    {
        return new NUnitTestRunner(name, assembly);
    }
}