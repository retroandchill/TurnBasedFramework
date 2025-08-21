using NUnit.Framework;

namespace UnrealSharp.Test.Discovery;

public static class ArgumentNameFormatter
{
    public static string GetDisplayName(this TestCaseData testCase)
    {
        return testCase.TestName ?? $"({string.Join(";", testCase.Arguments.Select(GetArgumentDisplayName))})";
    }

    private static string GetArgumentDisplayName(object? argument)
    {
        return argument switch
        {
            null => "null",
            string str => $"\"{str}\"",
            char ch => $"'{ch}'",
            _ => argument.ToString()!.Replace(",", "")
        };
    }
}