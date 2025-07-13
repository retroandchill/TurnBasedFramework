// Fill out your copyright notice in the Description page of Project Settings.


#include "DataRetrieval/GameDataAccessSubsystem.h"

void UGameDataAccessSubsystem::Initialize(FSubsystemCollectionBase& Collection) {
  Super::Initialize(Collection);
}

void UGameDataAccessSubsystem::Deinitialize() {
  Super::Deinitialize();
  DataTableProxies.Empty();
}
