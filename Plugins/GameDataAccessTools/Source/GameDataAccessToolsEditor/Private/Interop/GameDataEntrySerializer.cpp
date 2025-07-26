// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/GameDataEntrySerializer.h"

#include "DataRetrieval/GameDataEntry.h"
#include "Interop/SerializationCallbacks.h"

FText FGameDataEntrySerializer::GetFormatName() const
{
    return FSerializationCallbacks::Get().GetActionText(Handle.Handle);   
}

FString FGameDataEntrySerializer::GetFileExtensionText() const
{
    return FSerializationCallbacks::Get().GetFileExtensionText(Handle.Handle);
}

tl::expected<FString, FString> FGameDataEntrySerializer::SerializeData(const UGameDataRepository* Repository) const
{
    return FSerializationCallbacks::Get().SerializeToString(Handle.Handle, Repository); 
}

tl::expected<TArray<UGameDataEntry*>, FString> FGameDataEntrySerializer::DeserializeData(const FString& Data,
    UGameDataRepository* Repository) const
{
    return FSerializationCallbacks::Get().DeserializeFromString(Handle.Handle, Repository);
}
