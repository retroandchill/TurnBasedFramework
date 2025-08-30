namespace UnrealSharp.UnrealSharpCommonUI;

public readonly partial struct FManagedDelegateHandle
{
    private readonly IntPtr _ref;

    internal FManagedDelegateHandle(IntPtr @ref)
    {
        _ref = @ref;
    }
}