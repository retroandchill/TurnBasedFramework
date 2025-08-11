namespace Pokemon.Editor.Serializers.Pbs.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class PbsNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}