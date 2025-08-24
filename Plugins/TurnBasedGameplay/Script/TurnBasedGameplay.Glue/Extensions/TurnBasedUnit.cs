using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;

namespace UnrealSharp.TurnBasedCore;

public partial class UTurnBasedUnit
{
    public static T Create<T>(UObject outer, TSubclassOf<T> componentClass, Action<T>? initializer = null)
        where T : UTurnBasedUnit
    {
        var initializerDelegate = new FManagedInitializerDelegate(initializer is not null ? ptr =>
        {
            var unit = ObjectMarshaller<T>.FromNative(ptr, 0);
            initializer(unit);
        } : null);
        return Create(outer, componentClass, initializerDelegate);
    }
    
    public static T Create<T>(UObject outer, Action<T>? initializer = null)
        where T : UTurnBasedUnit
    {
        return Create(outer, typeof(T), initializer);
    }
    
    public T GetComponent<T>() where T : UTurnBasedUnitComponent
    {
        return GetComponent<T>(typeof(T));
    }

    public T GetComponent<T>(TSubclassOf<T> componentClass) where T : UTurnBasedUnitComponent
    {
        return TryGetComponent(componentClass, out var component)
            ? component
            : throw new InvalidOperationException($"Component of type {componentClass} found on {ObjectName}");
    }

    protected void RegisterNewComponent(UTurnBasedUnitComponent component)
    {
        if (!RegisterNewComponentInternal(component))
        {
            throw new InvalidOperationException($"Component of type {component.Class} already exists");
        }
    }
}