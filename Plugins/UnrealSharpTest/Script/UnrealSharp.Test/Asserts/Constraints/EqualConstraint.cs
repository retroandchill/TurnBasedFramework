using Retro.ReadOnlyParams.Annotations;

namespace UnrealSharp.Test.Asserts.Constraints;

public class EqualConstraint<T>([ReadOnly] T expected) : IResolveConstraint<T>
{
    
    public bool Matches(T actual)
    {
        return EqualityComparer<T>.Default.Equals(actual, expected);
    }

    public string GetFailureMessage(T actual)
    {
        return $"Expected {expected} but was {actual}";
    }
}