using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;

namespace PokemonData;

public partial class UGameDataSettings
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditDefaultsOnly)]
    [ClampMin("1")]
    public int MaxLevel { get; init; } = 100;
}