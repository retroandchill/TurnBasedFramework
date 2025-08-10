// Fill out your copyright notice in the Description page of Project Settings.


#include "Utils/GameplayTagTreeLibrary.h"

#include "GameplayTagsModule.h"

FGameplayTagContainer UGameplayTagTreeLibrary::GetGameplayTagChildren(const FGameplayTag Tag)
{
    const auto &TagsManager = UGameplayTagsManager::Get();
    return TagsManager.RequestGameplayTagChildren(Tag);
}
