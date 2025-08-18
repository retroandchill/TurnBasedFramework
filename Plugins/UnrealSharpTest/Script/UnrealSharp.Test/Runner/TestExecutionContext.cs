using UnrealSharp.CoreUObject;
using UnrealSharp.Test.Model;

namespace UnrealSharp.Test.Runner;

public interface ITestExecutionContext : IDisposable
{
    void LogEvent(string message, EAutomationEventType type, EventLocation location);
}