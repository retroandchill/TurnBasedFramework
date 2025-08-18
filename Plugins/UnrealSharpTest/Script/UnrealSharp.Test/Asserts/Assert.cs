using System.Runtime.CompilerServices;
using UnrealSharp.Test.Model;

namespace UnrealSharp.Test.Asserts;

public class AutomationException(string? message = null, Exception? cause = null) : Exception(message, cause);

public class AssertionException(string? message = null, Exception? cause = null) : AutomationException(message, cause);

public class SuccessException(string? message = null, Exception? cause = null) : AutomationException(message, cause);

public static class Assert
{
    internal static IAssertContext Context { get; set; } = new SingleAssertContext();
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void That<T>(T actual, IResolveConstraint<T> constraint)
    {
        if (!constraint.Matches(actual))
        {
            Context.LogAssertionFailure(constraint.GetFailureMessage(actual), location: EventLocation.FromCurrentStack(1));
        }
    }

    public static void Throws<TException>(Action action) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            // This is the expected exception type
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected exception {typeof(TException).Name} but was {e.GetType().Name}\n{e}");
        }
    }
    
    public static async ValueTask ThrowsAsync<TException>(Func<Task> action) where TException : Exception
    {
        try
        {
            await action();
        }
        catch (TException)
        {
            // This is the expected exception type
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected exception {typeof(TException).Name} but was {e.GetType().Name}\n{e}");
        }
    }
    
    public static async ValueTask ThrowsAsync<TException>(Func<ValueTask> action) where TException : Exception
    {
        try
        {
            await action();
        }
        catch (TException)
        {
            // This is the expected exception type
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected exception {typeof(TException).Name} but was {e.GetType().Name}\n{e}");
        }
    }

    public static void DoesNotThrow(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected no exception but was {e.GetType().Name}\n{e}");
        }
    }

    public static async ValueTask DoesNotThrowAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected no exception but was {e.GetType().Name}\n{e}");
        }
    }

    public static async ValueTask DoesNotThrowAsync(Func<ValueTask> action)
    {
        try
        {
            await action();
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected no exception but was {e.GetType().Name}\n{e}");
        }
    }

    public static void Multiple(Action action)
    {
        using var _ = new MultiAssertContextScope();
        action();
    }
    
    public static async ValueTask MultipleAsync(Func<Task> action)
    {
        using var _ = new MultiAssertContextScope();
        await action();
    }

    public static async ValueTask MultipleAsync(Func<ValueTask> action)
    {
        using var _ = new MultiAssertContextScope();
        await action();
    }

    public static void Pass(string? message = null)
    {
        Context.LogPass(message, location: EventLocation.FromCurrentStack(1));
    }
    
    public static void Fail(string message)
    {
        Context.LogAssertionFailure(message, location: EventLocation.FromCurrentStack(1));
    }
}