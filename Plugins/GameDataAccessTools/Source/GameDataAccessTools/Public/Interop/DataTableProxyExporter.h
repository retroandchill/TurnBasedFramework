// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "CSBindsManager.h"
#include "DataRetrieval/DataTableProxy.h"
#include "DataTableProxyExporter.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLS_API UDataTableProxyExporter : public UObject {
  GENERATED_BODY()

public:
  UNREALSHARP_FUNCTION()
  static void InitDataTableProxy(GameDataAccess::FDataTableProxy& Proxy, UDataTable* DataTable);

  UNREALSHARP_FUNCTION()
  static void DeinitDataTableProxy(GameDataAccess::FDataTableProxy& Proxy);

  UNREALSHARP_FUNCTION()
  static const UScriptStruct* GetScriptStruct(const UDataTable* DataTable);

  UNREALSHARP_FUNCTION()
  static bool ContainsKey(const GameDataAccess::FDataTableProxy& Proxy, FName Key);

  UNREALSHARP_FUNCTION()
  static int32 GetNumRows(const GameDataAccess::FDataTableProxy& Proxy);

  UNREALSHARP_FUNCTION()
  static const void* GetDataFromRow(const GameDataAccess::FDataTableProxy& Proxy, FName Key);

  UNREALSHARP_FUNCTION()
  static bool InitializeNativeIterator(GameDataAccess::FDataTableProxy::FEnumerator& Iterator, int32 BufferSize, const GameDataAccess::FDataTableProxy& Proxy);

  UNREALSHARP_FUNCTION()
  static bool MoveNextNativeIterator(GameDataAccess::FDataTableProxy::FEnumerator& Iterator);

  UNREALSHARP_FUNCTION()
  static bool GetNativeIteratorValue(GameDataAccess::FDataTableProxy::FEnumerator& Iterator, FName& Key, const void*& Value);
};
