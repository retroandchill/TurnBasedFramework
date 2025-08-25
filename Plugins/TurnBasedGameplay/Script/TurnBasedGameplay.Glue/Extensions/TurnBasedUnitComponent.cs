namespace UnrealSharp.TurnBasedCore;

public partial class UTurnBasedUnitComponent
{
    public T GetSiblingComponent<T>() where T : UTurnBasedUnitComponent
    {
        return GetSiblingComponent<T>(typeof(T));
    }

    public T GetSiblingComponent<T>(TSubclassOf<T> componentClass) where T : UTurnBasedUnitComponent
    {
        return TryGetSiblingComponent(componentClass, out var component)
            ? component
            : throw new InvalidOperationException($"Component of type {componentClass} found on {OwningUnit.ObjectName}");
    }
}