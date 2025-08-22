using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Pokemon.Data.Core;
using Pokemon.Editor.Utils;
using UnrealSharp;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Interop;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PokemonManagedActions
{
    [UsedImplicitly]
    public required delegate* unmanaged<
        IntPtr,
        IntPtr,
        IntPtr,
        NativeBool> PopulateEvolutions { get; init; }

    [UsedImplicitly]
    public required delegate* unmanaged<
        FGameplayTag,
        IntPtr,
        IntPtr,
        NativeBool> GetEvolutionConditionClass { get; init; }

    public static PokemonManagedActions Create()
    {
        return new PokemonManagedActions
        {
            PopulateEvolutions = &PokemonManagedCallbacks.PopulateEvolutions,
            GetEvolutionConditionClass = &PokemonManagedCallbacks.GetEvolutionConditionClass,
        };
    }
}

public static class PokemonManagedCallbacks
{
    [UnmanagedCallersOnly]
    public static NativeBool PopulateEvolutions(
        IntPtr mapProperty,
        IntPtr nativeMap,
        IntPtr resultString
    )
    {
        try
        {
            var managedMap = new TMap<FName, FGameplayTag>(
                mapProperty,
                nativeMap,
                BlittableMarshaller<FName>.FromNative,
                BlittableMarshaller<FName>.ToNative,
                StructMarshaller<FGameplayTag>.FromNative,
                StructMarshaller<FGameplayTag>.ToNative
            );
            managedMap.PopulateWithEvolutionData();
            return NativeBool.True;
        }
        catch (Exception e)
        {
            StringMarshaller.ToNative(resultString, 0, $"{e.Message}\n{e.StackTrace}");
            return NativeBool.False;
        }
    }

    [UnmanagedCallersOnly]
    public static NativeBool GetEvolutionConditionClass(
        FGameplayTag tag,
        IntPtr evolutionMethodResult,
        IntPtr resultString
    )
    {
        try
        {
            var evolutionMethodTag = PopulationUtils.GetEvolutionMethodClass(tag);
            SubclassOfMarshaller<UEvolutionConditionData>.ToNative(
                evolutionMethodResult,
                0,
                evolutionMethodTag
            );
            return NativeBool.True;
        }
        catch (Exception e)
        {
            StringMarshaller.ToNative(resultString, 0, $"{e.Message}\n{e.StackTrace}");
            return NativeBool.False;
        }
    }
}
