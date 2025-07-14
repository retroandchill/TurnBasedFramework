// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/DataTableProxyExporter.h"
#include "DataRetrieval/DataTableProxy.h"

void UDataTableProxyExporter::InitDataTableProxy(GameDataAccess::FDataTableProxy& Proxy, UDataTable* DataTable) {
  std::construct_at(std::addressof(Proxy), DataTable);
}

void UDataTableProxyExporter::DeinitDataTableProxy(GameDataAccess::FDataTableProxy& Proxy) {
  std::destroy_at(std::addressof(Proxy));
}

const UScriptStruct* UDataTableProxyExporter::GetScriptStruct(const UDataTable* DataTable) {
  check(DataTable != nullptr);
  return DataTable->GetRowStruct();
}

bool UDataTableProxyExporter::ContainsKey(const GameDataAccess::FDataTableProxy& Proxy, FName Key) {
  return Proxy.ContainsKey(Key);
}

int32 UDataTableProxyExporter::GetNumRows(const GameDataAccess::FDataTableProxy& Proxy) {
  return Proxy.GetNumRows();
  
}

const void* UDataTableProxyExporter::GetDataFromRow(const GameDataAccess::FDataTableProxy& Proxy, FName Key) {
  return Proxy.GetData(Key);
}

bool UDataTableProxyExporter::InitializeNativeIterator(GameDataAccess::FDataTableProxy::FEnumerator& Iterator, int32 BufferSize,
                                                       const GameDataAccess::FDataTableProxy& Proxy) {
  static_assert(std::is_trivially_destructible_v<GameDataAccess::FDataTableProxy::FEnumerator>);
  if (sizeof(GameDataAccess::FDataTableProxy::FEnumerator) > BufferSize) {
    return false;
  }

  std::construct_at(std::addressof(Iterator), Proxy);
  return true;
}

bool UDataTableProxyExporter::MoveNextNativeIterator(GameDataAccess::FDataTableProxy::FEnumerator& Iterator) {
  return Iterator.MoveNext();
}

bool UDataTableProxyExporter::GetNativeIteratorValue(GameDataAccess::FDataTableProxy::FEnumerator& Iterator, FName& Key,
  const void*& Value) {
  if (!Iterator.IsValid()) {
    return false;
  }

  auto [CurrentKey, CurrentValue] = Iterator.Current();
  Key = CurrentKey;
  Value = CurrentValue;
  return true;
}
