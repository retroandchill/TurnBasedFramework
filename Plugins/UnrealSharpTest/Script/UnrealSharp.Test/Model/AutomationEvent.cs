using System.Diagnostics;
using UnrealSharp.CoreUObject;

namespace UnrealSharp.Test.Model;

public readonly record struct EventLocation(string File, int Line)
{
    public static EventLocation FromException(Exception e)
    {
        return FromStackTrace(new StackTrace(e, true));
    }
    
    public static EventLocation FromCurrentStack(int offset = 0)
    {
        return FromStackTrace(new StackTrace(true), offset + 1);
    }

    public static EventLocation FromStackTrace(StackTrace stackTrace, int offset = 0)
    {
        var frame = stackTrace.GetFrame(offset);
        return frame is not null ? FromStackFrame(frame) : default;
    }
    
    public static EventLocation FromStackFrame(StackFrame frame)
    {
        return new EventLocation(frame.GetFileName() ?? "unknown", frame.GetFileLineNumber());
    }
}

public readonly record struct AutomationEvent(string Message, EAutomationEventType Severity, EventLocation Location);