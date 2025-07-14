using UnrealSharp.Attributes;
using UnrealSharp.UnrealSharpCore;

namespace ManagedGameDataAccessTools;

[UClass(ClassFlags.Config, DisplayName = "Data Retrieval", ConfigCategory = "Game")]
public class UDataRetrievalSettings : UCSDeveloperSettings
{
    [UProperty(PropertyFlags.EditDefaultsOnly | PropertyFlags.Config)] 
    public bool EnableDataRetrieval { get; set; }
}