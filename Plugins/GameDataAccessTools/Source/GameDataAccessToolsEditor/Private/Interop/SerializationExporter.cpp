// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/SerializationExporter.h"

void USerializationExporter::AssignSerializationActions(const FSerializationActions& SerializationActions)
{
    FSerializationCallbacks::Get().SetActions(SerializationActions);
}

void USerializationExporter::OnSerializationAction(const FSerializationAction* Action, const FGCHandleIntPtr Handle)
{
    (*Action)(Handle);
}
