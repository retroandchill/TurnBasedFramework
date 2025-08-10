// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/PokemonActionsExporter.h"

#include "Interop/PokemonManagedCallbacks.h"

void UPokemonActionsExporter::SetActions(const FPokemonManagedActions& InActions)
{
    FPokemonManagedCallbacks::Get().SetActions(InActions);
}
