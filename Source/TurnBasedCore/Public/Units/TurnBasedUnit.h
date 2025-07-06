// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "TurnBasedUnit.generated.h"

class UTurnBasedUnitComponent;

/**
 * 
 */
UCLASS()
class TURNBASEDCORE_API UTurnBasedUnit : public UObject {
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintPure, Category = "TurnBased|Units",
        meta = (DeterminesOutputType = "ComponentClass", DynamicOutputParam = "ReturnValue", Nullable))
    UTurnBasedUnitComponent* GetComponent(TSubclassOf<UTurnBasedUnitComponent> ComponentClass) const;

    UFUNCTION(BlueprintPure, Category = "TurnBased|Units",
        meta = (DeterminesOutputType = "ComponentClass", DynamicOutputParam = "Component"))
    bool TryGetComponent(TSubclassOf<UTurnBasedUnitComponent> ComponentClass,
        UPARAM(meta = (Nullable, NotNullWhen = true)) UTurnBasedUnitComponent*& Component) const;

private:
    UPROPERTY()
    TArray<UTurnBasedUnitComponent*> Components;
};
