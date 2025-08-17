using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using UnrealSharp.Core;
using UnrealSharp.Test.Attributes;
using UnrealSharp.Test.Interop;
using UnrealSharp.Test.Model;
using UnrealSharp.Test.Utils;

namespace UnrealSharp.Test.Discovery;

public static class UnrealSharpTestDiscoveryClient
{
    public static IEnumerable<UnrealTestCase> DiscoverTests(IReadOnlyList<string> assemblyPaths)
    {
        foreach (var assemblyPath in assemblyPaths)
        {
            FName assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            var assemblyPtr = ManagedTestingExporter.CallFindUserAssembly(assemblyName);
            var assembly = GCHandleUtilities.GetObjectFromHandlePtr<Assembly>(assemblyPtr);
            if (assembly is null)
            {
                LogUnrealSharpTest.LogError($"Assembly {assemblyName} not found");
                continue;
            }

            foreach (var testCase in assembly.GetTypes()
                         .Where(IsTestClass)
                         .SelectMany(t => DiscoverTests(assemblyName, t)))
            {
                yield return testCase;
            }
        }
    }

    private static bool IsTestClass(Type type)
    {
        if (!type.IsClass)
        {
            return false;
        }
        
        return type.GetCustomAttribute<AutomationTestFixtureAttribute>() is not null || type.GetMethods().Any(IsTestMethod);
    }

    private static bool IsTestMethod(MethodInfo method)
    {
        return method.GetCustomAttribute<AutomationTestAttribute>() is not null;
    }

    private static IEnumerable<UnrealTestCase> DiscoverTests(FName assemblyName, Type testClass)
    {
        var category = testClass.GetCustomAttribute<AutomationTestFixtureAttribute>()?.Category;
        var prefix = category ?? testClass.FullName ?? testClass.Name;
        
        MethodInfo? setupMethod = null;
        MethodInfo? teardownMethod = null;
        foreach (var method in testClass.GetMethods())
        {
            if (method.GetCustomAttribute<AutomationSetupAttribute>() is not null)
            {
                if (setupMethod is not null) throw new InvalidOperationException("Only one setup method is allowed");
                setupMethod = method;
            }

            if (method.GetCustomAttribute<AutomationTearDownAttribute>() is null) continue;
            
            if (teardownMethod is not null) throw new InvalidOperationException("Only one teardown method is allowed");
            teardownMethod = method;
        }

        return testClass.GetMethods()
            .Where(IsTestMethod)
            .SelectMany(m => GetTestCases(assemblyName, prefix, m, setupMethod, teardownMethod));
    }

    private static IEnumerable<UnrealTestCase> GetTestCases(FName assemblyName, string prefix, MethodInfo method, MethodInfo? setupMethod, MethodInfo? teardownMethod)
    {
        var displayName = method.GetCustomAttribute<AutomationTestAttribute>()?.DisplayName;
        var testName = displayName is not null ? $"{prefix}.{displayName}" : $"{prefix}.{method.Name}";

        var sequencePoint = method.GetFirstSequencePoint();
        
        return
        [
            new UnrealTestCase(assemblyName, testName, setupMethod, teardownMethod, method)
            {
                CodeFilePath = sequencePoint?.Document.ToString(),
                LineNumber = sequencePoint?.StartLine ?? 0,
            }
        ];
    }
    
    

}