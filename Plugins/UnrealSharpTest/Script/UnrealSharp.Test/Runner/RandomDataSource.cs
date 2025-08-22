using NUnit.Framework.Internal;

namespace UnrealSharp.Test.Runner;

public interface IRandomDataSource
{
    int Count { get; }
    Type Type { get; }
    object?[] CollectValues(Randomizer random, bool distinct = false);
}

public abstract class RandomDataSourceBase(int count, Type type) : IRandomDataSource
{
    public int Count { get; } = count;
    public Type Type { get; } = type;

    public object?[] CollectValues(Randomizer random, bool distinct = false)
    {
        var values = new object?[Count];
        for (var i = 0; i < Count; i++)
        {
            var nextValue = GetNextValue(random);
            var iterationCount = 0;
            while (distinct && values.Take(i).Any(x => Equals(x, nextValue)))
            {
                nextValue = GetNextValue(random);
                iterationCount++;
                if (iterationCount > 1000)
                    throw new InvalidOperationException("Too many iterations");
            }

            values[i] = Convert.ChangeType(nextValue, Type);
        }

        return values;
    }

    protected abstract object? GetNextValue(Randomizer random);
}

public abstract class RandomDataSourceBase<T> : RandomDataSourceBase
{
    protected T? Min { get; }
    protected T? Max { get; }
    protected bool InRange { get; }

    protected RandomDataSourceBase(int count)
        : base(count, typeof(T))
    {
        Min = default!;
        Max = default!;
        InRange = false;
    }

    protected RandomDataSourceBase(T min, T max, int count)
        : base(count, typeof(T))
    {
        Min = min;
        Max = max;
        InRange = true;
    }
}

public sealed class IntRandomDataSource : RandomDataSourceBase<int>
{
    public IntRandomDataSource(int count)
        : base(count) { }

    public IntRandomDataSource(int min, int max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.Next(Min, Max) : random.Next();
    }
}

public sealed class UIntRandomDataSource : RandomDataSourceBase<uint>
{
    public UIntRandomDataSource(int count)
        : base(count) { }

    public UIntRandomDataSource(uint min, uint max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextUInt(Min, Max) : random.NextUInt();
    }
}

public sealed class ShortRandomDataSource : RandomDataSourceBase<short>
{
    public ShortRandomDataSource(int count)
        : base(count) { }

    public ShortRandomDataSource(short min, short max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextShort(Min, Max) : random.NextShort();
    }
}

public sealed class UShortRandomDataSource : RandomDataSourceBase<ushort>
{
    public UShortRandomDataSource(int count)
        : base(count) { }

    public UShortRandomDataSource(ushort min, ushort max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextUShort(Min, Max) : random.NextUShort();
    }
}

public sealed class ByteRandomDataSource : RandomDataSourceBase<byte>
{
    public ByteRandomDataSource(int count)
        : base(count) { }

    public ByteRandomDataSource(byte min, byte max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextByte(Min, Max) : random.NextByte();
    }
}

public sealed class SByteRandomDataSource : RandomDataSourceBase<sbyte>
{
    public SByteRandomDataSource(int count)
        : base(count) { }

    public SByteRandomDataSource(sbyte min, sbyte max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextSByte(Min, Max) : random.NextSByte();
    }
}

public sealed class LongRandomDataSource : RandomDataSourceBase<long>
{
    public LongRandomDataSource(int count)
        : base(count) { }

    public LongRandomDataSource(long min, long max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextLong(Min, Max) : random.NextLong();
    }
}

public sealed class ULongRandomDataSource : RandomDataSourceBase<ulong>
{
    public ULongRandomDataSource(int count)
        : base(count) { }

    public ULongRandomDataSource(ulong min, ulong max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextULong(Min, Max) : random.NextULong();
    }
}

public sealed class FloatRandomDataSource : RandomDataSourceBase<float>
{
    public FloatRandomDataSource(int count)
        : base(count) { }

    public FloatRandomDataSource(float min, float max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextFloat(Min, Max) : random.NextFloat();
    }
}

public sealed class DoubleRandomDataSource : RandomDataSourceBase<double>
{
    public DoubleRandomDataSource(int count)
        : base(count) { }

    public DoubleRandomDataSource(double min, double max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextDouble(Min, Max) : random.NextDouble();
    }
}

public sealed class DecimalRandomDataSource : RandomDataSourceBase<decimal>
{
    public DecimalRandomDataSource(int count)
        : base(count) { }

    public DecimalRandomDataSource(decimal min, decimal max, int count)
        : base(min, max, count) { }

    protected override object GetNextValue(Randomizer random)
    {
        return InRange ? random.NextDecimal(Min, Max) : random.NextDecimal();
    }
}

public sealed class EnumRandomDataSource(int count, Type type) : RandomDataSourceBase(count, type)
{
    protected override object GetNextValue(Randomizer random)
    {
        return random.NextEnum(Type);
    }
}

public sealed class GuidRandomDataSource(int count) : RandomDataSourceBase(count, typeof(Guid))
{
    protected override object GetNextValue(Randomizer random)
    {
        return Guid.NewGuid();
    }
}
