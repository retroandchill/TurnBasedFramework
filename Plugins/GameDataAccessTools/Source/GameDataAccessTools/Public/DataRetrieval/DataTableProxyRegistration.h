// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"

namespace GameDataAccess {
  /**
   * 
   */
  class FDataTableProxyRegistration {
  public:
    explicit FDataTableProxyRegistration(const FGameplayTag ConfigurationTag);
    virtual ~FDataTableProxyRegistration() = default;

    virtual const UScriptStruct* GetExpectedStructType() = 0;

    FGameplayTag GetConfigurationTag() const {
      return ConfigurationTag;
    }

  private:
    FGameplayTag ConfigurationTag;
  };

  template <typename T>
  concept ValidDataTableStruct = std::derived_from<T, FTableRowBase> && requires {
    { T::ScriptStruct() } -> std::same_as<UScriptStruct*>;
  };

  template <ValidDataTableStruct T>
  class TNativeDataTableProxyRegistration final : public FDataTableProxyRegistration {
  public:
    explicit TNativeDataTableProxyRegistration(const FGameplayTag ConfigurationTag) : FDataTableProxyRegistration(ConfigurationTag) {}

    const UScriptStruct* GetExpectedStructType() override {
      return T::ScriptStruct();
    }
  };

  class FDynamicDataTableProxyRegistration final : public FDataTableProxyRegistration {
  public:
    explicit FDynamicDataTableProxyRegistration(const FGameplayTag& ConfigurationTag, const UScriptStruct* ScriptStruct);

    const UScriptStruct* GetExpectedStructType() override;

  private:
    TStrongObjectPtr<const UScriptStruct> ScriptStruct;
  };
}