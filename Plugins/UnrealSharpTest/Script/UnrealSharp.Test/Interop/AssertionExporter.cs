using UnrealSharp.Binds;
using UnrealSharp.CoreUObject;
using UnrealSharp.Test.Runner;

namespace UnrealSharp.Test.Interop;

[NativeCallbacks]
public static unsafe partial class AssertionExporter
{
    private static readonly delegate* unmanaged<ref WeakAutomationTestReference, char*, EAutomationEventType, char*, int, void> RecordAssertion;
}