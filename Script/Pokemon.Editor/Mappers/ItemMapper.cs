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
    [MapPropertyFromSource(nameof(ItemInfo.Price), Use = nameof(MapPrice))]
    [MapProperty(nameof(ItemInitializer.PriceToSell), nameof(ItemInfo.SellPrice))]
    private static partial ItemInfo ToItemInfo(this ItemInitializer item);

    [MapProperty(nameof(ItemInfo.PortionDisplayName), nameof(ItemInitializer.DisplayNamePortion))]
    [MapProperty(nameof(ItemInfo.PortionDisplayNamePlural), nameof(ItemInitializer.DisplayNamePortionPlural))]
    [MapProperty(nameof(ItemInfo.SellPrice), nameof(ItemInitializer.PriceToSell))]
    [MapPropertyFromSource(nameof(ItemInitializer.CanSell), Use = nameof(GetCanSell))]
    [MapPropertyFromSource(nameof(ItemInitializer.ShouldShowQuantity), Use = nameof(GetShowQuality))]
    [MapPropertyFromSource(nameof(ItemInitializer.IsConsumable), Use = nameof(GetConsumable))]
    private static partial ItemInitializer ToItemInitializer(this ItemInfo item, UObject? outer = null);
    
    private static FText? ToNullableText(this Option<FText> value) => value.Match<FText?>(v => v, () => null);

    private static Option<FText> ToTextOption(this FText? value) => value ?? Option<FText>.None;
    
    private static int? ToNullableInt(this Option<int> value) => value.ToNullable();

    private static Option<int> ToIntOption(this int? value) => value.ToOption();

    private static FGameplayTag ValueOrDefault(this FGameplayTag? value) => value ?? default;

    private static int MapPrice(ItemInitializer item)
    {
        return item.CanSell ? item.Price : 0;
    }
    
    private static bool GetCanSell(ItemInfo item)
    {
        return item.Price > 0 || item.SellPrice > 0;
    }
    
    private static bool GetShowQuality(ItemInfo item)
    {
        return item.ShouldShowQuantity ?? (item.FieldUse is not EFieldUse.TM and not EFieldUse.HM &&
                                           !item.Tags.HasTag(GameplayTags.Pokemon_Metadata_Items_KeyItem));
    }
    
    private static bool GetConsumable(ItemInfo item)
    {
        return item.IsConsumable ?? (item.FieldUse is not EFieldUse.TM and not EFieldUse.HM &&
                                     !item.Tags.HasTag(GameplayTags.Pokemon_Metadata_Items_KeyItem));
    }
}
