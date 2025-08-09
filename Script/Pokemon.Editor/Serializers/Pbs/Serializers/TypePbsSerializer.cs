using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public class TypePbsSerializer : IGameDataEntrySerializer<UType>
{
    public FName FormatTag => "PBS";
    public FText FormatName => "PBS";
    public string FileExtensionText => "PBS file |*.txt|";
    
    public string SerializeData(IEnumerable<UType> entries)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UType> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler.CompilePbsFile<TypeInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToType(outer));
    }
}