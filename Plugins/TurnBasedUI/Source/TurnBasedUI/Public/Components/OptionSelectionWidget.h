// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonActivatableWidget.h"
#include "OptionSelectionWidget.generated.h"

class UCommonButtonGroupBase;
class UCommonButtonStyle;
class UTurnBasedButtonBase;
class UCommonButtonBase;
class UInputAction;
class UTurnBasedButtonGroup;

USTRUCT(BlueprintType, meta = (NullableEnable, RecordStruct, ReadOnly, UseProperties))
struct FSelectableOption
{
    GENERATED_BODY()

    UPROPERTY(EditAnywhere, Category = "Selection", meta = (Required))
    FName Id;

    UPROPERTY(EditAnywhere, Category = "Selection")
    TOptional<FText> Text;

    UPROPERTY(EditAnywhere, Category = "Selection", meta = (Nullable))
    TObjectPtr<UInputAction> InputAction;
};

DECLARE_MULTICAST_DELEGATE_ThreeParams(FNativeOptionSelected, int32, FName, UTurnBasedButtonBase*);
DECLARE_DYNAMIC_MULTICAST_DELEGATE_ThreeParams(FOptionSelected, int32, Index, FName, SelectedId, UTurnBasedButtonBase*, Button);

/**
 * 
 */
UCLASS(Abstract)
class TURNBASEDUI_API UOptionSelectionWidget : public UCommonActivatableWidget
{
    GENERATED_BODY()

protected:
    void NativePreConstruct() override;
    void NativeDestruct() override;

public:
    const TArray<FSelectableOption>& GetOptions() const { return Options; }

    void SetOptions(TArray<FSelectableOption> NewOptions);

#if WITH_EDITOR
    EDataValidationResult IsDataValid(FDataValidationContext &Context) const override;
#endif

    FDelegateHandle BindToOptionSelected(FNativeOptionSelected::FDelegate Delegate);

    void UnbindFromOptionSelected(FDelegateHandle Handle);

    UPROPERTY(BlueprintAssignable, Category = "Events")
    FOptionSelected OnOptionSelected;

protected:
    UFUNCTION(BlueprintImplementableEvent, Category = "Selection")
    void PlaceOptionIntoWidget(int32 Index, FName Id, UTurnBasedButtonBase* Button);
    
private:
    void CreateOptions();

    UPROPERTY()
    TObjectPtr<UTurnBasedButtonGroup> Buttons;
    
    UPROPERTY(EditAnywhere, Getter, Setter, Category = "Selection")
    TArray<FSelectableOption> Options;

    UPROPERTY(EditAnywhere, Category = "Selection")
    TSubclassOf<UTurnBasedButtonBase> ButtonClass;

    UPROPERTY(EditAnywhere, Category = "Styles")
    TOptional<TSubclassOf<UCommonButtonStyle>> OverrideButtonStyle;

    FNativeOptionSelected NativeOptionSelectedDelegate;

    FDelegateHandle PlaceButtonsDelegateHandle;
    FDelegateHandle NativeOptionSelectedDelegateHandle;
};
