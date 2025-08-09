// Fill out your copyright notice in the Description page of Project Settings.


#include "Utils/TextSerializationBlueprintLibrary.h"

FText UTextSerializationBlueprintLibrary::FromLocalizedString(const FString& LocalizedString)
{
    FText TextValue;
    if (!FTextStringHelper::ReadFromBuffer(*LocalizedString, TextValue))
    {
        TextValue = FText::FromString(LocalizedString);
    }
    return TextValue;
}

FString UTextSerializationBlueprintLibrary::ToLocalizedString(const FText& Text)
{
    FString Output;
    FTextStringHelper::WriteToBuffer(Output, Text);
    return Output;
}

FText UTextSerializationBlueprintLibrary::CreateLocalizedText(const FString& Namespace, const FString& Key,
    const FString& DefaultValue)
{
    return FText::AsLocalizable_Advanced(Namespace, Key, DefaultValue);
}

bool UTextSerializationBlueprintLibrary::TryGetNamespace(const FText& Text, FString& OutNamespace)
{
    if (auto Namespace = FTextInspector::GetNamespace(Text); Namespace.IsSet())
    {
        OutNamespace = Namespace.GetValue();
        return true;
    }

    OutNamespace.Empty();
    return false;
}

bool UTextSerializationBlueprintLibrary::TryGetKey(const FText& Text, FString& OutKey)
{
    if (auto Namespace = FTextInspector::GetKey(Text); Namespace.IsSet())
    {
        OutKey = Namespace.GetValue();
        return true;
    }

    OutKey.Empty();
    return false;
}

TSubclassOf<UObject> UTextSerializationBlueprintLibrary::GetClassFromPath(
    const FString& Path)
{
    const TSoftClassPtr SoftPath(Path);
    return SoftPath.LoadSynchronous();
}

TSoftObjectPtr<> UTextSerializationBlueprintLibrary::GetSoftObjectPtrFromPath(const FString& Path)
{
    return TSoftObjectPtr(FSoftObjectPath(Path));
}

TSoftClassPtr<> UTextSerializationBlueprintLibrary::GetSoftClassPtrFromPath(const FString& Path)
{
    return TSoftClassPtr(FSoftClassPath(Path));
}
