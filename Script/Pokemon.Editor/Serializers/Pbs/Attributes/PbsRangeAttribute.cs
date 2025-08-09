using System.Numerics;

namespace Pokemon.Editor.Serializers.Pbs.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PbsRangeAttribute<T> : Attribute where T : struct, INumber<T>
{
    public T? Min { get; init; }
    
    public T? Max { get; init; }
}