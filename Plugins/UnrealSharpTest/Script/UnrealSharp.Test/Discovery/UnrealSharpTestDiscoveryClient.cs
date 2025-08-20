using System.Reflection;
using System.Reflection.Metadata;
using NUnit.Framework;
using UnrealSharp.Core;
using UnrealSharp.Test.Attributes;
using UnrealSharp.Test.Interop;
using UnrealSharp.Test.Model;
using UnrealSharp.Test.Utils;

namespace UnrealSharp.Test.Discovery;

public static class UnrealSharpTestDiscoveryClient
{
    public static IEnumerable<UnrealTestCase> DiscoverTests(ReadOnlySpan<FName> assemblyPaths)
    {
        var testCases = new List<UnrealTestCase>();
        foreach (var assemblyName in assemblyPaths)
        {
            var assemblyPtr = ManagedTestingExporter.CallFindUserAssembly(assemblyName);
            var assembly = GCHandleUtilities.GetObjectFromHandlePtr<Assembly>(assemblyPtr);
            if (assembly is null)
            {
                LogUnrealSharpTest.LogError($"Assembly {assemblyName} not found");
                continue;
            }

            if (assembly.GetCustomAttribute<TestAssemblyAttribute>() is null)
            {
                continue;
            }
            
            testCases.AddRange(assembly.GetTypes()
                .Where(IsTestClass)
                .SelectMany(t => DiscoverTests(assemblyName, t)));
        }
        
        return testCases;
    }

    private static bool IsTestClass(Type type)
    {
        if (!type.IsClass)
        {
            return false;
        }
        
        return type.GetCustomAttribute<TestFixtureAttribute>() is not null || type.GetMethods().Any(IsTestMethod);
    }

    private static bool IsTestMethod(MethodInfo method)
    {
        return method.GetCustomAttribute<TestAttribute>() is not null || method.GetCustomAttributes<TestCaseAttribute>().Any();
    }

    private static IEnumerable<UnrealTestCase> DiscoverTests(FName assemblyName, Type testClass)
    {
        var category = testClass.GetCustomAttribute<TestFixtureAttribute>()?.Category;
        var prefix = category ?? testClass.FullName ?? testClass.Name;
        
        MethodInfo? setupMethod = null;
        MethodInfo? teardownMethod = null;
        foreach (var method in testClass.GetMethods())
        {
            if (method.GetCustomAttribute<SetUpAttribute>() is not null)
            {
                if (setupMethod is not null) throw new InvalidOperationException("Only one setup method is allowed");
                setupMethod = method;
            }

            if (method.GetCustomAttribute<TearDownAttribute>() is null) continue;
            
            if (teardownMethod is not null) throw new InvalidOperationException("Only one teardown method is allowed");
            teardownMethod = method;
        }

        return testClass.GetMethods()
            .Where(IsTestMethod)
            .SelectMany(m => GetTestCases(assemblyName, prefix, m, setupMethod, teardownMethod));
    }

    private static IEnumerable<UnrealTestCase> GetTestCases(FName assemblyName, string prefix, MethodInfo method, MethodInfo? setupMethod, MethodInfo? teardownMethod)
    {
        var displayName = method.GetCustomAttribute<TestAttribute>()?.Description;
        var testName = displayName is not null ? $"{prefix}.{displayName}" : $"{prefix}.{method.Name}";

        var sequencePoint = method.GetFirstSequencePoint();
        
        var testCases = method.GetCustomAttributes<TestCaseAttribute>()
            .ToArray();

        if (testCases.Length == 0)
        {
            return [CreateTestCase(assemblyName, testName, method, setupMethod, teardownMethod, sequencePoint)];
        }

        return testCases
            .DistinctBy(GetArgumentsName)
            .Select(t =>
                CreateTestCase(assemblyName, $"{testName}.{GetArgumentsName(t)}", 
                    method, setupMethod, teardownMethod, sequencePoint, t.Arguments));
    }

    private static UnrealTestCase CreateTestCase(FName assemblyName, string testName, MethodInfo method,
                                                 MethodInfo? setupMethod, MethodInfo? teardownMethod, 
                                                 SequencePoint? sequencePoint, params object?[] arguments)
    {
        return new UnrealTestCase(assemblyName, testName, setupMethod, teardownMethod, method)
        {
            Arguments = ArgumentConverter.ConvertProvidedArguments(method, arguments),
            CodeFilePath = sequencePoint?.Document.ToString(),
            LineNumber = sequencePoint?.StartLine ?? 0,
        };
    }

    private static string GetArgumentsName(TestCaseAttribute testCase)
    {
        return testCase.TestName ?? $"({string.Join(";", testCase.Arguments.Select(a => a?.ToString() ?? "null"))})";
    }

}