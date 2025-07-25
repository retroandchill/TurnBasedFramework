using UnrealSharp;

namespace ManagedNativeReflectionAccessors.Reflection;

public class FFieldClass
{
    public ulong Id { get; }
    
    public string Name { get; }
    
    public FName FName { get; }
    
    public FText DisplayName { get; }
    
    public string Description { get; }
    
    public FField DefaultObject { get; }
    
    public FFieldClass? SuperClass { get; }

    public bool HasAnyClassFlags(EClassFlags flags)
    {
        
    }

    public bool IsChildOf(FFieldClass other)
    {
        
    }
}