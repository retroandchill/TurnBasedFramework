using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using UnrealSharp;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;
using static GameDataAccessTools.Core.Serialization.SerializationExtensions;

namespace Pokemon.Editor.Serializers.Pbs;

public readonly record struct PbsSectionData(
    string Name,
    int Index,
    IReadOnlyDictionary<string, List<string>> Contents);

public static partial class PbsCompiler
{
    public static IEnumerable<PbsSectionData> EachFileSection(string contents, PbsSchema? schema = null)
    {
        var sectionIndex = 0;
        var lineNumber = 1;
        string? sectionName = null;
        var lastSection = new Dictionary<string, List<string>>();
        using var reader = new StringReader(contents);
        var line = reader.ReadLine();
        while (line is not null)
        {
            if (!line.StartsWith('#') && !string.IsNullOrWhiteSpace(line))
            {
                line = PrepLine(line);

                if (TryGetSectionHeader(line, out var newSection))
                {
                    if (sectionName is not null)
                    {
                        yield return new PbsSectionData(sectionName, sectionIndex, lastSection);
                        sectionIndex++;
                    }

                    sectionName = newSection;
                    lastSection = new Dictionary<string, List<string>>();
                }
                else
                {
                    if (sectionName is null)
                    {
                        throw new InvalidOperationException(
                            $"Unexpected line {lineNumber} in file: {line}\nExpected a section at the beginning of the file.\nThis error may also occur if the file was not saved in UTF-8.");
                    }

                    if (!TryGetEntry(line, out var key, out var value))
                    {
                        throw new InvalidOperationException(
                            $"Unexpected line {lineNumber} in file: {line}\nBad line syntax (expected syntax like XXX=YYY).");
                    }

                    if (schema is not null && schema.TryGetField(key, out var property) &&
                        property.Repeat == RepeatMode.KeyRepeat)
                    {
                        if (!lastSection.TryGetValue(key, out var existingValue))
                        {
                            existingValue = [value.TrimEnd()];
                            lastSection[key] = existingValue;
                            continue;
                        }

                        lastSection[key] = existingValue.Append(value.TrimEnd()).ToList();
                    }
                    else
                    {
                        lastSection[key] = [value.TrimEnd()];
                    }
                }
            }

            line = reader.ReadLine();
            lineNumber++;
        }

        if (sectionName is not null) yield return new PbsSectionData(sectionName, sectionIndex, lastSection);
    }

    private static string PrepLine(string line)
    {
        return PreCommentRegex()
            .Replace(line, string.Empty)
            .Trim();
    }

    [GeneratedRegex(@"\s*\#.*$", RegexOptions.Compiled)]
    private static partial Regex PreCommentRegex();

    private static bool TryGetSectionHeader(string line, [NotNullWhen(true)] out string? sectionName)
    {
        var sectionHeaderRegex = SectionHeaderRegex();
        var match = sectionHeaderRegex.Match(line);
        if (match.Success)
        {
            sectionName = match.Groups[1].Value;
            return true;
        }

        sectionName = null;
        return false;
    }

    [GeneratedRegex(@"^\s*\[\s*(.*)\s*\]\s*$")]
    private static partial Regex SectionHeaderRegex();

    private static bool TryGetEntry(string line, [NotNullWhen(true)] out string? key,
                                    [NotNullWhen(true)] out string? value)
    {
        var valueEntryRegex = SectionEntryRegex();
        var match = valueEntryRegex.Match(line);
        if (match.Success)
        {
            key = match.Groups[1].Value;
            value = match.Groups[2].Value;
            return true;
        }

        key = null;
        value = null;
        return false;
    }

    [GeneratedRegex(@"^\s*(\w+)\s*=\s*(.*)$")]
    private static partial Regex SectionEntryRegex();

    public static IReadOnlyList<string> SplitCsvLine(string input)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            Mode = CsvMode.RFC4180,
            Escape = '\\',
            HasHeaderRecord = false
        };

        using var csv = new CsvReader(new StringReader(input), config);
        csv.Read();
        return csv.Parser.Record ?? [];
    }

    public static object? GetCsvRecord(string input, PbsFieldDescriptor schema)
    {
        var targetType = schema.TargetProperty.PropertyType;
        if (schema.IsScalar)
        {
            return GetCsvValue(input, schema.Elements.Single());
        }

        var subLists = schema is { Repeat: RepeatMode.CsvRepeat, Elements.Length: > 1 };
        var values = SplitCsvLine(input);
        var idx = -1;
        var result = new List<object?>();
        while (true)
        {
            var record = new object?[schema.Elements.Length];
            foreach (var (element, i) in schema.Elements.Select((x, i) => (x, i)))
            {
                idx++;
                if (element.IsOptional && string.IsNullOrWhiteSpace(values[idx]))
                {
                    record[i] = null;
                    continue;
                }

                record[i] = GetCsvValue(values[idx], element);
            }

            if (subLists)
            {
                result.Add(Activator.CreateInstance(targetType, record));
            }
            else
            {
                result.AddRange(record);
            }

            if (schema.Repeat != RepeatMode.CsvRepeat || idx >= values.Count - 1) break;
        }

        return targetType.TryGetCollectionFactory(out var factory) ? factory(result) : result.Single();
    }

    private static object GetCsvValue(string input, PbsScalarDescriptor scalarDescriptor)
    {
        if (scalarDescriptor.Type == typeof(bool))
        {
            return ParseBool(input);
        }

        if (scalarDescriptor.Type == typeof(string))
        {
            return input;
        }

        if (scalarDescriptor.Type == typeof(FName))
        {
            return new FName(input);
        }

        if (scalarDescriptor.Type == typeof(FText))
        {
            return input.FromLocalizedString();
        }

        if (scalarDescriptor.Type == typeof(FGameplayTag))
        {
            var tagName = scalarDescriptor.GameplayTagNamespace is not null
                ? $"{scalarDescriptor.GameplayTagNamespace}.{input}"
                : input;
            return scalarDescriptor.CreateNewGameplayTag ? GetOrCreateGameplayTag(tagName) : new FGameplayTag(tagName);
        }

        if (scalarDescriptor.Type.IsEnum)
        {
            return Enum.Parse(scalarDescriptor.Type, input, true);
        }

        if (scalarDescriptor.Type.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>)))
        {
            var parseNumber = typeof(PbsCompiler)
                .GetMethod(nameof(ParseNumber), BindingFlags.Static | BindingFlags.Public)!
                .MakeGenericMethod(scalarDescriptor.Type.GetGenericArguments()[0]);
            return parseNumber.Invoke(null, [input, scalarDescriptor.NumericBounds, null])!;
        }

        throw new InvalidOperationException($"Unsupported scalar type {scalarDescriptor.Type}");
    }
    
    public static bool ParseBool(string input)
    {
        var trueRegex = TrueRegex();
        var falseRegex = FalseRegex();
        
        if (trueRegex.IsMatch(input))
        {
            return true;
        }
        
        if (falseRegex.IsMatch(input))
        {
            return false;
        }
        
        throw new InvalidOperationException($"Failed to parse boolean {input}");
    }

    [GeneratedRegex("^(?:1|TRUE|YES|Y)$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex TrueRegex();
    [GeneratedRegex("^(?:0|FALSE|NO|N)$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex FalseRegex();
    
    public static T ParseNumber<T>(string input, NumericBounds<T> bounds = default, IFormatProvider? provider = null) where T : struct, INumber<T>
    {
        if (!T.TryParse(input, provider, out var result))
        {
            throw new InvalidOperationException($"Failed to parse number {input}");
        }

        if (bounds.Min.HasValue && result.CompareTo(bounds.Min.Value) < 0)
        {
            throw new InvalidOperationException($"Number {input} is below minimum {bounds.Min}");
        }

        if (bounds.Max.HasValue && result.CompareTo(bounds.Max.Value) > 0)
        {
            throw new InvalidOperationException($"Number {input} is above maximum {bounds.Max}");
        }
        
        return result;
    }

    public static IReadOnlyDictionary<string, T> CompilePbsFile<T>(string input)
    {
        var result = new Dictionary<string, T>();
        var schema = PbsMetamodel.GetSchema<T>();
        foreach (var (sectionName, i, contents) in EachFileSection(input, schema))
        {
            if (result.ContainsKey(sectionName))
            {
                throw new InvalidOperationException($"Duplicate section {sectionName} in file");
            }

            var instance = Activator.CreateInstance<T>();
            foreach (var (_, field) in schema.Fields)
            {
                if (field.IsIdentifier)
                {
                    field.TargetProperty.SetValue(instance, GetCsvRecord(sectionName, field));
                    continue;
                }

                if (field.IsRowIndex)
                {
                    field.TargetProperty.SetValue(instance, i);
                    continue;
                }

                if (!contents.TryGetValue(field.KeyName, out var contentValue)) continue;

                if (field.Repeat == RepeatMode.KeyRepeat)
                {
                    var value = contentValue.Select(x => GetCsvRecord(x, field));
                    if (!field.TargetProperty.PropertyType.TryGetCollectionFactory(out var factory))
                    {
                        throw new InvalidOperationException($"Unsupported collection type {field.TargetProperty.PropertyType}");
                    }
                    
                    field.TargetProperty.SetValue(instance, factory(value));
                }
                else
                {
                    field.TargetProperty.SetValue(instance, GetCsvRecord(contentValue.Single(), field));
                }
            }

            result.Add(sectionName, instance);
        }

        return result;
    }
}