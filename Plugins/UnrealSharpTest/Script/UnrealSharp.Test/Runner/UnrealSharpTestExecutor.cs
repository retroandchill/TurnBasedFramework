using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using NUnit.VisualStudio.TestAdapter;
using UnrealSharp.Engine;
using UnrealSharp.Test.Model;

namespace UnrealSharp.Test.Runner;

public static class UnrealSharpTestExecutor
{
    public static async Task RunTestInProcess(UnrealTestCase testCase)
    {
        var testClass = testCase.Method.DeclaringType;
        ArgumentNullException.ThrowIfNull(testClass);

        var testInstance = Activator.CreateInstance(testClass);

        var setupResult = testCase.SetupMethod?.Invoke(testInstance, testCase.Arguments);
        switch (setupResult)
        {
            case Task task:
                await task;
                break;
            case ValueTask valueTask:
                await valueTask;
                break;
        }

        try
        {
            
        }
        finally
        {
            var result = testCase.TearDownMethod?.Invoke(testInstance, testCase.Arguments);
        }
    }

    private static async ValueTask RunTestMethod(this MethodInfo methodInfo, params object[] arguments)
    {
        
    }
}