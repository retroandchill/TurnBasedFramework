using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace UnrealSharp.Test.Discovery;

public class DynamicAssemblyBuilder : ITestAssemblyBuilder
{
    public ITest Build(Assembly assembly, IDictionary<string, object> options)
    {
        var testSuite = new TestAssembly(assembly, assembly.FullName!);
        var testTypes = assembly.GetTypes()
            .Where(type => type.GetCustomAttributes<TestFixtureAttribute>(true).Any() ||
                           type.GetMethods().Any(m => m.GetCustomAttributes<TestAttribute>(true).Any()));

        foreach (var type in testTypes)
        {
            var typeInfo = new TypeWrapper(type);
            var suite = new TestFixture(typeInfo);
            testSuite.Add(suite);

            var testMethods = type.GetMethods()
                .Where(m => m.GetCustomAttributes<TestAttribute>(true).Any());

            foreach (var method in testMethods)
            {
                var methodInfo = new MethodWrapper(method.DeclaringType!, method);
                var test = new TestMethod(methodInfo, suite);
                suite.Add(test);
            }
        }

        return testSuite;

    }

    public ITest Build(string assemblyName, IDictionary<string, object> options)
    {
        throw new NotSupportedException("Building from assembly name is not supported for dynamic assemblies");
    }
}