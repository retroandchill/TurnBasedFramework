using System.Reflection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnrealSharp.CoreUObject;
using UnrealSharp.Test.Interop;
using UnrealSharp.Test.Model;

namespace UnrealSharp.Test.Runner;

public static class UnrealSharpTestExecutor
{
    private static readonly Dictionary<Type, object?> TestClassInstances = new();

    internal static void ClearTestClassInstances()
    {
        TestClassInstances.Clear();
    }
    
    public static bool RunTestInProcess(AutomationTestRef automationTestReference, UnrealTestCase testCase)
    {
        var testTask = RunTestInProcessInternal(testCase);
        if (testTask.IsCompletedSuccessfully)
        {
            return testTask.Result;
        }
        
        automationTestReference.EnqueueNativeTask(testTask.AsTask());
        return true;
    }

    private static async ValueTask<bool> RunTestInProcessInternal(UnrealTestCase testCase)
    {
        using var nunitContext = new TestExecutionContext.IsolatedContext();
        var testResult = TestExecutionContext.CurrentContext.CurrentResult;

        try
        {
            var testClass = testCase.Method.DeclaringType;
            ArgumentNullException.ThrowIfNull(testClass);

            if (!TestClassInstances.TryGetValue(testClass, out var testInstance))
            {
                testInstance = Activator.CreateInstance(testClass);
                TestClassInstances[testClass] = testInstance;
            }

            try
            {
                await RunTestMethod(testInstance, testCase.SetupMethod);
                await RunTestMethod(testInstance, testCase.Method, testCase.Arguments);
            }
            catch (Exception e)
            {
                testResult.RecordException(e);
            }
            finally
            {
                try
                {
                    await RunTestMethod(testInstance, testCase.TearDownMethod);
                }
                catch (Exception e)
                {
                    testResult.RecordTearDownException(e);
                }
            }
        }
        catch (Exception e)
        {
            testResult.RecordException(e);
        }

        LogTestResult(testResult);
        return testResult.ResultState.Status != TestStatus.Failed;
    }

    public static void LogTestResult(TestResult result)
    {
        if (!string.IsNullOrWhiteSpace(result.Message) 
            && result.AssertionResults.All(r => r.Message != result.Message))
        {
            LogTestMessage(result.Message);
        }
        
        foreach (var assertion in result.AssertionResults)
        {
            var eventType = assertion.Status switch
            {
                AssertionStatus.Inconclusive => EAutomationEventType.Info,
                AssertionStatus.Passed => EAutomationEventType.Info,
                AssertionStatus.Warning => EAutomationEventType.Warning,
                AssertionStatus.Failed => EAutomationEventType.Error,
                AssertionStatus.Error => EAutomationEventType.Error,
                _ => throw new InvalidOperationException("Unknown assertion status")
            };
            
            LogTestMessage(assertion.Message, eventType);
        }
    }

    private static void LogTestMessage(string message, EAutomationEventType eventType = EAutomationEventType.Info)
    {
        unsafe
        {
            fixed (char* messagePtr = message)
            {
                AutomationTestExporter.CallLogEvent(messagePtr, eventType);
            }
        }
    }

    private static async ValueTask RunTestMethod(object? testFixture, MethodInfo? methodInfo, params object?[] arguments)
    {
        if (methodInfo is null) return;
        try
        {
            var result = methodInfo.Invoke(testFixture, arguments);
            switch (result)
            {
                case null:
                    return;
                case Task task:
                    await task;
                    break;
                case ValueTask valueTask:
                    await valueTask;
                    break;
            }

            var resultType = result.GetType();
            if (!resultType.IsGenericType) return;

            var genericTypeDefinition = resultType.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(ValueTask<>))
            {
                var awaitMethod = typeof(UnrealSharpTestExecutor)
                    .GetMethod(nameof(AwaitValueTask), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(resultType.GenericTypeArguments[0]);
                
                var awaitTask = (ValueTask)awaitMethod.Invoke(null, [result])!;
                await awaitTask;
            }
        }
        catch (TargetInvocationException e)
        {
            if (e.InnerException is null)
            {
                throw new InvalidOperationException("Test method threw an exception but no inner exception was set", e);
            }
            
            throw e.InnerException;
        }
    }

    private static async ValueTask AwaitValueTask<T>(ValueTask<T> valueTask)
    {
        await valueTask;
    }
}