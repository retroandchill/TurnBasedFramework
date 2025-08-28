using System.Diagnostics.CodeAnalysis;

namespace UnrealSharp.TurnBasedCore;

public partial class UTurnBasedUnitComponent
{
    /// <summary>
    /// Retrieves a sibling component of the specified type from the owning unit.
    /// This method searches for a sibling component of the given type associated with the entity's owning unit.
    /// If the component is found, it is returned; otherwise, an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve.</typeparam>
    /// <returns>
    /// The sibling component of the specified type.
    /// Throws <see cref="InvalidOperationException"/> if the component is not found.
    /// </returns>
    public T GetSiblingComponent<T>()
        where T : UTurnBasedUnitComponent
    {
        return TryGetSiblingComponent<T>(out var component)
            ? component
            : throw new InvalidOperationException(
                $"Component of type {typeof(T)} found on {OwningUnit.ObjectName}"
            );
    }

    /// <summary>
    /// Retrieves a sibling component of the specified type from the owning unit.
    /// This method searches for a sibling component of the given type associated with the entity's owning unit.
    /// If the component is found, it is returned; otherwise, an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve.</typeparam>
    /// <param name="componentClass">The class type of the sibling component to search for.</param>
    /// <returns>
    /// The sibling component of the specified type.
    /// Throws <see cref="InvalidOperationException"/> if the component is not found.
    /// </returns>
    public T GetSiblingComponent<T>(TSubclassOf<T> componentClass)
        where T : UTurnBasedUnitComponent
    {
        return TryGetSiblingComponent(componentClass, out var component)
            ? component
            : throw new InvalidOperationException(
                $"Component of type {componentClass} found on {OwningUnit.ObjectName}"
            );
    }

    /// <summary>
    /// Attempts to retrieve a sibling component of the specified type from the owning unit without throwing an exception if not found.
    /// This method checks for a sibling component of the given type associated with the entity's owning unit and outputs the component if found.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve.</typeparam>
    /// <param name="componentClass">The class type of the component to search for.</param>
    /// <param name="component">
    /// When this method returns, contains the sibling component of the specified type if the retrieval is successful,
    /// or null if the component was not found.
    /// </param>
    /// <returns>
    /// True if the sibling component of the specified type is found; otherwise, false.
    /// </returns>
    public bool TryGetSiblingComponent<T>(
        TSubclassOf<T> componentClass,
        [NotNullWhen(true)] out T? component
    )
        where T : UTurnBasedUnitComponent
    {
        return OwningUnit.TryGetComponent(componentClass, out component);
    }

    /// <summary>
    /// Attempts to retrieve a sibling component of the specified type from the owning unit without throwing an exception.
    /// This method searches for a sibling component of the specified type associated with the entity's owning unit.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve.</typeparam>
    /// <param name="component">
    /// When the method returns, contains the sibling component of the specified type if found; otherwise, the default value.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether or not the sibling component was successfully retrieved.
    /// Returns true if the component is found; otherwise, false.
    /// </returns>
    public bool TryGetSiblingComponent<T>([NotNullWhen(true)] out T? component)
        where T : UTurnBasedUnitComponent
    {
        return OwningUnit.TryGetComponent(out component);
    }
}
