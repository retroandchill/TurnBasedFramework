// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "TurnBasedUIPolicy.generated.h"

class UPrimaryGameLayout;
class UTurnBasedUIManagerSubsystem;

UENUM()
enum class ELocalMultiplayerInteractionMode : uint8
{
    // Fullscreen viewport for the primary player only, regardless of the other player's existence
    PrimaryOnly,

    // Fullscreen viewport for one player, but players can swap control over who's is displayed and who's is dormant
    SingleToggle,

    // Viewports displayed simultaneously for both players
    Simultaneous
};

USTRUCT()
struct FRootViewportLayoutInfo
{
    GENERATED_BODY()
    
    UPROPERTY(Transient)
    TObjectPtr<ULocalPlayer> LocalPlayer;

    UPROPERTY(Transient)
    TObjectPtr<UPrimaryGameLayout> RootLayout;

    UPROPERTY(Transient)
    bool bAddedToViewport = false;

    FRootViewportLayoutInfo() = default;
    FRootViewportLayoutInfo(ULocalPlayer* InLocalPlayer, UPrimaryGameLayout* InRootLayout, const bool bIsInViewport)
        : LocalPlayer(InLocalPlayer)
        , RootLayout(InRootLayout)
        , bAddedToViewport(bIsInViewport)
    {}

    bool operator==(const ULocalPlayer* OtherLocalPlayer) const { return LocalPlayer == OtherLocalPlayer; }
};

/**
 * 
 */
UCLASS(MinimalAPI, Abstract, Blueprintable, Within = TurnBasedUIManagerSubsystem)
class UTurnBasedUIPolicy : public UObject
{
    GENERATED_BODY()

public:
    UTurnBasedUIPolicy() = default;
    
    template <std::derived_from<UTurnBasedUIPolicy> PolicyClass = UTurnBasedUIPolicy>
    static PolicyClass* GetInstance(const UObject* WorldContextObject)
    {
        return Cast<PolicyClass>(GetInstance(WorldContextObject));
    }

    UFUNCTION(meta = (WorldContext = "WorldContextObject", ScriptMethod))
    static UTurnBasedUIPolicy* GetInstance(const UObject* WorldContextObject);

    TURNBASEDUI_API UWorld* GetWorld() const override;

    TURNBASEDUI_API UTurnBasedUIManagerSubsystem* GetOwner() const;
    
    TURNBASEDUI_API UPrimaryGameLayout *GetRootLayout(const ULocalPlayer* Player) const;

    TURNBASEDUI_API ELocalMultiplayerInteractionMode GetLocalMultiplayerInteractionMode() const { return LocalMultiplayerInteractionMode; }

    TURNBASEDUI_API void RequestPrimaryControl(UPrimaryGameLayout* Layout);

protected:
    UFUNCTION(meta = (ScriptMethod))
    TURNBASEDUI_API void AddLayoutToViewport(ULocalPlayer* LocalPlayer, UPrimaryGameLayout* Layout);
    
    UFUNCTION(meta = (ScriptMethod))
    TURNBASEDUI_API void RemoveLayoutFromViewport(ULocalPlayer* LocalPlayer, UPrimaryGameLayout* Layout);
    
    UFUNCTION(BlueprintNativeEvent, Category = "UI Policy")
    TURNBASEDUI_API void OnRootLayoutAddedToViewport(ULocalPlayer* LocalPlayer, UPrimaryGameLayout* Layout);
    
    UFUNCTION(BlueprintNativeEvent, Category = "UI Policy")
    TURNBASEDUI_API void OnRootLayoutRemovedFromViewport(ULocalPlayer* LocalPlayer, UPrimaryGameLayout* Layout);

    
    UFUNCTION(BlueprintNativeEvent, Category = "UI Policy")
    TURNBASEDUI_API void OnRootLayoutReleased(ULocalPlayer* LocalPlayer, UPrimaryGameLayout* Layout);

    UFUNCTION(meta = (ScriptMethod))
    TURNBASEDUI_API void CreateLayoutWidget(ULocalPlayer* LocalPlayer);
    
    UFUNCTION(meta = (ScriptMethod))
    TURNBASEDUI_API TSubclassOf<UPrimaryGameLayout> GetLayoutWidgetClass() const;

private:
    TURNBASEDUI_API void NotifyPlayerAdded(ULocalPlayer* LocalPlayer);
    TURNBASEDUI_API void NotifyPlayerRemoved(ULocalPlayer* LocalPlayer);
    TURNBASEDUI_API void NotifyPlayerDestroyed(ULocalPlayer* LocalPlayer);

    friend class UTurnBasedUIManagerSubsystem;
    
    ELocalMultiplayerInteractionMode LocalMultiplayerInteractionMode = ELocalMultiplayerInteractionMode::PrimaryOnly;

    UPROPERTY(EditAnywhere)
    TSoftClassPtr<UPrimaryGameLayout> LayoutClass;

    UPROPERTY(Transient)
    TArray<FRootViewportLayoutInfo> RootViewportLayouts;
};
