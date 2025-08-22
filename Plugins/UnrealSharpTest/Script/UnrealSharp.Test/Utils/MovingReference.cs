namespace UnrealSharp.Test.Utils;

public sealed class MovingReference<T>(T value)
{
    public T Value { get; set; } = value;
}