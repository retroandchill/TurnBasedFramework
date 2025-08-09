namespace Pokemon.Editor.Serializers.Pbs.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PbsLocalizedTextAttribute(string ns, string keyFormat) : Attribute
{
    public string Namespace { get; } = ns;
    public string KeyFormat { get; } = keyFormat;
}