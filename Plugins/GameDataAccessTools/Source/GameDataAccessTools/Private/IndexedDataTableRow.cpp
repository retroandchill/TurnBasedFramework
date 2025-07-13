// Fill out your copyright notice in the Description page of Project Settings.


#include "IndexedDataTableRow.h"


void FIndexedTableRow::OnDataTableChanged(const UDataTable* InDataTable, const FName InRowName) {
    ID = InRowName;
    RowID = InDataTable->GetRowNames().IndexOfByKey(ID) + 1;
}
