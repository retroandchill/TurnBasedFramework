using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using System.Text.Json.Nodes;
using CaseConverter;
using Pokemon.Data;
using Pokemon.Data.Attributes;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Serializers.Pbs.Converters;

public class EvolutionMethodConverter : PbsConverterBase<EvolutionConditionInfo>
{
    private readonly TStrongObjectPtr<UEvolutionMethodDataRepository> _evolutionMethodsRepository;

    public EvolutionMethodConverter()
    {
        var settings = UObject.GetDefault<UGameDataSettings>();
        var evolutionMethodsRepo =
            ((TSoftObjectPtr<UObject>)settings.EvolutionMethods).LoadSynchronous() as UEvolutionMethodDataRepository;
        ArgumentNullException.ThrowIfNull(evolutionMethodsRepo);
        evolutionMethodsRepo.Refresh();
        _evolutionMethodsRepository = evolutionMethodsRepo;
    }

    public override string WriteCsvValue(EvolutionConditionInfo value, PbsScalarDescriptor schema, string? sectionName)
    {
        var speciesString = value.Species.ToString();
        var species = speciesString.StartsWith($"{USpecies.TagCategory}.") ? speciesString[(USpecies.TagCategory.Length + 1)..] : speciesString;
        var (methodName, methodTag) = UObject.GetDefault<UPokemonEditorSettings>().EvolutionConditionToGameplayTag
            .Single(x => x.Value == value.Method);

        if (value.Data is null)
        {
            return $"{species},{methodName}";
        }
        
        var method = _evolutionMethodsRepository.Value!.GetEntry(methodTag);
        
        var dataParameters = method.ConditionType.Valid ? method.ConditionType.DefaultObject.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<UPropertyAttribute>() is not null)
            .ToImmutableArray() : [];
        
        ArgumentOutOfRangeException.ThrowIfLessThan(dataParameters.Length, value.Data.Count, nameof(value.Data));

        var additionalParameters = dataParameters.Zip(value.Data, (x, y) => (Property: x, Node: y))
            .Select(x =>
            {
                if (x.Property.PropertyType != typeof(FGameplayTag)) return x.Node.Value!.ToString();
                
                var prefix = x.Property.GetCustomAttribute<ParentTagAttribute>()?.TagPrefix;
                var tagName = x.Node.Value!.AsObject()["tagName"]!.GetValue<string>();
                return prefix is not null && tagName.StartsWith($"{prefix}.")
                    ? tagName[(prefix.Length + 1)..]
                    : tagName;
            });

        return value.Data is not null ? $"{species},{methodName},{string.Join(",", additionalParameters)}" : $"{species},{methodName}";
    }

    public override EvolutionConditionInfo GetCsvValue(string input, PbsScalarDescriptor scalarDescriptor, string? sectionName)
    {
        var data = input.Split(",");
        ArgumentOutOfRangeException.ThrowIfLessThan(data.Length, 2, nameof(data));

        var species = new FName($"{USpecies.TagCategory}.{data[0]}");
        var methodName = new FName(data[1]);
        var methodTag = UObject.GetDefault<UPokemonEditorSettings>().EvolutionConditionToGameplayTag[methodName];
        
        var method = _evolutionMethodsRepository.Value!.GetEntry(methodTag);
        if (!method.ConditionType.Valid)
        {
            return new EvolutionConditionInfo(species, methodTag);
        }

        var dataParameters = method.ConditionType.DefaultObject.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<UPropertyAttribute>() is not null)
            .ToImmutableArray();
        
        ArgumentOutOfRangeException.ThrowIfLessThan(data.Length, dataParameters.Length + 2, nameof(data));
        var evolutionData = new JsonObject();
        foreach (var (key, value) in dataParameters.Zip(data.Skip(2), (x, y) => (x, y)))
        {
            JsonNode jsonNode;
            if (key.PropertyType == typeof(FGameplayTag))
            {
                var prefix = key.GetCustomAttribute<ParentTagAttribute>()?.TagPrefix;
                jsonNode = new JsonObject { { "tagName", JsonValue.Create(prefix is not null ? $"{prefix}.{value}" : value) } };
            }
            else if (key.PropertyType.GetInterfaces()
                        .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(INumber<>)))
            {
                jsonNode = JsonNode.Parse(value)!;
            }
            else
            {
                jsonNode = JsonValue.Create(value);
            }
            
            evolutionData.Add(key.Name.ToCamelCase(), jsonNode);
        }
        
        return new EvolutionConditionInfo(species, methodTag, method.ConditionType, evolutionData);
    }
}