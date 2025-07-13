// Fill out your copyright notice in the Description page of Project Settings.


#include "DataRetrieval/DataTableProxy.h"

namespace GameDataAccess {
  TArray<FName> FDataTableProxy::GetTableRowNames() const {
    return DataTable->GetRowNames();
  }

  bool FDataTableProxy::IsRowNameValid(const FName ID) const {
    return DataTable->GetRowMap().Contains(ID);
  }

  const UScriptStruct* FOpaqueDataTableProxy::GetStructType() const {
    return GetDataTable()->GetRowStruct();
  }

  const void* FOpaqueDataTableProxy::GetData(const FName ID) const {
    return GetDataTable()->FindRowUnchecked(ID);
  }
}