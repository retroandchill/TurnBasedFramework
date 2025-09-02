// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonActivatableWidget.h"
#include "SelectableWidget.generated.h"

class UTurnBasedButtonGroup;
class UCommonButtonGroupBase;

/**
 * 
 */
UCLASS(Abstract)
class TURNBASEDUI_API USelectableWidget : public UCommonActivatableWidget
{
    GENERATED_BODY()

protected:
    void NativePreConstruct() override;

    UFUNCTION(BlueprintPure, BlueprintInternalUseOnly)
    UTurnBasedButtonGroup* GetButtons() const
    {
        return Buttons.Get();
    }

public:
    UWidget* NativeGetDesiredFocusTarget() const override;

    const TOptional<int32>& GetDesiredFocusIndex() const
    {
        return DesiredFocusIndex;
    }
    
    UFUNCTION(BlueprintCallable, Category = "Selection")
    bool TryGetDesiredFocusIndex(int32& OutIndex) const;

    UFUNCTION(BlueprintCallable, Category = "Selection")
    void SetDesiredFocusIndex(int32 Index);

    UFUNCTION(BlueprintCallable, Category = "Selection")
    void ClearDesiredFocusIndex()
    {
        DesiredFocusIndex.Reset();
    }

private:
    UPROPERTY(BlueprintGetter = GetButtons, Category = "Selection")
    TObjectPtr<UTurnBasedButtonGroup> Buttons;

    UPROPERTY(EditAnywhere, Getter, Category = "Selection", meta = (ClampMin = 0, UIMin = 0))
    TOptional<int32> DesiredFocusIndex;
};
