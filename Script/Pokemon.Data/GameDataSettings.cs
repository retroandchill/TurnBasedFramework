using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;

namespace Pokemon.Data;

public partial class UGameDataSettings
{
    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditDefaultsOnly,
        Category = "Gameplay"
    )]
    [ClampMin("1")]
    public int MaxLevel { get; init; } = 100;

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditDefaultsOnly,
        Category = "Gameplay"
    )]
    public bool MoveCategoryPerMove { get; init; } = false;
}
