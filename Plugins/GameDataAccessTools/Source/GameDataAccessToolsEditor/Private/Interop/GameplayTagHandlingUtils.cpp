// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/GameplayTagHandlingUtils.h"

#include "GameplayTagsEditorModule.h"
#include "GameplayTagsManager.h"


bool UGameplayTagHandlingUtils::TryAddGameplayTagToIni(const FName TagSource, const FString& TagName, FString& Error)
{
    if (UGameplayTagsManager::Get().IsValidGameplayTagString(TagName))
    {
        Error.Empty();
        return true;
    }

    FText ErrorMsg;
    if (!UGameplayTagsManager::Get().IsValidGameplayTagString(TagName, &ErrorMsg))
    {
        Error = ErrorMsg.ToString();
        return false;
    }

    if (!IGameplayTagsEditorModule::Get().AddNewGameplayTagToINI(TagName, TEXT(""), TagSource))
    {
        Error = TEXT("An unknown error occurred!");
        return false;
    }

    Error.Empty();
    return true;
}
