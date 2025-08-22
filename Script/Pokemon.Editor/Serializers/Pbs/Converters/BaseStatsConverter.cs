using System.Collections.Immutable;
using GameDataAccessTools.Core.DataRetrieval;
using Pokemon.Data;
using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Serializers.Pbs.Converters;

public sealed class BaseStatsConverter : PbsConverterBase<IReadOnlyDictionary<FGameplayTag, int>>
{
    private readonly ImmutableList<FGameplayTag> _statOrder;

    public BaseStatsConverter()
    {
        var settings = UObject.GetDefault<UGameDataSettings>();
        var statsRepository =
            ((TSoftObjectPtr<UObject>)settings.Stats).LoadSynchronous()
            as IGameDataRepository<UStat>;
        ArgumentNullException.ThrowIfNull(statsRepository);
        statsRepository.Refresh();
        _statOrder =
        [
            .. statsRepository
                .AllEntries.Where(x => x.IsMainStat)
                .OrderBy(x => x.PbsOrder)
                .Select(x => x.Id),
        ];
    }

    public override string WriteCsvValue(
        IReadOnlyDictionary<FGameplayTag, int> value,
        PbsScalarDescriptor schema,
        string? sectionName
    )
    {
        return string.Join(",", _statOrder.Select(x => value.GetValueOrDefault(x, 1)));
    }

    public override IReadOnlyDictionary<FGameplayTag, int> GetCsvValue(
        string input,
        PbsScalarDescriptor scalarDescriptor,
        string? sectionName
    )
    {
        var values = input.Split(',').Select(int.Parse).ToImmutableList();
        if (values.Count != _statOrder.Count)
        {
            throw new ArgumentException("Invalid number of values");
        }

        foreach (var value in values)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0, nameof(value));
        }

        return _statOrder
            .Zip(values, (x, y) => (Stat: x, Value: y))
            .ToImmutableDictionary(x => x.Stat, x => x.Value);
    }
}
