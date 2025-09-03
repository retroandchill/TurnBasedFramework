using Microsoft.Extensions.DependencyInjection;
using Pokemon.Core.Characters;
using Pokemon.Core.Executor.Display;
using Pokemon.Core.Executor.Exp;
using UnrealInject.Subsystems;
using UnrealSharp.Attributes;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;
using UnrealSharp.UnrealSharpCore;

namespace Pokemon.Core;

[UClass]
public class UPokemonSubsystem : UCSGameInstanceSubsystem
{
    private readonly Dictionary<FGameplayTag, IExpGrowthFormula> _expGrowthFormulas = new();

    public IDisplayService DisplayActions { get; private set; } = null!;
    
    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Player")]
    public UTrainer Player { get; private set; }

    protected override void Initialize(FSubsystemCollectionBaseRef collection)
    {
        var subsystem =
            collection.InitializeRequiredSubsystem<UDependencyInjectionGameInstanceSubsystem>();
        foreach (var expGrowthFormula in subsystem.GetServices<IExpGrowthFormula>())
        {
            _expGrowthFormulas.Add(expGrowthFormula.GrowthRateFor, expGrowthFormula);
        }

        DisplayActions = subsystem.GetService<IDisplayService>() ?? new NullDisplayService();
    }

    protected override void Deinitialize()
    {
        _expGrowthFormulas.Clear();
        DisplayActions = null!;
    }

    public IExpGrowthFormula GetExpGrowthFormula(FGameplayTag growthRate)
    {
        return _expGrowthFormulas.TryGetValue(growthRate, out var formula)
            ? formula
            : throw new InvalidOperationException($"No formula for growth rate {growthRate}");
    }
}
