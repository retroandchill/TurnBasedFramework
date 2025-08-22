using UnrealSharp.Binds;

namespace Pokemon.Editor.Interop;

[NativeCallbacks]
public static unsafe partial class PokemonActionsExporter
{
    private static readonly delegate* unmanaged<ref PokemonManagedActions, void> SetActions;
}
