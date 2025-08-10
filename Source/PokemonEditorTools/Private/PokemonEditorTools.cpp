#include "PokemonEditorTools.h"

#include "Customization/PokemonEditorSettingsCustomization.h"

#define LOCTEXT_NAMESPACE "FPokemonEditorToolsModule"

void FPokemonEditorToolsModule::StartupModule()
{
    auto &PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");
    PropertyModule.RegisterCustomClassLayout("PokemonEditorSettings_C",
                                             FOnGetDetailCustomizationInstance::CreateStatic(
                                                 &FPokemonEditorSettingsCustomization::MakeInstance));
}

void FPokemonEditorToolsModule::ShutdownModule()
{
    auto &PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");
    PropertyModule.UnregisterCustomClassLayout("PokemonEditorSettings_C");
}

#undef LOCTEXT_NAMESPACE
    
IMPLEMENT_MODULE(FPokemonEditorToolsModule, PokemonEditorTools)