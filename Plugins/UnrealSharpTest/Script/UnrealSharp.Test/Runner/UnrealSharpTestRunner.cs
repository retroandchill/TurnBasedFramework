namespace UnrealSharp.Test.Runner;

public interface IUnrealSharpTestRunner
{
    IEnumerable<string> TestCases { get; }
    
    Task RunTest(string testCase);
}