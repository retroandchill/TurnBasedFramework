using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;

namespace UnrealSharp.TurnBasedCore;

public partial class UTurnBasedUnit
{
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