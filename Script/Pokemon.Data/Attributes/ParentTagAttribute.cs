namespace Pokemon.Data.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ParentTagAttribute(string tagPrefix) : Attribute
{
    public string TagPrefix { get; } = tagPrefix;
}