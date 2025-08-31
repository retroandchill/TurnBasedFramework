// Fill out your copyright notice in the Description page of Project Settings.


#include "Components/TurnBasedActionWidget.h"

#include "CommonInputBaseTypes.h"
#include "CommonInputSubsystem.h"
#include "EnhancedInputSubsystems.h"

FSlateBrush UTurnBasedActionWidget::GetIcon() const
{
    if (AssociatedInputAction == nullptr)
    {
        if (const auto* EnhancedInputSubsystem = GetEnhancedInputSubsystem(); EnhancedInputSubsystem != nullptr)
        {
            TArray<FKey> BoundKeys = EnhancedInputSubsystem->QueryKeysMappedToAction(AssociatedInputAction);

            const auto* CommonInputSubsystem = GetInputSubsystem();
            if (FSlateBrush SlateBrush; !BoundKeys.IsEmpty() && CommonInputSubsystem
                && UCommonInputPlatformSettings::Get()->TryGetInputBrush(SlateBrush, BoundKeys[0],
                    CommonInputSubsystem->GetCurrentInputType(), CommonInputSubsystem->GetCurrentGamepadName()))
            {
                return SlateBrush;
            }
        }
    }

    
    return Super::GetIcon();
}

UEnhancedInputLocalPlayerSubsystem* UTurnBasedActionWidget::GetEnhancedInputSubsystem() const
{
    const UWidget* BoundWidget = DisplayedBindingHandle.GetBoundWidget();
    if (const auto* BindingOwner = BoundWidget ? BoundWidget->GetOwningLocalPlayer() : GetOwningLocalPlayer(); BindingOwner != nullptr)
    {
        return BindingOwner->GetSubsystem<UEnhancedInputLocalPlayerSubsystem>();
    }
    return nullptr;
}
