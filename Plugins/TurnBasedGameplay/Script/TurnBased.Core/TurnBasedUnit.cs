using System.Diagnostics.CodeAnalysis;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using ZLinq;

namespace TurnBased.Core;

public interface ITurnBasedUnit<out T> where T : UTurnBasedUnitComponent
{
    T Component { get; }
}

[UClass(ClassFlags.Abstract)]
public class UTurnBasedUnit : UObject
{
    [UProperty] 
    public TArray<UTurnBasedUnitComponent> Components { get; }

    private bool _initialized;
    
    public static T Create<T>(UObject outer, Action<T>? constructor = null) where T : UTurnBasedUnit
    {
        var newUnit = NewObject<T>(outer);
        InitializeInternal(newUnit, constructor);
        return newUnit;
    }
    
    public static T Create<T>(UObject outer, TSubclassOf<T> unitClass, Action<T>? constructor = null) where T : UTurnBasedUnit
    {
        var newUnit = NewObject(outer, unitClass);
        InitializeInternal(newUnit, constructor);
        return newUnit;
    }

    private static void InitializeInternal<T>(T newUnit, Action<T>? constructor = null) where T : UTurnBasedUnit
    {
        newUnit.CreateComponents();
        constructor?.Invoke(newUnit);

        foreach (var component in newUnit.Components)
        {
            component.PostInitializeUnit();
        }
        newUnit._initialized = true;
    }
    
    /// <summary>
    /// Retrieves a component of the specified type associated with this turn-based unit.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve. Must derive from UTurnBasedUnitComponent.</typeparam>
    /// <returns>The component of the specified type, if it exists; otherwise, an exception is thrown if no such component exists.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no component of the specified type exists.</exception>
    public T GetComponent<T>()
        where T : UTurnBasedUnitComponent
    {
        return TryGetComponent<T>(out var component)
            ? component
            : throw new InvalidOperationException(
                $"Component of type {typeof(T)} found on {ObjectName}"
            );
    }

    /// <summary>
    /// Retrieves a component of the specified type associated with this turn-based unit.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve. Must derive from UTurnBasedUnitComponent.</typeparam>
    /// <param name="componentClass">The specific subclass of the component type to retrieve.</param>
    /// <returns>The component of the specified type, if it exists; otherwise, an exception is thrown if no such component exists.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no component of the specified type exists.</exception>
    public T GetComponent<T>(TSubclassOf<T> componentClass)
        where T : UTurnBasedUnitComponent
    {
        return TryGetComponent(componentClass, out var component)
            ? component
            : throw new InvalidOperationException(
                $"Component of type {componentClass} found on {ObjectName}"
            );
    }

    /// <summary>
    /// Attempts to retrieve a component of the specified type associated with this turn-based unit.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve. Must derive from UTurnBasedUnitComponent.</typeparam>
    /// <param name="componentClass">The class of the component to retrieve.</param>
    /// <param name="component">When this method returns, contains the component of the specified type, if found; otherwise, null.</param>
    /// <returns>True if the component of the specified type is found; otherwise, false.</returns>
    public bool TryGetComponent<T>(
        TSubclassOf<T> componentClass,
        [NotNullWhen(true)] out T? component
    )
        where T : UTurnBasedUnitComponent
    {
        if (this is not ITurnBasedUnit<T> turnBasedUnit || !turnBasedUnit.Component.IsA(componentClass))
        {
            component = Components.AsValueEnumerable()
                .OfType<T>()
                .Where(c => c.IsA(componentClass))
                .FirstOrDefault();
            return component is not null;
        }
                

        component = turnBasedUnit.Component;
        return true;
    }

    /// <summary>
    /// Attempts to retrieve a component of the specified type associated with this turn-based unit.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve. Must derive from UTurnBasedUnitComponent.</typeparam>
    /// <param name="component">
    /// When this method returns, contains the component of the specified type if it exists; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a component of the specified type exists and was successfully retrieved; otherwise, false.
    /// </returns>
    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component)
        where T : UTurnBasedUnitComponent
    {
        if (this is not ITurnBasedUnit<T> turnBasedUnit)
        {
            component = Components.AsValueEnumerable()
                .OfType<T>()
                .FirstOrDefault();
            return component is not null;
        }

        component = turnBasedUnit.Component;
        return true;
    }

    public T RegisterNewComponent<T>(Action<T>? constructor = null) where T : UTurnBasedUnitComponent
    {
        var component = NewObject<T>(this);
        constructor?.Invoke(component);
        return component;
    }
    
    public T RegisterNewComponent<T>(TSubclassOf<T> componentClass, Action<T>? constructor = null) where T : UTurnBasedUnitComponent
    {
        var component = NewObject(this, componentClass);
        constructor?.Invoke(component);
        
        if (_initialized)
        {
            component.PostInitializeUnit();
        }
        return component;
    }

    protected virtual void CreateComponents()
    {
        
    }
}