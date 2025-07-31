using LanguageExt;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class ItemMapper
{
    public static UItem ToItem(this ItemInfo itemInfo, UObject? outer = null)
    {
        return itemInfo.ToItemInitializer(outer);
    }

    public static ItemInfo ToItemInfo(this UItem item) => ItemInitializer.From(item).ToItemInfo();

    [MapProperty(nameof(ItemInitializer.DisplayNamePortion), nameof(ItemInfo.PortionDisplayName))]
    [MapProperty(nameof(ItemInitializer.DisplayNamePortionPlural), nameof(ItemInfo.PortionDisplayNamePlural))]
    [MapProperty(nameof(ItemInitializer.PriceToSell), nameof(ItemInfo.SellPrice))]
    private static partial ItemInfo ToItemInfo(this ItemInitializer item);

    [MapProperty(nameof(ItemInfo.PortionDisplayName), nameof(ItemInitializer.DisplayNamePortion))]
    [MapProperty(nameof(ItemInfo.PortionDisplayNamePlural), nameof(ItemInitializer.DisplayNamePortionPlural))]
    [MapProperty(nameof(ItemInfo.SellPrice), nameof(ItemInitializer.PriceToSell))]
    private static partial ItemInitializer ToItemInitializer(this ItemInfo item, UObject? outer = null);
    
    private static FText? ToNullableText(this Option<FText> value) => value.Match<FText?>(v => v, () => null);

    private static Option<FText> ToTextOption(this FText? value) => value ?? Option<FText>.None;
    
    private static int? ToNullableInt(this Option<int> value) => value.ToNullable();

    private static Option<int> ToIntOption(this int? value) => value.ToOption();
    
    private static int ValueOrDefault(this int? value) => value ?? 0;
    private static FGameplayTag ValueOrDefault(this FGameplayTag? value) => value ?? FGameplayTag.None;
}
