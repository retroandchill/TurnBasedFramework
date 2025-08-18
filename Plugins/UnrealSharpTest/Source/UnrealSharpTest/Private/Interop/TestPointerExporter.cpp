// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/TestPointerExporter.h"

void UTestPointerExporter::CopyWeakPtr(const TWeakPtr<FAutomationTestBase>& Source, TWeakPtr<FAutomationTestBase>& Dest)
{
    std::construct_at(&Dest, Source); 
}

void UTestPointerExporter::ReleaseWeakPtr(TWeakPtr<FAutomationTestBase>& Source)
{
    std::destroy_at(&Source);
}
