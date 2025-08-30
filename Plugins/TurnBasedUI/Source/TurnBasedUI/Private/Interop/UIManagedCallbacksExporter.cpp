// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/UIManagedCallbacksExporter.h"

#include "Interop/UIManagedCallbacks.h"

void UUIManagedCallbacksExporter::SetActions(const FUIManagedActions& Actions)
{
    FUIManagedCallbacks::Get().SetActions(Actions);   
}
