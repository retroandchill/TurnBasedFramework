using Pokemon.Data.Pbs;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record MoveInfo
{
    public required FGameplayTag Id { get; init; }
    public required int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    
    public required FText Description { get; init; }
    
    public required FGameplayTag Type { get; init; }
    
    public required EDamageCategory Category { get; init; }
    
    public required EDamageType DamageType { get; init; }
    
    public int Power { get; init; } = 5;
    
    public bool AlwaysHits { get; init; } = false;
    
    public int Accuracy { get; init; } = 100;
    
    public int TotalPP { get; init; } = 1;
    
    public int Priority { get; init; }
    
    public FGameplayTag Target { get; init; }
    
    public FGameplayTag FunctionCode { get; init; }
    
    public bool GuaranteedEffect { get; init; } = true;
    
    public int EffectChance { get; init; } = 30;
    
    public FGameplayTagContainer Tags { get; init; }
}
