// Fill out your copyright notice in the Description page of Project Settings.


#include "DataRetrieval/DataTableProxy.h"

namespace GameDataAccess {
  const UScriptStruct* FDataTableProxy::GetExpectedStructType() const {
    return DataTable->GetRowStruct(); 
  }

  bool FDataTableProxy::ContainsKey(const FName ID) const {
    return DataTable->GetRowMap().Contains(ID);
  }

  int32 FDataTableProxy::GetNumRows() const {
    return DataTable->GetRowMap().Num();
  }

  const void* FDataTableProxy::GetData(const FName ID) const {
    return DataTable->FindRowUnchecked(ID);
  }

  bool FDataTableProxy::FEnumerator::MoveNext() {
    if (!IsValid()) {
      return false;
    }

    ++Iterator;
    return IsValid();
  }

  TPair<FName, const void*> FDataTableProxy::FEnumerator::Current() const {
    return *Iterator;
  }

  bool FDataTableProxy::FEnumerator::IsValid() const {
    return static_cast<bool>(Iterator);
  }
}
