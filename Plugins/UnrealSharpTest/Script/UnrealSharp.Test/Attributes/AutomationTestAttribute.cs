namespace UnrealSharp.Test.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AutomationTestAttribute : Attribute
{
    public string? DisplayName { get; init; }
}