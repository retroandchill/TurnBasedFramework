using System.Diagnostics.CodeAnalysis;
using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

public abstract class GrowthRate
{
    public static int MaxLevel => UObject.GetDefault<UGameDataSettings>().MaxLevel;
    
    public abstract FGameplayTag Key { get; }
    
    public abstract int GetMinimumExpForLevel(int level);
    
    public int MaximumExp => GetMinimumExpForLevel(MaxLevel);

    public int AddExp(int exp1, int exp2) => Math.Clamp(exp1 + exp2, 0, MaximumExp);

    public int GetLevelForExp(int exp)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(exp, 0);
        var max = MaxLevel;
        if (exp >= MaximumExp) return max;

        for (var i = 0; i <= max; i++)
        {
            if (exp < GetMinimumExpForLevel(i)) return i - 1;
        }

        return max;
    }
}

public sealed class MediumExpGrowth : GrowthRate
{
    public override FGameplayTag Key { get; }

    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3);
    }
}

public class GrowthRateContainer(IEnumerable<GrowthRate> growthRates)
{
    private readonly Dictionary<FGameplayTag, GrowthRate> _growthRates = growthRates.ToDictionary(x => x.Key);
    
    public GrowthRate GetGrowthRate(FGameplayTag key)
    {
        return _growthRates[key];
    }

    public bool TryGetGrowthRate(FGameplayTag key, [NotNullWhen(true)] out GrowthRate? growthRate)
    {
        return _growthRates.TryGetValue(key, out growthRate);
    }
}

[UClass(ClassFlags.Abstract)]
public class UGrowthRateFormula : UObject
{
    [UFunction(FunctionFlags.BlueprintEvent)] 
    public virtual int GetMinimumExpForLevel(int level) 
    {
        throw new NotImplementedException();
    }
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UGrowthRate : UObject, IGameDataEntry
{
    public static int MaxLevel => GetDefault<UGameDataSettings>().MaxLevel;
    public const string TagCategory = "Pokemon.Data.GrowthRates";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [Categories(TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
}