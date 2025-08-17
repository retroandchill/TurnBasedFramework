using System.Reflection;

namespace UnrealSharp.Test.Model;

public sealed record UnrealTestCase(
    FName AssemblyName,
    string FullyQualifiedName,
    MethodInfo? SetupMethod,
    MethodInfo? TearDownMethod,
    MethodInfo Method)
{
    public object[] Arguments { get; init; } = [];
    public string? CodeFilePath { get; init; }
    public int LineNumber { get; init; }
}