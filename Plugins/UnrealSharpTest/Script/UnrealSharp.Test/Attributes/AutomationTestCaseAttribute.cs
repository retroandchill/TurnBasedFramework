using System.Collections.Immutable;

namespace UnrealSharp.Test.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutomationTestCaseAttribute(params object?[] arguments) : Attribute
{
    public object?[] Arguments { get; } = arguments;
    
    public string? DisplayName { get; init; }
}