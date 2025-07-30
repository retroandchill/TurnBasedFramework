// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/SerializationExporter.h"

#include "Interop/GameDataEntrySerializer.h"

void USerializationExporter::AssignSerializationActions(const FSerializationActions& SerializationActions)
{
    FSerializationCallbacks::Get().SetActions(SerializationActions);
}

void USerializationExporter::AddSerializationAction(TArray<TSharedRef<FGameDataEntrySerializer>>& Serializers,
                                                    const FGCHandleIntPtr Handle)
{
    Serializers.Emplace(MakeShared<FGameDataEntrySerializer>(Handle));
}

void USerializationExporter::AddEntryToCollection(TArray<UObject*>& Entries, UObject* EntryObject)
{
    Entries.Emplace(EntryObject); 
}
