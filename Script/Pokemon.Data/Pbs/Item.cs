using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using LanguageExt;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Pbs;

[UEnum]
public enum EFieldUse : byte
{
    /// <summary>
    /// Not usable in the field
    /// </summary>
    NoFieldUse = 0,

    /// <summary>
    /// Used on a Pokémon
    /// </summary>
    OnPokemon = 1,

    /// <summary>
    /// Used directly from the bag
    /// </summary>
    Direct = 2,

    /// <summary>
    /// Teaches a Pokémon a move (resuable on newer mechanics)
    /// </summary>
    TM = 3,

    /// <summary>
    /// Teaches a Pokémon a move (reusable)
    /// </summary>
    HM = 4,

    /// <summary>
    /// Teaches a Pokémon a move (single-use)
    /// </summary>
    TR = 5
}

[UEnum]
public enum EBattleUse : byte {
    /// <summary>
    /// Not usable in battle
    /// </summary>
    NoBattleUse = 0,

    /// <summary>
    /// Usable on a Pokémon in the party
    /// </summary>
    OnPokemon = 1,

    /// <summary>
    /// Usable on a Pokémon in the party and requiring a move to be selected
    /// </summary>
    OnMove = 2,

    /// <summary>
    /// Usable on the active Pokémon in battle
    /// </summary>
    OnBattler = 3,

    /// <summary>
    /// Used on an opponent in battle
    /// </summary>
    OnFoe = 4,

    /// <summary>
    /// Used directly with no target selection
    /// </summary>
    Direct = 5
};

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UItem : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.Items";
    public const string PocketCategory = "Pokemon.Bag.Pocket";
    public const string MetadataCategory = "Pokemon.Metadata.Items";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [Categories(TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, 
        DisplayName = "Display Name (Plural)", Category = "Display")]
    public FText DisplayNamePlural { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, 
        DisplayName = "Display Name (Portion)", Category = "Display")]
    private Option<FText> DisplayNamePortion { get; init; }

    public FText PortionDisplayName
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Display")]
        get => DisplayNamePortion.Match(x => x, () => DisplayName);
    }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, 
        DisplayName = "Display Name (Plural Portion)", Category = "Display")]
    private Option<FText> DisplayNamePortionPlural { get; init; }
    
    public FText PortionDisplayNamePlural
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Display")]
        get => DisplayNamePortionPlural.Match(x => x, () => DisplayNamePlural);
    }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    private bool ShouldShowQuantity { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText Description { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "BagInfo")]
    [Categories(TagCategory)]
    public FGameplayTag Pocket { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Price")]
    public bool CanSell { get; init; } = true;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Price")]
    [EditCondition(nameof(CanSell))]
    [ClampMin("1")]
    [UIMin("1")]
    public int Price { get; init; } = 1;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, DisplayName = "Sell Price", Category = "Price")]
    [EditCondition(nameof(CanSell))]
    [ClampMin("1")]
    [UIMin("1")]
    private Option<int> PriceToSell { get; init; }
    
    public int SellPrice
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Price")]
        get => PriceToSell.Match(x => x, () => Price);
    }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, DisplayName = "BP Price", Category = "Price")]
    [EditCondition(nameof(CanSell))]
    [ClampMin("1")]
    [UIMin("1")]
    public int BpPrice { get; init; } = 1;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Usage")]
    public EFieldUse FieldUse { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Usage")]
    public EFieldUse BattleUse { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Usage")]
    [EditCondition($"{nameof(BattleUse)} != {nameof(EBattleUse)}::{nameof(EBattleUse.NoBattleUse)}")]
    [EditConditionHides]
    public FGameplayTagContainer BattleUsageCategories { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Usage")]
    public bool IsConsumable { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Usage")]
    [EditCondition($"{nameof(FieldUse)} == {nameof(EFieldUse)}::{nameof(EFieldUse.TM)} || " +
                   $"{nameof(FieldUse)} == {nameof(EFieldUse)}::{nameof(EFieldUse.TR)} || " +
                   $"{nameof(FieldUse)} == {nameof(EFieldUse)}::{nameof(EFieldUse.HM)}")]
    [EditConditionHides]
    public FGameplayTag Move { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata")]
    [Categories(MetadataCategory)]
    public FGameplayTagContainer Tags { get; init; }

    public bool IsTM
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Usage")]
        get => FieldUse == EFieldUse.TM;
    }
    
    public bool IsTR
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Usage")]
        get => FieldUse == EFieldUse.TR;
    }
    
    public bool IsHM
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Usage")]
        get => FieldUse == EFieldUse.HM;
    }

    public bool IsMachine
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Usage")]
        get => IsTM || IsTR || IsHM;
    }

    public bool IsMail
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get => Tags.HasTag(GameplayTags.Pokemon_Metadata_Items_Mail);
    }
    
    public bool IsIconMail
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get => Tags.HasTag(GameplayTags.Pokemon_Metadata_Items_IconMail);
    }
    
    public bool IsPokeBall
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_PokeBall);
        }
    }
    
    public bool IsBerry
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_Berry);
        }
    }
    
    public bool IsKeyItem
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_KeyItem);
        }
    }
    
    public bool IsEvolutionStone
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_EvolutionStone);
        }
    }
    
    public bool IsFossil
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_Fossil);
        }
    }
    
    public bool IsApricorn
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_Apricorn);
        }
    }
    
    public bool IsGem
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_TypeGem);
        }
    }
    
    public bool IsMulch
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_Mulch);
        }
    }
    
    public bool IsMegaStone
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_MegaStone);
        }
    }
    
    public bool IsScent
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Metadata")]
        get
        {
            var tags = Tags;
            return tags.HasTag(GameplayTags.Pokemon_Metadata_Items_Scent);
        }
    }

    public bool IsImportant
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Usage")]
        get => IsKeyItem || IsHM || IsTM;
    }

    public bool CanHold
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Usage")]
        get => !IsImportant;
    }

    
    public bool ShowQuantity
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Display")]
        get => ShouldShowQuantity || !IsImportant;
    }

    public bool ConsumedAfterUse
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Usage")]
        get => !IsImportant && IsConsumable;
    }
}