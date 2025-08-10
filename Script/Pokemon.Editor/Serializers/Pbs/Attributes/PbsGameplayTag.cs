namespace Pokemon.Editor.Serializers.Pbs.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PbsGameplayTag(string ns) : Attribute
{
    public string Namespace { get; } = ns;
    
    public bool Create { get; init; }
    
    public string? Separator { get; init; }
}