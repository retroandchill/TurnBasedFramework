// Fill out your copyright notice in the Description page of Project Settings.


#include "Components/SelectableWidget.h"

#include "Components/TurnBasedButtonGroup.h"

void USelectableWidget::NativePreConstruct()
{
    Super::NativePreConstruct();

#if WITH_EDITOR
    if (Buttons != nullptr)
    {
        Buttons->RemoveAll();
    }
#endif
    
    Buttons = NewObject<UTurnBasedButtonGroup>(this);

    Buttons->NativeOnButtonBaseClicked.AddWeakLambda(this, [this](UCommonButtonBase*, const int32 Index)
    {
        DesiredFocusIndex = Index;
    });
}

UWidget* USelectableWidget::NativeGetDesiredFocusTarget() const
{
    const int32 FocusIndex = DesiredFocusIndex.Get(INDEX_NONE);
    if (auto *ButtonAtIndex = Buttons->GetButtonBaseAtIndex(FocusIndex); ButtonAtIndex != nullptr)
    {
        return ButtonAtIndex;
    }
    
    return Super::NativeGetDesiredFocusTarget();
}

bool USelectableWidget::TryGetDesiredFocusIndex(int32& OutIndex) const
{
    if (DesiredFocusIndex.IsSet())
    {
        OutIndex = DesiredFocusIndex.GetValue();
        return true;
    }

    OutIndex = INDEX_NONE;
    return false;
}

void USelectableWidget::SetDesiredFocusIndex(const int32 Index)
{
    DesiredFocusIndex.Emplace(FMath::Clamp(Index, 0, Buttons->GetButtonCount() - 1));
}
