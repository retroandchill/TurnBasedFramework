using System.Reflection;

namespace UnrealSharp.Test.Runner;

public interface IUnrealSharpTestRunnerFactory
{
    bool IsTestAssembly(Assembly assembly);
    IUnrealSharpTestRunner CreateRunner(FName name, Assembly assembly);
}