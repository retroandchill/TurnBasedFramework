// Fill out your copyright notice in the Description page of Project Settings.


#include "DataRetrieval/DataTableProxyRegistration.h"

namespace GameDataAccess {
  FDataTableProxyRegistration::FDataTableProxyRegistration(const FGameplayTag ConfigurationTag): ConfigurationTag(ConfigurationTag) {}

  FDynamicDataTableProxyRegistration::FDynamicDataTableProxyRegistration(const FGameplayTag& ConfigurationTag, const UScriptStruct* ScriptStruct)
      : FDataTableProxyRegistration(ConfigurationTag), ScriptStruct(ScriptStruct) {
  }

  const UScriptStruct* FDynamicDataTableProxyRegistration::GetExpectedStructType() {
    return ScriptStruct.Get();
  }
}
