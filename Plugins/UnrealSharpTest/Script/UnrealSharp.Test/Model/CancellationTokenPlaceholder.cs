namespace UnrealSharp.Test.Model;

public sealed class CancellationTokenPlaceholder
{
    public static CancellationTokenPlaceholder Default { get; } = new();
    
    private CancellationTokenPlaceholder()
    {
        
    }
}