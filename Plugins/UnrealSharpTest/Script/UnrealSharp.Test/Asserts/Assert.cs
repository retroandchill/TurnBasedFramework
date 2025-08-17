namespace UnrealSharp.Test.Asserts;

public class AssertionException(string message, Exception? cause = null) : Exception(message, cause);

public class SuccessException : Exception;

public static class Assert
{
    public static void That<T>(T actual, IResolveConstraint<T> constraint)
    {
        if (!constraint.Matches(actual))
        {
            throw new AssertionException(constraint.GetFailureMessage(actual));
        }
    }

    public static void Pass()
    {
        throw new SuccessException();
    }
    
    public static void Fail(string message)
    {
        throw new AssertionException(message);
    }
}