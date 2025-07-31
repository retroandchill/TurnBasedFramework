using System.Diagnostics.CodeAnalysis;
using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

[UClass(ClassFlags.EditInlineNew | ClassFlags.Abstract)]
public class UEvolutionConditionData : UObject;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEvolutionMethod : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.EvolutionMethods";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [UMetaData("Categories", TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Evolution")]
    public TSubclassOf<UEvolutionConditionData> ConditionType { get; init; }
}

[UStruct]
public struct FEvolutionCondition
{
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [UMetaData("Categories", UEvolutionMethod.TagCategory)]
    public FGameplayTag Method { get; init; }

    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere | PropertyFlags.Instanced)]
    public UEvolutionConditionData Data { get; init; }
    
    public bool IsValid => Method.IsValid && Data is not null 
                                          && GameData.EvolutionMethods.GetEntry(Method)
                                              .ConditionType.IsChildOf(Data.GetType());

    public UEvolutionConditionData GetData<T>() where T : UEvolutionConditionData
    {
        if (TryGetData<T>(out var data))
        {
            return data;
        }
        
        throw new InvalidOperationException($"Evolution condition data is not of type {typeof(T)}");
    }

    public bool TryGetData<T>([NotNullWhen(true)] out T? data) where T : UEvolutionConditionData
    {
        if (GameData.EvolutionMethods.TryGetEntry(Method, out var entry) && 
            entry.ConditionType.IsChildOf(typeof(T)) && Data is T typedData)
        {
            data = typedData;
            return true;
        }
        
        data = null;
        return false;
    }
}