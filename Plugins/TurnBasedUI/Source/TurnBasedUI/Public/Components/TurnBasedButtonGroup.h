// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Groups/CommonButtonGroupBase.h"
#include "TurnBasedButtonGroup.generated.h"

class UCommonActivatableWidget;
DECLARE_MULTICAST_DELEGATE_TwoParams(FPlaceButton, int32, UCommonButtonBase*);

/**
 * 
 */
UCLASS()
class TURNBASEDUI_API UTurnBasedButtonGroup : public UCommonButtonGroupBase
{
    GENERATED_BODY()

public:
    FDelegateHandle BindToPlaceButton(FPlaceButton::FDelegate Delegate)
    {
        return OnPlaceButton.Add(MoveTemp(Delegate));   
    }

    void UnbindFromPlaceButton(const FDelegateHandle Handle)
    {
        OnPlaceButton.Remove(Handle);
    }
    
protected:
    void OnWidgetAdded(UWidget* Widget) override;
    void OnWidgetRemoved(UWidget* Widget) override;

private:
    FPlaceButton OnPlaceButton;
};
