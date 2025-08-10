using Pokemon.Editor.Model.Data.Pbs;

namespace Pokemon.Editor.Serializers.Pbs.Converters;

public class EvolutionMethodConverter : PbsConverterBase<EvolutionConditionInfo>
{
    public override string WriteCsvValue(EvolutionConditionInfo value, PbsScalarDescriptor schema, string? sectionName)
    {
        throw new NotImplementedException();
    }

    public override EvolutionConditionInfo GetCsvValue(string input, PbsScalarDescriptor scalarDescriptor, string? sectionName)
    {
        throw new NotImplementedException();
    }
}