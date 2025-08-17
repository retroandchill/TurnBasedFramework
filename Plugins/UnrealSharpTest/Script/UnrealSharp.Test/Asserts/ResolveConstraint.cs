namespace UnrealSharp.Test.Asserts;

public interface IResolveConstraint<in T>
{
    bool Matches(T actual);
    string GetFailureMessage(T actual);
}