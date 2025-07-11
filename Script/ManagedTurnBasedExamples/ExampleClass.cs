using System.Numerics;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;

namespace ManagedTurnBasedExamples;

[UClass]
public class AExampleClass : AActor
{

    [UFunction(FunctionFlags.BlueprintCallable)]
    public FStructWithManagedProperty GetStruct()
    {
        return new FStructWithManagedProperty
        {
            NativeProperty = "Native String",
            UnmarkedProperty = 4,
            ManagedProperty = Enumerable.Range(0, 10).ToLookup(x => x % 3, x => x.ToString()),
            UnmanagedStruct = new Vector3(1, 2, 3)
        };
    }
    
    [UFunction(FunctionFlags.BlueprintCallable)]
    public void TakeStruct(FStructWithManagedProperty value)
    {
        PrintString(value.NativeProperty);
        PrintString(value.UnmarkedProperty.ToString());
        foreach (var grouping in value.ManagedProperty)
        {
            PrintString(grouping.Key.ToString());
            PrintString(string.Join(",", grouping));
        }
        PrintString(value.UnmanagedStruct.ToString());
    }
    
    
}