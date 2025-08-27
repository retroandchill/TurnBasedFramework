using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.Engine;
using UnrealSharp.TurnBasedCore;

namespace TurnBased.Core;

/// <summary>
/// Represents the result of a component search operation performed on a unit.
/// </summary>
/// <remarks>
/// This enumeration is used to indicate whether a specific component was found on the given unit.
/// </remarks>
[UEnum]
public enum EComponentFindResult : byte
{
    /// <summary>
    /// The component was found.
    /// </summary>
    Found,

    /// <summary>
    /// The component was not found.
    /// </summary>
    NotFound,
}

[UClass]
public class UTurnBasedUnitExtensions : UBlueprintFunctionLibrary
{
    /// <summary>
    /// Attempts to find a specific component of the given type on a specified unit.
    /// </summary>
    /// <param name="unit">
    /// The unit on which the component search will be performed. Must not be null.
    /// </param>
    /// <param name="componentClass">
    /// The class type of the component to search for.
    /// </param>
    /// <param name="component">
    /// If the component is found, this will hold the reference to the found component. If not found, it will be set to null.
    /// </param>
    /// <returns>
    /// An <see cref="EComponentFindResult"/> indicating whether the component was found or not.
    /// </returns>
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Component")]
    [ExpandEnumAsExecs("ReturnValue")]
    [DeterminesOutputType("componentClass")]
    [UMetaData("DynamicOutputParam", "component")]
    [UMetaData("DefaultToSelf", "unit")]
    public static EComponentFindResult GetComponent(
        UTurnBasedUnit unit,
        TSubclassOf<UTurnBasedUnitComponent> componentClass,
        out UTurnBasedUnitComponent? component
    )
    {
        return unit.TryGetComponent(componentClass, out component)
            ? EComponentFindResult.Found
            : EComponentFindResult.NotFound;
    }
}
