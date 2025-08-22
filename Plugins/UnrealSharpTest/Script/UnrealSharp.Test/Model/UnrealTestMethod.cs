using System.Reflection;
using NUnit.Framework;

namespace UnrealSharp.Test.Model;

public sealed record UnrealTestMethod(
    FName AssemblyName,
    string FullyQualifiedName,
    MethodInfo? SetupMethod,
    MethodInfo? TearDownMethod,
    MethodInfo Method
)
{
    public IReadOnlyDictionary<FName, TestCaseData> TestCases { get; init; } =
        new OrderedDictionary<FName, TestCaseData>();
    public string? CodeFilePath { get; init; }
    public int LineNumber { get; init; }
}
