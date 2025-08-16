using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace UnrealSharp.Test.Discovery;

public class UnrealSharpTestMessageLogger : IMessageLogger
{
    public static UnrealSharpTestMessageLogger Instance { get; } = new();

    private UnrealSharpTestMessageLogger()
    {
        
    }
    
    public void SendMessage(TestMessageLevel testMessageLevel, string message)
    {
        switch (testMessageLevel)
        {
            case TestMessageLevel.Informational:
                LogUnrealSharpTest.Log(message);
                break;
            case TestMessageLevel.Warning:
                LogUnrealSharpTest.LogWarning(message);
                break;
            case TestMessageLevel.Error:
                LogUnrealSharpTest.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testMessageLevel), testMessageLevel, null);
        }
    }
}