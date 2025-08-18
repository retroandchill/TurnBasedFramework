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
    public static void That<T>(T actual, IResolveConstraint<T> constraint,
                               [CallerFilePath] string? filePath = null,
                               [CallerLineNumber] int lineNumber = 0)
    {
        if (!constraint.Matches(actual))
        {
            Context.LogAssertionFailure(constraint.GetFailureMessage(actual), 
                location: EventLocation.From(filePath, lineNumber));
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Throws<TException>(Action action,
                                          [CallerFilePath] string? filePath = null,
                                          [CallerLineNumber] int lineNumber = 0) where TException : Exception
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
            Context.LogAssertionFailure($"Expected exception {typeof(TException).Name} but was {e.GetType().Name}\n{e}", 
                location: EventLocation.From(filePath, lineNumber));
        }
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static async ValueTask ThrowsAsync<TException>(Func<Task> action,
                                                          [CallerFilePath] string? filePath = null,
                                                          [CallerLineNumber] int lineNumber = 0) 
        where TException : Exception
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
            Context.LogAssertionFailure($"Expected exception {typeof(TException).Name} but was {e.GetType().Name}\n{e}", 
                location: EventLocation.From(filePath, lineNumber));
        }
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static async ValueTask ThrowsAsync<TException>(Func<ValueTask> action,
                                                          [CallerFilePath] string? filePath = null,
                                                          [CallerLineNumber] int lineNumber = 0) 
        where TException : Exception
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void DoesNotThrow(Action action,
                                    [CallerFilePath] string? filePath = null,
                                    [CallerLineNumber] int lineNumber = 0)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected no exception but was {e.GetType().Name}\n{e}", 
                location: EventLocation.From(filePath, lineNumber));
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static async ValueTask DoesNotThrowAsync(Func<Task> action,
                                                    [CallerFilePath] string? filePath = null,
                                                    [CallerLineNumber] int lineNumber = 0)
    {
        try
        {
            await action();
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected no exception but was {e.GetType().Name}\n{e}", 
                location: EventLocation.From(filePath, lineNumber));
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static async ValueTask DoesNotThrowAsync(Func<ValueTask> action,
                                                    [CallerFilePath] string? filePath = null,
                                                    [CallerLineNumber] int lineNumber = 0)
    {
        try
        {
            await action();
        }
        catch (Exception e)
        {
            Context.LogAssertionFailure($"Expected no exception but was {e.GetType().Name}\n{e}", 
                location: EventLocation.From(filePath, lineNumber));
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Pass(string? message = null,
                            [CallerFilePath] string? filePath = null,
                            [CallerLineNumber] int lineNumber = 0)
    {
        Context.LogPass(message, location: EventLocation.From(filePath, lineNumber));
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Fail(string message,
                            [CallerFilePath] string? filePath = null,
                            [CallerLineNumber] int lineNumber = 0)
    {
        Context.LogAssertionFailure(message, location: EventLocation.From(filePath, lineNumber));
    }
}