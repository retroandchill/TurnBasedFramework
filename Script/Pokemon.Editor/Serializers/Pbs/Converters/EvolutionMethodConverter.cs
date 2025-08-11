using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CaseConverter;
using GameDataAccessTools.Core.DataRetrieval;
using Microsoft.Extensions.DependencyInjection;
using Pokemon.Data;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealInject.Subsystems;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Converters;

public class EvolutionMethodConverter : PbsConverterBase<EvolutionConditionInfo>
{
    private readonly TStrongObjectPtr<UEvolutionMethodDataRepository> _evolutionMethodsRepository;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public EvolutionMethodConverter()
    {
        var settings = UObject.GetDefault<UGameDataSettings>();
        var evolutionMethodsRepo =
            ((TSoftObjectPtr<UObject>)settings.EvolutionMethods).LoadSynchronous() as UEvolutionMethodDataRepository;
        ArgumentNullException.ThrowIfNull(evolutionMethodsRepo);
        evolutionMethodsRepo.Refresh();
        _evolutionMethodsRepository = evolutionMethodsRepo;

        var subsystem = UObject.GetEngineSubsystem<UDependencyInjectionEngineSubsystem>();
        _jsonSerializerOptions = subsystem.GetRequiredService<JsonSerializerOptions>();
    }

    public override string WriteCsvValue(EvolutionConditionInfo value, PbsScalarDescriptor schema, string? sectionName)
    {
        var speciesString = value.Species.ToString();
        var species = speciesString.StartsWith($"{USpecies.TagCategory}.") ? speciesString[(USpecies.TagCategory.Length + 1)..] : speciesString;
        var (methodName, _) = UObject.GetDefault<UPokemonEditorSettings>().EvolutionConditionToGameplayTag
            .Single(x => x.Value == value.Method);

        return value.Data is not null ? $"{species},{methodName},{string.Join(",", value.Data.Select(x => x.ToString()))}" : $"{species},{methodName}";
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
            var jsonNodeValue = !key.PropertyType.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(INumber<>)) ? $"\"{value}\"" : value;
            
            evolutionData.Add(key.Name.ToCamelCase(), JsonSerializer.Deserialize<JsonNode>(jsonNodeValue, _jsonSerializerOptions)!);
        }
        
        return new EvolutionConditionInfo(species, methodTag, method.ConditionType, evolutionData);
    }
}