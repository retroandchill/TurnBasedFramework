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

    protected T RegisterNewComponent<T>(TSubclassOf<T> componentClass) where T : UTurnBasedUnitComponent
    {
        var result = RegisterNewComponentInternal(componentClass);
        if (result is null)
        {
            throw new InvalidOperationException($"Component of type {componentClass} already exists");
        }
        
        return result;
    }

    protected T RegisterNewComponent<T>() where T : UTurnBasedUnitComponent
    {
        return RegisterNewComponent<T>(typeof(T));
    }
}