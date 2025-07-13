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
    virtual ~FDataTableProxy() = default;

    /**
     * Get the type of struct this proxy points to
     * @return The type of struct contained within the table
     */
    virtual const UScriptStruct* GetStructType() const = 0;

    /**
     * Get the specified row from the data table
     * @param ID he ID to get the data from
     * @return The retrieved row from the database
     */
    virtual const void* GetData(const FName ID) const = 0;

    /**
     * Get the list of IDs in the table
     * @return A unique set of row names
     */
    TArray<FName> GetTableRowNames() const;

    /**
     * Check if the provided row name is valid or not
     * @param ID The ID to check against
     * @return If there is a row defined with the provided ID
     */
    bool IsRowNameValid(const FName ID) const;

    /**
     * Get the underlying Data Table that holds the data
     * @return The actual Data Table asset
     */
    UDataTable* GetDataTable() const {
      return DataTable.Get();
    }

  private:
    /**
     * A pointer to the data table asset that this proxy object contains
     */
    TStrongObjectPtr<UDataTable> DataTable;
  };

  /**
   * Proxy class that stores a data table and allows the retrieval of properties from it
   * @tparam T The row type this proxy references
   */
  template <std::derived_from<FTableRowBase> T>
  class TDataTableProxy final : public FDataTableProxy {
  public:
    explicit TDataTableProxy(UDataTable* DataTable) : FDataTableProxy(DataTable) {
    }

    const UScriptStruct* GetStructType() const override {
      return T::StaticStruct();
    }

    const T* GetData(const FName ID) const override {
      return GetDataTable()->FindRow<T>(ID, TEXT("Find row!"));
    }

    const T& GetDataChecked(const FName ID) const {
      auto Data = GetDataTable()->FindRow<T>(ID, TEXT("Find row!"));
      check(Data != nullptr)
      return *Data;
    }
  };

  class FOpaqueDataTableProxy final : public FDataTableProxy {
  public:
    explicit FOpaqueDataTableProxy(UDataTable* DataTable) : FDataTableProxy(DataTable) {
    }

    const UScriptStruct* GetStructType() const override;
    const void* GetData(FName ID) const override;
  };
}