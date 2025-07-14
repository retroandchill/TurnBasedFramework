// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"

namespace GameDataAccess {
  /**
   * Basic interface to manage getting data from a data table
   */
  class GAMEDATAACCESSTOOLS_API FDataTableProxy {
  public:
    explicit FDataTableProxy(UDataTable* DataTable) : DataTable(DataTable) {
    }

    const UScriptStruct* GetExpectedStructType() const;

    bool ContainsKey(const FName ID) const;

    int32 GetNumRows() const;

    /**
     * Get the specified row from the data table
     * @param ID he ID to get the data from
     * @return The retrieved row from the database
     */
    const void* GetData(const FName ID) const;

    struct FEnumerator {
      using FIterator = TMap<FName, uint8*>::TConstIterator;
      
      explicit FEnumerator(const FDataTableProxy& Owner) : Iterator(Owner.DataTable->GetRowMap().CreateConstIterator()) {
        
      }

      bool MoveNext();

      TPair<FName, const void*> Current() const;

      bool IsValid() const;
      
    private:
      FIterator Iterator;
    };

  private:
    /**
     * A pointer to the data table asset that this proxy object contains
     */
    TStrongObjectPtr<UDataTable> DataTable;
  };
}
