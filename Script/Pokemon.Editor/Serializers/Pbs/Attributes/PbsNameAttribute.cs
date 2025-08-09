namespace Pokemon.Editor.Serializers.Pbs.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PbsNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}