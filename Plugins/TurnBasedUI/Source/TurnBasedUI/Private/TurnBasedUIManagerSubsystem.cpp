// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUIManagerSubsystem.h"

#include "LogTurnBasedUI.h"
#include "TurnBasedUIPolicy.h"

void UTurnBasedUIManagerSubsystem::Initialize(FSubsystemCollectionBase& Collection)
{
    Super::Initialize(Collection);
    if (CurrentPolicy == nullptr && !DefaultUIPolicyClass.IsNull())
    {
        const auto PolicyClass = DefaultUIPolicyClass.LoadSynchronous();
        SwitchToPolicy(NewObject<UTurnBasedUIPolicy>(this, PolicyClass));
    }

    auto *GameInstance = GetGameInstance();
    GameInstance->OnLocalPlayerAddedEvent.AddWeakLambda(this, [this](ULocalPlayer* NewPlayer)
    {
        if (!PrimaryPlayer.IsValid())
        {
            UE_LOG(LogTurnBasedUI, Log, TEXT("AddLocalPlayer: Set %s to Primary Player"), *NewPlayer->GetName());
            PrimaryPlayer = NewPlayer;
        }

        NotifyPlayerAdded(NewPlayer);
    });

    GameInstance->OnLocalPlayerRemovedEvent.AddWeakLambda(this, [this](ULocalPlayer* ExistingPlayer)
    {
        if (PrimaryPlayer == ExistingPlayer)
        {
            //TODO: do we want to fall back to another player?
            PrimaryPlayer.Reset();
            UE_LOG(LogTurnBasedUI, Log, TEXT("RemoveLocalPlayer: Unsetting Primary Player from %s"), *ExistingPlayer->GetName());
        }

        NotifyPlayerRemoved(ExistingPlayer);
    });
    
    K2_Initialize(Collection);
}

void UTurnBasedUIManagerSubsystem::Deinitialize()
{
    Super::Deinitialize();
    SwitchToPolicy(nullptr);
    K2_Deinitialize();
}

bool UTurnBasedUIManagerSubsystem::ShouldCreateSubsystem(UObject* Outer) const
{
    return K2_ShouldCreateSubsystem(Outer);
}

void UTurnBasedUIManagerSubsystem::NotifyPlayerAdded_Implementation(ULocalPlayer* LocalPlayer)
{
    if (ensure(LocalPlayer != nullptr) && CurrentPolicy != nullptr)
    {
        CurrentPolicy->NotifyPlayerAdded(LocalPlayer);
    }
}

void UTurnBasedUIManagerSubsystem::NotifyPlayerRemoved_Implementation(ULocalPlayer* LocalPlayer)
{
    if (LocalPlayer != nullptr && CurrentPolicy != nullptr)
    {
        CurrentPolicy->NotifyPlayerRemoved(LocalPlayer);
    }
}

void UTurnBasedUIManagerSubsystem::NotifyPlayerDestroyed_Implementation(ULocalPlayer* LocalPlayer)
{
    if (LocalPlayer != nullptr && CurrentPolicy != nullptr)
    {
        CurrentPolicy->NotifyPlayerDestroyed(LocalPlayer);
    }
}

void UTurnBasedUIManagerSubsystem::SwitchToPolicy(UTurnBasedUIPolicy* NewPolicy)
{
    if (CurrentPolicy != NewPolicy)
    {
        CurrentPolicy = NewPolicy;
    }
}

bool UTurnBasedUIManagerSubsystem::K2_ShouldCreateSubsystem_Implementation(UObject* Outer) const
{
    if (!CastChecked<UGameInstance>(Outer)->IsDedicatedServerInstance())
    {
        TArray<UClass*> ChildClasses;
        GetDerivedClasses(GetClass(), ChildClasses, false);

        // Only create an instance if there is no override implementation defined elsewhere
        return ChildClasses.Num() == 0;
    }

    return false;
}
