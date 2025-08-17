using UnrealSharp.Test.Asserts.Constraints;

namespace UnrealSharp.Test.Asserts;

public static class Is
{
    public static EqualConstraint<bool> True { get; } = new(true);
    
    public static EqualConstraint<bool> False { get; } = new(false);
    
    public static EqualConstraint<T> EqualTo<T>(T expected)
    {
        return new EqualConstraint<T>(expected);
    }
}