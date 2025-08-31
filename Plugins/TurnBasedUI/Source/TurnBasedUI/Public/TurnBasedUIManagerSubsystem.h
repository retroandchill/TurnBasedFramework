// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Extensions/Subsystems/SubsystemCollectionBaseRef.h"
#include "Kismet/GameplayStatics.h"
#include "TurnBasedUIManagerSubsystem.generated.h"

class UTurnBasedUIPolicy;
/**
 * 
 */
UCLASS(MinimalAPI, Abstract, Config = Game)
class UTurnBasedUIManagerSubsystem : public UGameInstanceSubsystem
{
    GENERATED_BODY()

public:
    UTurnBasedUIManagerSubsystem() = default;

    TURNBASEDUI_API void Initialize(FSubsystemCollectionBase& Collection) override;
    TURNBASEDUI_API void Deinitialize() override;
    TURNBASEDUI_API bool ShouldCreateSubsystem(UObject* Outer) const override;

    UFUNCTION(meta = (ScriptMethod, WorldContext = WorldContextObject))
    static UTurnBasedUIManagerSubsystem* GetInstance(const UObject* WorldContextObject)
    {
        return UGameplayStatics::GetGameInstance(WorldContextObject)->GetSubsystem<UTurnBasedUIManagerSubsystem>();
    }

    UFUNCTION(BlueprintPure, BlueprintInternalUseOnly)
    UTurnBasedUIPolicy* GetCurrentUIPolicy() { return CurrentPolicy; }
    
    const UTurnBasedUIPolicy* GetCurrentUIPolicy() const { return CurrentPolicy; }
    
    UFUNCTION(BlueprintNativeEvent, Category = "UI Manager")
    TURNBASEDUI_API void NotifyPlayerAdded(ULocalPlayer* LocalPlayer);

    UFUNCTION(BlueprintNativeEvent, Category = "UI Manager")
    TURNBASEDUI_API void NotifyPlayerRemoved(ULocalPlayer* LocalPlayer);

    UFUNCTION(BlueprintNativeEvent, Category = "UI Manager")
    TURNBASEDUI_API void NotifyPlayerDestroyed(ULocalPlayer* LocalPlayer);

protected:
    UFUNCTION(BlueprintCallable, Category = "UI Manager")
    TURNBASEDUI_API void SwitchToPolicy(UTurnBasedUIPolicy* NewPolicy);

    UFUNCTION(BlueprintNativeEvent, meta = (ScriptName = "ShouldCreateSubsystem"), Category = "Managed Subsystems")
    bool K2_ShouldCreateSubsystem(UObject* Outer) const;

    UFUNCTION(BlueprintImplementableEvent, meta = (ScriptName = "Initialize"), Category = "Managed Subsystems")
    void K2_Initialize(FSubsystemCollectionBaseRef Collection);

    UFUNCTION(BlueprintImplementableEvent, meta = (ScriptName = "Deinitialize"), Category = "Managed Subsystems")
    void K2_Deinitialize();

private:
    TWeakObjectPtr<ULocalPlayer> PrimaryPlayer;
    
    UPROPERTY(Transient, BlueprintGetter = GetCurrentUIPolicy, Category = "UI Manager")
    TObjectPtr<UTurnBasedUIPolicy> CurrentPolicy = nullptr;

    UPROPERTY(Config, EditAnywhere)
    TSoftClassPtr<UTurnBasedUIPolicy> DefaultUIPolicyClass;
};
