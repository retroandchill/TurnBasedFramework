namespace UnrealSharp.Test.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AutomationTestFixtureAttribute : Attribute
{
    public string? Category { get; init; }
}