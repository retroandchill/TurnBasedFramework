using System.Numerics;
using UnrealSharp.Attributes;

namespace ManagedTurnBasedExamples;

[UStruct]
public struct FStructWithManagedProperty
{
    [UProperty]
    public string NativeProperty;

    public int UnmarkedProperty;

    public ILookup<int, string> ManagedProperty;

    public Vector3 UnmanagedStruct;
}