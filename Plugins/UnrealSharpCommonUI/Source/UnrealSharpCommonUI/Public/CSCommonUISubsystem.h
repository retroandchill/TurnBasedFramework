// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "Subsystems/GameInstanceSubsystem.h"
#include "CSCommonUISubsystem.generated.h"

class UWidget;
class FCSInputBindingCallbacks;
/**
 * 
 */
UCLASS(NotBlueprintType)
class UNREALSHARPCOMMONUI_API UCSCommonUISubsystem : public UGameInstanceSubsystem, public FUObjectArray::FUObjectDeleteListener
{
    GENERATED_BODY()

public:
    void Deinitialize() override;

    TSharedRef<FCSInputBindingCallbacks> BindInputActionCallbacks(UWidget* InObject, const FGCHandle& OnExecuteAction, const FGCHandle& OnHoldActionPressed,
        const FGCHandle& OnHoldActionReleased, const FGCHandle& OnHoldActionProgressed);

    void NotifyUObjectDeleted(const UObjectBase* Object, int32 Index) override;
    void OnUObjectArrayShutdown() override;

private:
    TMap<uint32, TArray<TSharedRef<FCSInputBindingCallbacks>>> CallbackBindings;
};
