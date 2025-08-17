using System.Reflection;
using UnrealSharp.Test.Asserts;
using UnrealSharp.Test.Model;

namespace UnrealSharp.Test.Runner;

public static class UnrealSharpTestExecutor
{
    public static async Task RunTestInProcess(UnrealTestCase testCase)
    {
        var testClass = testCase.Method.DeclaringType;
        ArgumentNullException.ThrowIfNull(testClass);

        var testInstance = Activator.CreateInstance(testClass);

        await RunTestMethod(testInstance, testCase.SetupMethod);

        try
        {
            await RunTestMethod(testInstance, testCase.Method, testCase.Arguments);
        }
        catch (SuccessException)
        {
            // Do nothing, just swallow the exception
        }
        catch (AssertionException e)
        {
            LogUnrealSharpTest.LogError(e.Message);
        }
        finally
        {
            await RunTestMethod(testInstance, testCase.TearDownMethod);
        }
    }

    private static async ValueTask RunTestMethod(object? testFixture, MethodInfo? methodInfo, params object[] arguments)
    {
        try
        {
            var result = methodInfo?.Invoke(testFixture, arguments);
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