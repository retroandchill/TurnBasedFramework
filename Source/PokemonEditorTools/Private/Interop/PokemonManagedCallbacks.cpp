// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/PokemonManagedCallbacks.h"


FPokemonManagedCallbacks& FPokemonManagedCallbacks::Get()
{
    static FPokemonManagedCallbacks Instance;
    return Instance;
}

void FPokemonManagedCallbacks::SetActions(const FPokemonManagedActions& InActions)
{
    Actions = InActions;
}

tl::expected<void, FString> FPokemonManagedCallbacks::PopulateEvolutions(const FProperty* Property,
    const FScriptMap& ScriptMap) const
{
    FString Error;
    if (Actions.PopulateEvolutions(Property, &ScriptMap, &Error))
    {
        return {};
    }

    return tl::unexpected(MoveTemp(Error));
}
