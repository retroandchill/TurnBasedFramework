// ReSharper disable once CheckNamespace
namespace UnrealSharp.UnrealSharpCommonUI;

public readonly ref partial struct FBindUIActionArgsRef
{
    internal readonly IntPtr _ref;

    internal FBindUIActionArgsRef(IntPtr @ref)
    {
        _ref = @ref;
    }
    
    public static unsafe implicit operator FBindUIActionArgsRef(FBindUIActionArgs* ptr) => new((IntPtr)ptr);
}