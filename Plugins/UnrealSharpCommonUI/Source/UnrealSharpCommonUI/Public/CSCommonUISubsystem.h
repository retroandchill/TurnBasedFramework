// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Subsystems/GameInstanceSubsystem.h"
#include "CSCommonUISubsystem.generated.h"

class UWidget;
class FCSInputBindingCallback;

/**
 * 
 */
UCLASS(NotBlueprintType)
class UNREALSHARPCOMMONUI_API UCSCommonUISubsystem : public UGameInstanceSubsystem, public FUObjectArray::FUObjectDeleteListener
{
    GENERATED_BODY()

public:
    void Deinitialize() override;

    void RegisterInputBindingCallback(const UWidget* Widget, const ::FGuid& Id,
                                      const TSharedRef<FCSInputBindingCallback>& Callback);

    FCSInputBindingCallback* GetInputBindingCallback(const UWidget* Widget, const FGuid& Guid);

    void NotifyUObjectDeleted(const UObjectBase* Object, int32 Index) override;
    void OnUObjectArrayShutdown() override;

private:
    TMap<uint32, TMap<FGuid, TSharedPtr<FCSInputBindingCallback>>> InputBindingCallbacks;
};
