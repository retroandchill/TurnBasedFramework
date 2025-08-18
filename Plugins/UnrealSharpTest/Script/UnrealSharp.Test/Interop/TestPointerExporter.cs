using UnrealSharp.Binds;
using UnrealSharp.Test.Runner;

namespace UnrealSharp.Test.Interop;

[NativeCallbacks]
public static unsafe partial class TestPointerExporter
{
    private static readonly delegate* unmanaged<ref WeakAutomationTestReference, ref WeakAutomationTestReference, void> CopyWeakPtr;
    private static readonly delegate* unmanaged<ref WeakAutomationTestReference, void> ReleaseWeakPtr;
}