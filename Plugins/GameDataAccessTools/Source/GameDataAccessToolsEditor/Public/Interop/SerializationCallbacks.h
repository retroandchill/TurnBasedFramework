// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "Utils/expected.hpp"

class FGameDataEntrySerializer;
class UGameDataRepository;
using FSerializationAction = TFunctionRef<void(const FGCHandleIntPtr)>;

struct FSerializationActions
{
    using FGetSerializationActions = void(__stdcall*)(const UClass*, const TArray<TSharedRef<FGameDataEntrySerializer>>*);
    using FGetActionText = void(__stdcall*)(const FGCHandleIntPtr, FText*);
    using FGetFileExtensionText = void(__stdcall*)(const FGCHandleIntPtr, FString*);
    using FSerializeToString = bool(__stdcall*)(const FGCHandleIntPtr, const UGameDataRepository*, FString*);
    using FDeserializeFromString = bool(__stdcall*)(const FGCHandleIntPtr, const FString*, const UGameDataRepository*, TArray<UObject*>*, FString*);

    FGetSerializationActions GetSerializationActions = nullptr;
    FGetActionText GetActionText = nullptr;
    FGetFileExtensionText GetFileExtensionText = nullptr;
    FSerializeToString SerializeToString = nullptr;
    FDeserializeFromString DeserializeFromString = nullptr;
};

class FSerializationCallbacks
{
    FSerializationCallbacks() = default;
    ~FSerializationCallbacks() = default;

public:
    static FSerializationCallbacks& Get();

    void SetActions(const FSerializationActions& InActions);
    TArray<TSharedRef<FGameDataEntrySerializer>> GetSerializationActions(const UClass* Class) const;
    FText GetActionText(const FGCHandleIntPtr Handle) const;
    FString GetFileExtensionText(const FGCHandleIntPtr Handle) const;
    tl::expected<FString, FString> SerializeToString(const FGCHandleIntPtr Handle, const UGameDataRepository* Repository) const;
    tl::expected<TArray<UObject*>, FString> DeserializeFromString(const FGCHandleIntPtr Handle, const FString& InputString, const UGameDataRepository* Repository) const;

private:
    FSerializationActions Actions;
};