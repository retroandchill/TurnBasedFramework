using Pokemon.Editor.Serializers.Pbs.Converters;

namespace Pokemon.Editor.Serializers.Pbs.Attributes;

public abstract class PbsScalarAttribute : Attribute
{
    public abstract Type ConverterType { get; }
}


[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
public sealed class PbsScalarAttribute<T> : PbsScalarAttribute where T : IPbsConverter
{
    public override Type ConverterType => typeof(T);
}