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
