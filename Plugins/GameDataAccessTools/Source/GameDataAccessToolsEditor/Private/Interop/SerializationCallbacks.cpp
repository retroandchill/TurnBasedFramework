// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/SerializationCallbacks.h"

#include "DataRetrieval/GameDataEntry.h"


FSerializationCallbacks& FSerializationCallbacks::Get()
{
    static FSerializationCallbacks Instance;
    return Instance;   
}

void FSerializationCallbacks::SetActions(const FSerializationActions& InActions)
{
    this->Actions = InActions;
}

TArray<TSharedRef<FGameDataEntrySerializer>> FSerializationCallbacks::GetSerializationActions(const UClass* Class) const
{
    TArray<TSharedRef<FGameDataEntrySerializer>> Result;
    check(Actions.GetSerializationActions != nullptr);
    Actions.GetSerializationActions(Class, &Result);
    return Result; 
}

FText FSerializationCallbacks::GetActionText(const FGCHandleIntPtr Handle) const
{
    FText Result;
    check(Actions.GetActionText != nullptr);
    Actions.GetActionText(Handle, &Result);
    return Result;
}

FString FSerializationCallbacks::GetFileExtensionText(const FGCHandleIntPtr Handle) const
{
    FString Result;
    check(Actions.GetFileExtensionText != nullptr);
    Actions.GetFileExtensionText(Handle, &Result);
    return Result;
}

tl::expected<FString, FString> FSerializationCallbacks::SerializeToString(const FGCHandleIntPtr Handle,
                                                   const UGameDataRepository* Repository) const
{
    FString Result;
    check(Actions.SerializeToString != nullptr);
    if (Actions.SerializeToString(Handle, Repository, &Result))
    {
        return Result;
    }

    return tl::unexpected(Result);
}

tl::expected<TArray<UGameDataEntry*>, FString> FSerializationCallbacks::DeserializeFromString(const FGCHandleIntPtr Handle,
    const UGameDataRepository* Repository) const
{
    TArray<UGameDataEntry*> Result;
    FString Exception;
    check(Actions.SerializeToString != nullptr);
    if (Actions.DeserializeFromString(Handle, Repository, &Result, &Exception))
    {
        return MoveTemp(Result);
    }
    return tl::unexpected(MoveTemp(Exception));
}
