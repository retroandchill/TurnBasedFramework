using System.Diagnostics.CodeAnalysis;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;

namespace UnrealSharp.TurnBasedCore;

/// <summary>
/// Represents a contract for a turn-based unit associated with a specific component in a turn-based system.
/// This interface exposes a generic accessor to the component that represents or manages the behavior
/// of the unit within the turn-based system.
/// </summary>
/// <typeparam name="TComponent">The type of the component associated with the unit. It must derive from UTurnBasedUnitComponent</typeparam>
/// <remarks>
/// This interface is used to provide a generic way to access the component associated with a unit when the association
/// is known at compile time. Calls to <see cref="UTurnBasedUnit.GetComponent{T}()">GetComponent</see> and
/// <see cref="UTurnBasedUnit.TryGetComponent{T}(out T?)">TryGetComponent</see> will see if the unit implements this
/// interface before falling back to the component list.
/// </remarks>
public interface ITurnBasedUnit<out TComponent>
    where TComponent : UTurnBasedUnitComponent
{
    /// <summary>
    /// Gets the component associated with the turn-based unit. This component represents
    /// or manages specific behavior or data related to the unit within the turn-based system.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component associated with the unit.
    /// It must derive from <see cref="UTurnBasedUnitComponent"/>.</typeparam>
    /// <returns>
    /// The instance of the component that is associated with the unit.
    /// </returns>
    TComponent Component { get; }
}

public partial class UTurnBasedUnit
{
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
        if (
            this is not ITurnBasedUnit<T> turnBasedUnit
            || !componentClass.IsParentOf(turnBasedUnit.Component.GetType())
        )
            return TryGetComponentInternal(componentClass, out component);

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
            return TryGetComponentInternal<T>(out component);

        component = turnBasedUnit.Component;
        return true;
    }

    /// <summary>
    /// Registers a new component for this turn-based unit. Throws an exception if a component of the same type already exists.
    /// </summary>
    /// <param name="component">The component to register with this turn-based unit. Must derive from UTurnBasedUnitComponent.</param>
    /// <exception cref="InvalidOperationException">Thrown if a component of the same type is already registered.</exception>
    protected void RegisterNewComponent(UTurnBasedUnitComponent component)
    {
        if (!RegisterNewComponentInternal(component))
        {
            throw new InvalidOperationException(
                $"Component of type {component.Class} already exists"
            );
        }
    }
}
