using System.Reflection;
using NUnit.Framework;

namespace UnrealSharp.Test.Model;

public sealed record UnrealTestMethod(
    FName AssemblyName,
    string FullyQualifiedName,
    MethodInfo? SetupMethod,
    MethodInfo? TearDownMethod,
    MethodInfo Method)
{
    public IReadOnlyDictionary<FName, TestCaseData> TestCases { get; init; } = new Dictionary<FName, TestCaseData>();
    public string? CodeFilePath { get; init; }
    public int LineNumber { get; init; }

    public bool SetupMethodCancellable => IsCancellable(SetupMethod);
    
    public bool TearDownMethodCancellable => IsCancellable(TearDownMethod);

    private static bool IsCancellable(MethodInfo? methodInfo)
    {
        if (methodInfo is null) return false;

        var parameter = methodInfo.GetParameters().SingleOrDefault();
        return parameter?.ParameterType == typeof(CancellationToken);
    }
}