using UnrealSharp.CoreUObject;
using UnrealSharp.Test.Model;
using UnrealSharp.Test.Runner;

namespace UnrealSharp.Test.Asserts;

public interface IAssertContext
{
    void LogPass(string? message = null, EventLocation location = default);
    void LogAssertionFailure(string message, EAutomationEventType type = EAutomationEventType.Error, EventLocation location = default);
}

public class SingleAssertContext : IAssertContext
{
    public void LogPass(string? message = null, EventLocation location = default)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            UnrealSharpTestExecutor.Context.LogEvent(message, EAutomationEventType.Info, location);
        }
        throw new SuccessException(message);
    }

    public void LogAssertionFailure(string message, EAutomationEventType type = EAutomationEventType.Error, EventLocation location = default)
    {
        UnrealSharpTestExecutor.Context.LogEvent(message, type, location);
        if (type == EAutomationEventType.Error)
        {
            throw new AssertionException(message);
        }
    }
}

public class MultiAssertContext : IAssertContext
{
    private readonly List<AutomationEvent> _messages = [];

    public void LogPass(string? message = null, EventLocation location = default)
    {
        throw new InvalidOperationException("Cannot log pass in multi assert context");
    }

    public void LogAssertionFailure(string message, EAutomationEventType type = EAutomationEventType.Error, EventLocation location = default)
    {
        UnrealSharpTestExecutor.Context.LogEvent(message, type, location);
        _messages.Add(new AutomationEvent(message, type, location));
    }

    public void AssertAll()
    {
        var errors = _messages
            .Where(x => x.Severity == EAutomationEventType.Error)
            .Select(x => x.Message)
            .ToArray();
        if (errors.Length > 0)
        {
            throw new AssertionException($"Multiple assertions failed: \n{string.Join("\n", errors)}");
        }
    }
}

public readonly struct MultiAssertContextScope : IDisposable
{
    private readonly MultiAssertContext _context;

    public MultiAssertContextScope()
    {
        _context = new MultiAssertContext();
        Assert.Context = _context;
    }

    public void Dispose()
    {
        _context.AssertAll();
    }
}