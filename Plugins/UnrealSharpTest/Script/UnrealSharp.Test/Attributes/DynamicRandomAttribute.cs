using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnrealSharp.Test.Runner;

namespace UnrealSharp.Test.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class DynamicRandomAttribute : NUnitAttribute
{
    private IRandomDataSource? _dataSource;
    public int Count { get; }
    
    public bool Distinct { get; init; }

    public DynamicRandomAttribute(int count = 1)
    {
        Count = count;
    }

    public DynamicRandomAttribute(int min, int max, int count = 1)
    {
        Count = count;
        _dataSource = new IntRandomDataSource(min, max, count);
    }
    
    public DynamicRandomAttribute(uint min, uint max, int count = 1)
    {
        Count = count;
        _dataSource = new UIntRandomDataSource(min, max, count);
    }

    public DynamicRandomAttribute(short min, short max, int count = 1)
    {
        Count = count;
        _dataSource = new ShortRandomDataSource(min, max, count);
    }
    
    public DynamicRandomAttribute(ushort min, ushort max, int count = 1)
    {
        Count = count;
        _dataSource = new UShortRandomDataSource(min, max, count);
    }

    public DynamicRandomAttribute(byte min, byte max, int count = 1)
    {
        Count = count;
        _dataSource = new ByteRandomDataSource(min, max, count);
    }
    
    public DynamicRandomAttribute(sbyte min, sbyte max, int count = 1)
    {
        Count = count;
        _dataSource = new SByteRandomDataSource(min, max, count);
    }

    public DynamicRandomAttribute(long min, long max, int count = 1)
    {
        Count = count;
        _dataSource = new LongRandomDataSource(min, max, count);
    }
    
    public DynamicRandomAttribute(ulong min, ulong max, int count = 1)
    {
        Count = count;
        _dataSource = new ULongRandomDataSource(min, max, count);
    }

    public DynamicRandomAttribute(float min, float max, int count = 1)
    {
        Count = count;
        _dataSource = new FloatRandomDataSource(min, max, count);
    }
    
    public DynamicRandomAttribute(double min, double max, int count = 1)
    {
        Count = count;
        _dataSource = new DoubleRandomDataSource(min, max, count);
    }

    public DynamicRandomAttribute(decimal min, decimal max, int count = 1)
    {
        Count = count;
        _dataSource = new DecimalRandomDataSource(min, max, count);
    }

    public IRandomDataSource GetDataSource(ParameterInfo parameterInfo)
    {
        if (_dataSource is not null)
        {
            return _dataSource;
        }

        if (parameterInfo.ParameterType == typeof(sbyte))
        {
            _dataSource = new SByteRandomDataSource(sbyte.MinValue, sbyte.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(byte))
        {
            _dataSource = new ByteRandomDataSource(byte.MinValue, byte.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(short))
        {
            _dataSource = new ShortRandomDataSource(short.MinValue, short.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(ushort))
        {
            _dataSource = new UShortRandomDataSource(ushort.MinValue, ushort.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(int))
        {
            _dataSource = new IntRandomDataSource(int.MinValue, int.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(uint))
        {
            _dataSource = new UIntRandomDataSource(uint.MinValue, uint.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(long))
        {
            _dataSource = new LongRandomDataSource(long.MinValue, long.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(ulong))
        {
            _dataSource = new ULongRandomDataSource(ulong.MinValue, ulong.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(float))
        {
            _dataSource = new FloatRandomDataSource(float.MinValue, float.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(double))
        {
            _dataSource = new DoubleRandomDataSource(double.MinValue, double.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType == typeof(decimal))
        {
            _dataSource = new DecimalRandomDataSource(decimal.MinValue, decimal.MaxValue, Count);
        }
        else if (parameterInfo.ParameterType.IsEnum)
        {
            _dataSource = new EnumRandomDataSource(Count, parameterInfo.ParameterType);
        }
        else if (parameterInfo.ParameterType == typeof(Guid))
        {
            _dataSource = new GuidRandomDataSource(Count);
        }
        else
        {
            throw new InvalidOperationException($"Unsupported type {parameterInfo.ParameterType}");
        }
        
        return _dataSource!;
    }
    
    
}