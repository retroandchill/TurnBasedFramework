// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/TimerManagerExporter.h"

#include <bit>

#include "CSManagedDelegate.h"
#include "Kismet/GameplayStatics.h"


FTimerHandle UTimerManagerExporter::NextTick(UObject* WorldContext, uint64 Callback)
{
    FCSManagedDelegate ManagedCallback(FGCHandle(std::bit_cast<FGCHandleIntPtr>(Callback), GCHandleType::StrongHandle));
    const auto* GameInstance = UGameplayStatics::GetGameInstance(WorldContext);
    auto& TimerManager = GameInstance->GetTimerManager();
    return TimerManager.SetTimerForNextTick(FTimerDelegate::CreateWeakLambda(WorldContext,
                                                                      [WorldContext, ManagedCallback] mutable
                                                                      {
                                                                          ManagedCallback.Invoke(WorldContext);
                                                                      }));
}

void UTimerManagerExporter::CancelTimer(const UObject* WorldContext, FTimerHandle Handle)
{
    const auto* GameInstance = UGameplayStatics::GetGameInstance(WorldContext);
    auto& TimerManager = GameInstance->GetTimerManager();
    TimerManager.ClearTimer(Handle);   
}

